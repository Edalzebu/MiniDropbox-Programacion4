using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AutoMapper;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Entities;
using MiniDropbox.Web.Models;
using MiniDropbox.Web.Models.Api;
using MiniDropbox.Domain.Services;

namespace MiniDropbox.Web.Controllers.API
{
    public class FilesController : ApiController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;

        public FilesController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }

        // GET api/files     
        public List<DiskContentModel> Get([FromUri]string token)
        {
            var account = CheckPermissions(token);
            var userContent = new List<DiskContentModel>();
            if (CheckCuenta(account))
            {
                userContent=ListRootFolder(account);
            }
            return userContent;
        }

        // GET api/files/5
        public List<DiskContentModel> Get([FromBody]string currentPath, [FromUri]string token)
        {
            var account = CheckPermissions(token);
            if (CheckCuenta(account))
            {
                return ListFolder(currentPath, account);
                //Mandar el File con ese ID si le pertenece
            }
            return null;
        }

        public bool CreateFolder()
        {
            return false;
        }

        // POST api/files
        public string Post([FromBody]string dirAt, string name) // Para crear una carpeta
        {
            if (CreateFolder())
            {
                return name+" ha sido creado.";
            }
            return "test";

        }

        // PUT api/files/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/files/5
        public void Delete(int id)
        {

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
    }
}
