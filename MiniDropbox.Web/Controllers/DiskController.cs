using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Amazon.S3.Model;
using Amazon.S3.Model;
using AutoMapper;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Entities;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;
using MiniDropbox.Web.Utils;
using NHibernate.Mapping;
using File = MiniDropbox.Domain.File;

namespace MiniDropbox.Web.Controllers
{
    public class DiskController : BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;

        public DiskController(IWriteOnlyRepository writeOnlyRepository, IReadOnlyRepository readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }


        [HttpGet]
        public ActionResult ListAllContent()
        {
            //var actualPath = Session["ActualPath"].ToString();
            IEnumerable<File> list = null;
            var userFiles = list;

            var actualFolder = Session["ActualFolder"].ToString();
            if (actualFolder == "Shared")
            {
                var temp = _readOnlyRepository.First<FileShared>(x => x.UserReceive == User.Identity.Name);
                if (temp != null)
                    userFiles = temp.Files;
            }
            else
                userFiles = _readOnlyRepository.First<Account>(x => x.EMail == User.Identity.Name).Files;

            var userContent = new List<DiskContentModel>();
            if (userFiles != null)
            {
                foreach (var file in userFiles)
                {

                    if (file == null)
                        continue;
                    var fileFolderArray = file.Url.Split('/');
                    var fileFolder = fileFolderArray.Length > 1
                        ? fileFolderArray[fileFolderArray.Length - 2]
                        : fileFolderArray.FirstOrDefault();




                    if (!file.IsArchived && fileFolder.Equals(actualFolder) && !string.Equals(file.Name, actualFolder))
                        userContent.Add(Mapper.Map<DiskContentModel>(file));
                    else if (!file.IsArchived && file.IsShared && actualFolder == "Shared")
                        userContent.Add(Mapper.Map<DiskContentModel>(file));
                }
            }

            if (userContent.Count == 0)
            {
                userContent.Add(new DiskContentModel
                {
                    Id = 0,
                    ModifiedDate = DateTime.Now.Date,
                    Name = "You can now add files to your account",
                    Type = "none"
                });
            }

            return View(userContent);
        }

