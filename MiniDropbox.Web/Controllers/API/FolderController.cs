using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Amazon;
using Amazon.S3.Model;
using Amazon.S3;
using Newtonsoft.Json;
using System.Web.Http;
using AutoMapper;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Entities;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;
using MiniDropbox.Web.Models.Api;

namespace MiniDropbox.Web.Controllers.API
{
    public class FolderController : ApiController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;
        public readonly AmazonS3 AWSClient = AWSClientFactory.CreateAmazonS3Client();

        public FolderController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }

        // GET api/folder
        public string Get([FromUri] string token)
        {
            var account = CheckPermissions(token);
           var model = new FolderModel();
            
            if (CheckCuenta(account))
            {
                model.listaModels = ListRootFolder(account);
            }
            return JsonConvert.SerializeObject(model);
            
            
        }

        // POST api/folder
        public string Post([FromBody]string currentPath, [FromUri] string token) // Funcion para devolver cierto folder
        {
            var account = CheckPermissions(token);
            var model = new FolderModel();

            if (CheckCuenta(account))
            {
                model.listaModels = ListFolder(currentPath,account);
                return JsonConvert.SerializeObject(model);
            }
            return null;
        }

        // PUT api/folder/5
        public string Put([FromBody] CreateFolderModel model, [FromUri] string token) // funcion para crear un folder, devuelve la listqa del directorio
        {
            var account = CheckPermissions(token);
            if (CheckCuenta(account))
            {
                if (CreateFolder(model.currentPath, model.folderName))
                {
                    var modelo = new FolderModel();
                    modelo.listaModels = ListRootFolder(account);
                    return JsonConvert.SerializeObject(modelo);
                }
                
            }
            return null;
        }

        // DELETE api/folder/5
        public bool Delete([FromUri] string token , [FromUri]int id) // Borra un folder, y te retorna el root folder
        {
            var account = CheckPermissions(token);
            if (CheckCuenta(account))
            {
                var folder = _readOnlyRepository.First<File>(x => x.Id == id);
                if (folder != null)
                {
                    folder.IsArchived = true;
                    _writeOnlyRepository.Update(folder);
                    var model = new FolderModel();
                    model.listaModels = ListRootFolder(account);
                    return true;
                }
            }
            return false;
        }




        // funciones Auxiliares
        private bool CreateFolder(string path,string folderName)
        {
            if (folderName.Length > 25)
            {
                return false;
            }

            var userData = _readOnlyRepository.First<Account>(x => x.EMail == User.Identity.Name);

            if (userData.Files.Count(l => l.Name == folderName) > 0)
            {
                return false;
            }
            
            var actualPath = path;

            var putFolder = new PutObjectRequest { BucketName = userData.BucketName, Key = actualPath + folderName + "/", ContentBody = string.Empty };
            AWSClient.PutObject(putFolder);
            
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

            _writeOnlyRepository.Update(userData);
            return true;

        }
        private List<DiskContentModel> ListFolder(string currentPath, Account account)
        {
            var userData = _readOnlyRepository.First<Account>(x => x.EMail == account.EMail);
            var userContent = new List<DiskContentModel>();

            var actualFolder = currentPath;

            var userFiles = userData.Files;



            foreach (var file in userFiles)
            {
                if (file == null)
                    continue;

                var fileFolderArray = file.Url.Split('/');
                var fileFolder = fileFolderArray.Length > 1 ? fileFolderArray[fileFolderArray.Length - 2] : fileFolderArray.FirstOrDefault();

                if (!file.IsArchived && fileFolder.Equals(actualFolder) && !string.Equals(file.Name, actualFolder))
                    userContent.Add(Mapper.Map<DiskContentModel>(file));
            }
            //Mandar todos los archivos de la cuenta
            return userContent;

        }
        private List<DiskContentModel> ListRootFolder(Account account)
        {
            var userData = _readOnlyRepository.First<Account>(x => x.EMail == account.EMail);
            var userContent = new List<DiskContentModel>();

            var actualFolder = "";

            var userFiles = userData.Files;



            foreach (var file in userFiles)
            {
                if (file == null)
                    continue;

                var fileFolderArray = file.Url.Split('/');
                var fileFolder = fileFolderArray.Length > 1 ? fileFolderArray[fileFolderArray.Length - 2] : fileFolderArray.FirstOrDefault();

                if (!file.IsArchived && fileFolder.Equals(actualFolder) && !string.Equals(file.Name, actualFolder))
                    userContent.Add(Mapper.Map<DiskContentModel>(file));
            }
            //Mandar todos los archivos de la cuenta
            return userContent;
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
    }
}
