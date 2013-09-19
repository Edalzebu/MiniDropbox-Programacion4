using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Amazon;
using Amazon.S3;
using Newtonsoft.Json;
using Amazon.S3.Model;
using AutoMapper;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Entities;
using MiniDropbox.Web.Models;
using MiniDropbox.Web.Models.Api;
using MiniDropbox.Domain.Services;
using File = MiniDropbox.Domain.File;
using BootstrapMvcSample.Controllers;

namespace MiniDropbox.Web.Controllers.API
{
    public class FilesController : ApiController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;
        public readonly AmazonS3 AWSClient = AWSClientFactory.CreateAmazonS3Client();

        public FilesController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {

            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }

        // GET api/files/5
        public string Get([FromUri]int Id, [FromUri]string token)
        {
            var account = CheckPermissions(token);
            if (CheckCuenta(account))
            {
                return JsonConvert.SerializeObject(DownloadFile(Id, account)); // HttpResponnsemessage
                //Mandar el File con ese ID si le pertenece
            }
            return null;
        }

        

        // POST api/files
        public bool Post([FromBody]UploadFileModel model, [FromUri] string token) // Para subir un archivo
        {

            var account = CheckPermissions(token);
            if (model != null)
            {
                if (CheckCuenta(account))
                {
                    if (UploadFile(model))
                    {
                        return true;
                    }
                }
            }
            
            return false;

        }

       // DELETE api/files/5
        public bool Delete([FromUri]int id, [FromUri]string token)
        {
            var account = CheckPermissions(token);
            if (CheckCuenta(account))
            {
                if (DeleteFile(id, account))
                {
                    return true;
                }
                //Mandar el File con ese ID si le pertenece
            }
            return false;
        }

        private Account CheckPermissions(string token) // Hace un check si el token existe, si existe devuelve una cuenta, sino null;
        {
            var access = _readOnlyRepository.First<ApiKeys>(x => x.Token == token);
            if (access.IsTokenActive())
            {
                
                var account = _readOnlyRepository.First<Account>(x => x.Id == access.UserId);
                return account;
                
            }

            return null;
        }

        private static bool CheckCuenta(Account cuenta)
        {
            if (cuenta != null)
                return true;
            return false;
           
        }

        private HttpResponseMessage DownloadFile(int fileId, Account userData)
        {
            var fileData = _readOnlyRepository.First<File>(x => x.Id == fileId);
            var objectRequest = new GetObjectRequest { BucketName = userData.BucketName, Key = fileData.Url + fileData.Name };
            var file = AWSClient.GetObject(objectRequest);
            var byteArray = new byte[file.ContentLength];
            file.ResponseStream.Read(byteArray, 0, (int)file.ContentLength);
            
            HttpResponseMessage response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            response.Content = new StreamContent(file.ResponseStream);
            return response;
        }

        private bool DeleteFile(int fileId , Account userData)
        {
            
            var fileToDelete = userData.Files.FirstOrDefault(f => f.Id == fileId);

            if (fileToDelete != null)
            {
                if (!fileToDelete.IsDirectory)
                {
                    var deleteRequest = new DeleteObjectRequest { BucketName = userData.BucketName, Key = fileToDelete.Url + fileToDelete.Name };
                    AWSClient.DeleteObject(deleteRequest);
                }

                //Borrar carpetas, mandar mensaje de confirmacion para eliminar cuando esta vacia y cuando contiene algun archivo

                //System.IO.File.Delete(fileToDelete.Url+fileToDelete.Name);

                fileToDelete.IsArchived = true;

                _writeOnlyRepository.Update(userData);
                return true;
            }
            return false;
        }
        private bool UploadFile(UploadFileModel model)
        {
            var fileControl = model.file;
            if (fileControl == null)
            {
                return false;
            }

            var fileSize = fileControl.ContentLength;

            if (fileSize > 10485760)
            {
                return false;
            }

            var userData = _readOnlyRepository.First<Account>(x => x.EMail == User.Identity.Name);
            var actualPath = model.CurrentPath;
            var fileName = Path.GetFileName(fileControl.FileName);

            

            if (userData.Files.Count(l => l.Name == fileName) > 0)//Actualizar Info Archivo
            {
                var bddInfo = userData.Files.FirstOrDefault(f => f.Name == fileName);
                bddInfo.ModifiedDate = DateTime.Now;
                bddInfo.Type = fileControl.ContentType;
                bddInfo.FileSize = fileSize;
                _writeOnlyRepository.Update(bddInfo);
            }
            else
            {
                userData.Files.Add(new File
                {
                    Name = fileName,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    FileSize = fileSize,
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
    }
}