        [HttpGet]
        public PartialViewResult FileUploadModal()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase fileControl)
        {
            if (fileControl == null)
            {
                Error("There was a problem uploading the file :( , please try again!!!");
                return RedirectToAction("ListAllContent");
            }
            
            var fileSize = fileControl.ContentLength;

            if (fileSize > 10485760)
            {
                Error("The file must be of 10 MB or less!!!");
                return RedirectToAction("ListAllContent");
            }
            fileUploader(fileControl,User.Identity.Name);
            AddActivity("El usuario ha subido el siguiente archivo "+fileControl.FileName);
            Success("File uploaded successfully!!! :D");
            return RedirectToAction("ListAllContent");
        }

        private bool fileUploader(HttpPostedFileBase fileControl, string user)
        {
            var userData = _readOnlyRepository.First<Account>(x => x.EMail == user);
            var actualPath = Session["ActualPath"].ToString();
            var fileName = Path.GetFileName(fileControl.FileName);

            //var serverFolderPath = Server.MapPath("~/App_Data/UploadedFiles/"+actualPath);
           
            //var directoryInfo = new DirectoryInfo(serverFolderPath);

            //if (!directoryInfo.Exists)
            //{
            //    directoryInfo.Create();
            //}

            //var sharedDirectory = new DirectoryInfo( Server.MapPath("~/App_Data/UploadedFiles/"+User.Identity.Name + "/Shared"));
            //if (!sharedDirectory.Exists)
            //{
            //    sharedDirectory.Create();
            //    userData.Files.Add(new File
            //    {
            //        Name = "Shared",
            //        CreatedDate = DateTime.Now,
            //        ModifiedDate = DateTime.Now,
            //        FileSize = 0,
            //        Type = "",
            //        Url = sharedDirectory.FullName,
            //        IsArchived = false,
            //        IsDirectory = true
            //    });
            //}

            //var path = Path.Combine(serverFolderPath, fileName);

            //var fileInfo = new DirectoryInfo(serverFolderPath+fileName);

            if (userData.Files.Count(l=>l.Name==fileName && l.Url.EndsWith(actualPath) && !l.IsArchived)>0)//Actualizar Info Archivo
            {
                var bddInfo = userData.Files.FirstOrDefault(f => f.Name == fileName);
                bddInfo.ModifiedDate = DateTime.Now;
                bddInfo.Type = fileControl.ContentType;
                bddInfo.FileSize = fileControl.ContentLength;
                _writeOnlyRepository.Update(bddInfo);
            }
            else
            {
                userData.Files.Add(new File
                {
                    Name = fileName,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    FileSize = fileControl.ContentLength,
                    Type = fileControl.ContentType,
                    Url = actualPath,
                    IsArchived = false,
                    IsDirectory = false
                });
                _writeOnlyRepository.Update(userData);
            }

            //fileControl.SaveAs(path);
            var putObjectRequest = new PutObjectRequest
            {
                BucketName = userData.BucketName,
                Key = actualPath + fileName,
                InputStream = fileControl.InputStream
            };



            AWSClient.PutObject(putObjectRequest);


            return true;
            
        }

        public ActionResult DeleteFile(int fileId)
        {
            var userData = _readOnlyRepository.First<Account>(a => a.EMail == User.Identity.Name);
            var fileToDelete = userData.Files.FirstOrDefault(f => f.Id == fileId);

            if (fileToDelete != null)
            {
                if (!fileToDelete.IsDirectory)
                {
                    var deleteRequest = new DeleteObjectRequest
                    {
                        BucketName = userData.BucketName,
                        Key = fileToDelete.Url + fileToDelete.Name
                    };
                    AWSClient.DeleteObject(deleteRequest);
                    fileToDelete.IsArchived = true;
                }
                else//Borrar carpeta con todos sus archivos
                {
                    DeleteFolder(fileToDelete.Id);
                    //var filesList = new List<KeyVersion>();
                    //var userFiles = userData.Files;

                    //foreach (var file in userFiles)
                    //{
                    //    if (file == null)
                    //        continue;

                    //    var fileFolderArray = file.Url.Split('/');
                    //    var fileFolder = fileFolderArray.Length > 1 ? fileFolderArray[fileFolderArray.Length - 2] : fileFolderArray.FirstOrDefault();

                    //    if (!file.IsArchived && fileFolder.Equals(fileToDelete.Name) &&
                    //        !string.Equals(file.Name, fileToDelete.Name))
                    //    {
                    //        filesList.Add(!file.IsDirectory ? new KeyVersion(file.Url + file.Name) : new KeyVersion(file.Url+file.Name + "/"));
                    //        file.IsArchived = true;
                    //    }

                    fileToDelete.IsArchived = true;
                    //}

                    //filesList.Add(new KeyVersion(fileToDelete.Name+"/"));

                    //var deleteRequest = new DeleteObjectsRequest
                    //{
                    //    BucketName = userData.BucketName,
                    //    Keys = filesList
                    //};

                    //AWSClient.DeleteObjects(deleteRequest);

                        var deleteRequest = new DeleteObjectRequest
                        {
                            BucketName = userData.BucketName,
                            Key = fileToDelete.Url + fileToDelete.Name+"/"
                        };
                        AWSClient.DeleteObject(deleteRequest);
                    AddActivity("El usuario ha borrado el siguiente archivo "+fileToDelete.Name);
                }
                _writeOnlyRepository.Update(userData);
            }

            return RedirectToAction("ListAllContent");
        }

        [HttpPost]
        public ActionResult CreateFolder(string folderName)
        {
            if (folderName.Length > 25)
            {
                Error("Folder name should be 25 characters maximum!!!");
                return RedirectToAction("ListAllContent");
            }

            var userData = _readOnlyRepository.First<Account>(x => x.EMail == User.Identity.Name);
            
            if (userData.Files.Count(l=>l.Name==folderName)>0)
            {
                Error("Folder already exists!!!");
                return RedirectToAction("ListAllContent");
            }

            var actualPath = Session["ActualPath"].ToString();

            var putFolder = new PutObjectRequest { BucketName = userData.BucketName, Key = actualPath+folderName+"/", ContentBody = string.Empty };
            AWSClient.PutObject(putFolder);
            AddActivity("El usuario ha creado el folder "+folderName);
            //var serverFolderPath = Server.MapPath("~/App_Data/UploadedFiles/" + actualPath + "/"+folderName);

            //var folderInfo = new DirectoryInfo(serverFolderPath);

            //if (folderInfo.Exists)
            //{
            //    Error("Folder already exists!!!");
            //    return RedirectToAction("ListAllContent");
            //}

            
            userData.Files.Add(new File
            {
                Name = folderName,
                CreatedDate = DateTime.Now,
                FileSize = 0,
                IsArchived = false,
                IsDirectory = true,
                ModifiedDate = DateTime.Now,
                Type = "",
                Url = actualPath
            });

            //var result=Directory.CreateDirectory(serverFolderPath);

            //if(!result.Exists)
            //    Error("The folder was not created!!! Try again please!!!");
            //else
            //{
                Success("The folder was created successfully!!!");
                _writeOnlyRepository.Update(userData);
            //}

            return RedirectToAction("ListAllContent");
        }

        public void DeleteFolder(long folderId)
        {
            var userData = _readOnlyRepository.First<Account>(a => a.EMail == User.Identity.Name);
            var folderToDelete = userData.Files.FirstOrDefault(f => f.Id == folderId);

            //if (!folderToDelete.IsDirectory)
            //    {
            //        var deleteRequest = new DeleteObjectRequest
            //        {
            //            BucketName = userData.BucketName,
            //            Key = fileToDelete.Url + fileToDelete.Name
            //        };
            //        AWSClient.DeleteObject(deleteRequest);
            //        fileToDelete.IsArchived = true;
            //    }
            //    else
            //    {
                    var userFiles = userData.Files.Where(t=>t.Url.Contains(folderToDelete.Name));

                    foreach (var file in userFiles)
                    {
                        if (file == null)
                            continue;

                        if(file.IsDirectory)
                            DeleteFolder(file.Id);
                        
                        var fileFolderArray = file.Url.Split('/');
                        var fileFolder = fileFolderArray.Length > 1 ? fileFolderArray[fileFolderArray.Length - 2] : fileFolderArray.FirstOrDefault();

                        if (!file.IsArchived && fileFolder.Equals(folderToDelete.Name) &&
                            !string.Equals(file.Name, folderToDelete.Name))
                        {
                            var deleteRequest = new DeleteObjectRequest
                            {
                                BucketName = userData.BucketName,
                                Key = file.Url + file.Name
                            };
                            AWSClient.DeleteObject(deleteRequest);
                            AddActivity("El usuario ha borrado el folder "+folderToDelete.Name);
                            file.IsArchived = true;
                            _writeOnlyRepository.Update(userData);
                        }
                    }

                    folderToDelete.IsArchived = true;
                    var deleteRequest2 = new DeleteObjectRequest
                    {
                        BucketName = userData.BucketName,
                        Key = folderToDelete.Url + folderToDelete.Name + "/"
                    };
                    AWSClient.DeleteObject(deleteRequest2);
                    _writeOnlyRepository.Update(userData);
            //}
            

        }
        
        public ActionResult ListFolderContent(string folderName)
        {
            Session["ActualPath"] += folderName + "/";
            Session["ActualFolder"] = folderName;
            return RedirectToAction("ListAllContent");
        }
        public ActionResult ListFolderContent2(string path, string folderName)
        {
            if (path == null)
            {
                path = "";
            }
            Session["ActualPath"] = path;
            Session["ActualFolder"] = folderName;
            return RedirectToAction("ListAllContent");
        }

        public ActionResult ListAllContentRoot()
        {
            Session["ActualPath"] = string.Empty;
            Session["ActualFolder"] = string.Empty;
            return RedirectToAction("ListAllContent");
        }

        public ActionResult DownloadFile(int fileId)
        {
            var userData =_readOnlyRepository.First<Account>(a => a.EMail == User.Identity.Name);
            var fileData = userData.Files.FirstOrDefault(f => f.Id == fileId);

            var objectRequest = new GetObjectRequest{BucketName =userData.BucketName,Key = fileData.Url+fileData.Name};
            var file=AWSClient.GetObject(objectRequest);
            var byteArray = new byte[file.ContentLength];
            file.ResponseStream.Read(byteArray, 0,(int)file.ContentLength);
            //var template_file = System.IO.File.ReadAllBytes();

            return new FileContentResult(byteArray, fileData.Type)
            {
            FileDownloadName = fileData.Name};
}
        [HttpGet]
        public PartialViewResult Shared(int fileId)
        {
            Session["IDARCHIVOCOMPARTIR"] = fileId;
            return PartialView(new FolderPublicoModel2());
        }

        [HttpPost]
        public ActionResult Shared(FolderPublicoModel2 model)
        {
            var file = _readOnlyRepository.First<File>(x => x.Id == Convert.ToInt64(Session["IDARCHIVOCOMPARTIR"]));

            var exisfshared = _readOnlyRepository.First<FileShared>(x => x.UserReceive == model.Email);
            if (exisfshared != null)
            {
                file.FileShared_id = exisfshared.Id;
                file.IsShared = true;
                _writeOnlyRepository.Update<File>(file);
            }
            else
            {
                var fshared = new FileShared();
                fshared.AddFile(file);
                fshared.IsArchived = false;
                fshared.UserShared = User.Identity.Name;
                fshared.UserReceive = model.Email;
                var cfshared = _writeOnlyRepository.Create<FileShared>(fshared);

                file.FileShared_id = cfshared.Id;
                file.IsShared = true;
                _writeOnlyRepository.Update<File>(file);
                AddActivity("El usuario ha compartido el siguiente archivo: "+file.Name);
            }

            #region Envio de Mail o Invitacion

            var accoun = _readOnlyRepository.First<Account>(x => x.EMail == model.Email);
            var accounActual = _readOnlyRepository.First<Account>(x => x.EMail == User.Identity.Name);
            if (accoun != null)
            {

                var emailBody = new StringBuilder("<b>Se ha compartido el archivo: </b>");
                emailBody.Append("<b>" + accounActual.Name + " " + accounActual.LastName + "</b>");
                emailBody.Append(
                    "<b> Se ha compartido el archivo: " + file.Name + "!!" + "</b>");
                emailBody.Append("<br/>");
                emailBody.Append(
                    "<b>Ingrese a MiniDropbox, ingrese en su carpeta 'Shared' para descargarlo.!!! </b>");

                emailBody.Append("<br/>");
                emailBody.Append("<br/>");
                emailBody.Append("<br/>");

                if (MailSender.SendEmail(model.Email, "Join Mini DropBox", emailBody.ToString()))
                {
                    Success("Shared sent successfully!!");
                    return PartialView(model);
                }
                Error("E-Mail couldn't be sent!!!!");
            }
            else
            {
                var userData = _readOnlyRepository.First<Account>(x => x.EMail == User.Identity.Name);

                var emailBody = new StringBuilder("<b>Your friend </b>");
                emailBody.Append("<b>" + userData.Name + " " + userData.LastName + "</b>");
                emailBody.Append(
                    "<b> Se ha compartido el archivo: " + file.Name + "!!" + "</b>");
                emailBody.Append(
                    "<b> wants you to join to MiniDropBox, a site where you can store your files in the cloud!!</b>");
                emailBody.Append("<br/>");
                emailBody.Append(
                    "<b>To register in the site just click on the link below and fill up a quick form! Enjoy!!! </b>");

                //emailBody.Append("http://minidropbox-1.apphb.com/AccountSignUp/AccountSignUp?token=" + userId);
                emailBody.Append("http://localhost:1840/AccountSignUp/AccountSignUp?token=" + userData.Id);
                emailBody.Append("<br/>");
                emailBody.Append("<br/>");
                emailBody.Append("<br/>");

                if (MailSender.SendEmail(model.Email, "Join Mini DropBox", emailBody.ToString()))
                {
                    Success("E-Mail sent successfully!!");
                    return PartialView(model);
                }
                Error("E-Mail couldn't be sent!!!!");
            }

            #endregion

            Success("Archivo Compartido Correctamente");
            return RedirectToAction("ListAllContent");
        }

        [HttpGet]
        public ActionResult Stopshared(long id)
        {
            var file = _readOnlyRepository.First<File>(x => x.Id == id);
            if (file != null)
            {
                file.IsShared = false;
                _writeOnlyRepository.Update<File>(file);
                Success("Recurso se dejo de compartir correctamente");
            }
            return RedirectToAction("ListAllContent");
        }

        [HttpGet]
        public PartialViewResult PublicFolder(long fileId)
        {
            Session["IDARCHIVOCOMPARTIRPUBLICO"] = fileId;
            return PartialView(new FolderPublicoModel());
        }

        [HttpPost]
        public ActionResult PublicFolder(FolderPublicoModel model)
        {
            var account = _readOnlyRepository.First<Account>(x => x.EMail == User.Identity.Name);
            var file =
                _readOnlyRepository.First<File>(x => x.Id == Convert.ToInt64(Session["IDARCHIVOCOMPARTIRPUBLICO"]));

            var fpublico = new PublicFolder();
            fpublico.Account_Id = account.Id; // este se podria cambiar por el email
            fpublico.File_Id = file.Id;
            fpublico.IsArchived = false;
            fpublico.Token = System.Guid.NewGuid().ToString();
            var publicFolder = _writeOnlyRepository.Create<PublicFolder>(fpublico);

            file.IsShared = true;
            file.PublicFolder_id = publicFolder.Id;
            _writeOnlyRepository.Update<File>(file);

        var emailBody = new StringBuilder("<b>Se ha compartido carpeta por: </b>");
            emailBody.Append("<b>" + account.Name + " " + account.LastName + "</b>");
            emailBody.Append(
                "<b> Se ha compartido la carpeta : " + file.Name + "!!" + "</b>");
            emailBody.Append("<br/>");
            emailBody.Append(
                "Ingrese a este link para ver los archivos de la carpeta: http://localhost:1840/Disk/SeePublicShare?Token=" +
                fpublico.Token + "</b>");

            emailBody.Append("<br/>");
            emailBody.Append("<br/>");
            emailBody.Append("<br/>");

            if (MailSender.SendEmail(model.Email, "Shared Mini DropBox", emailBody.ToString()))
            {
                Success("Shared sent successfully!!");
                return PartialView(model);
            }
            Error("E-Mail couldn't be sent!!!!");

            Success("Folder Compartido Correctamente");
            return RedirectToAction("ListAllContent");
        }
        
        [HttpGet]
        public ActionResult SeePublicShare(string Token)
        {
            var userContent = new List<DiskContentModel>();

            if (Token != null)
            {
                var userFiles = _readOnlyRepository.First<PublicFolder>(x => x.Token == Token).Files;
                if (userFiles != null)
                {

                    foreach (var file in userFiles)
                    {

                        if (file == null)
                            continue;

                        if (!file.IsArchived && file.IsShared)
                        {
                            var fileFolder = file.Url.Split('\\').LastOrDefault();
                            ViewData["NombrePublic"] = fileFolder;
                            if (fileFolder != null)
                            {
                                userContent.Add(Mapper.Map<DiskContentModel>(file));
                            }
                        }
                    }
                    if (userContent.Count == 0)
                    {
                        userContent.Add(new DiskContentModel
                        {
                            Id = 0,
                            ModifiedDate = DateTime.Now.Date,
                            Name = "You can now add files to your account",
                            Type = "none"
                        });
                    }
                    else
                    {
                        Error("Recurso No existe");
                        return RedirectToAction("Logout", "Account");
                    }
                }
                else
                {
                    Error("Recurso No existe");
                    return RedirectToAction("Logout", "Account");
                }
            }
            else
            {
                Error("Recurso No existe");
                return RedirectToAction("Logout", "Account");
            }
            return View(userContent);
        }
        public void AddActivity(string actividad)
        {
            var account = _readOnlyRepository.First<Account>(x => x.EMail == User.Identity.Name);
            var act = new Actividades();
            act.Actividad = actividad;
            act.hora = DateTime.Now;
            account.History.Add(act);
            _writeOnlyRepository.Update(account);

        }

        public ActionResult ShowFile(int id)
        {
            var ar = new MostrarArchivosModel();
            ar.id = id;
            ar.file = Download(id);
           
            return PartialView(ar);
        }

        public FileContentResult Download(long id)
        {
            var userData = _readOnlyRepository.First<Account>(a => a.EMail == User.Identity.Name);
            var fileData = userData.Files.FirstOrDefault(f => f.Id == id);

            var objectRequest = new GetObjectRequest
            {
                BucketName = userData.BucketName,
                Key = fileData.Url + fileData.Name
            };
            var file = AWSClient.GetObject(objectRequest);
            var byteArray = new byte[file.ContentLength];
            file.ResponseStream.Read(byteArray, 0, (int) file.ContentLength);
            
            //var template_file = System.IO.File.ReadAllBytes();

            return new FileContentResult(byteArray, fileData.Type);


            // Assuming that the resume is an MS Word document...
            // return File(t.FileStream, fileData.Type);
        }
        public ActionResult DownloadAudio(long id)
        {
            var userData = _readOnlyRepository.First<Account>(a => a.EMail == User.Identity.Name);
            var fileData = userData.Files.FirstOrDefault(f => f.Id == id);

            var objectRequest = new GetObjectRequest
            {
                BucketName = userData.BucketName,
                Key = fileData.Url + fileData.Name
            };
            var file = AWSClient.GetObject(objectRequest);
            var byteArray = new byte[file.ContentLength];
            file.ResponseStream.Read(byteArray, 0, (int)file.ContentLength);
            //var template_file = System.IO.File.ReadAllBytes();
            
            return File(byteArray, "audio/mp3");


            // Assuming that the resume is an MS Word document...
            // return File(t.FileStream, fileData.Type);
        }

        public ActionResult Checked(int id)
        {
            return View();
        }
    }
}
