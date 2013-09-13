using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
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
        public IEnumerable<string> Get([FromUri]string token)
        {
            var account = CheckPermissions(token);
            if (CheckCuenta(account))
            {
                //Mandar todos los archivos de la cuenta
            }
            return new string[] { "value1", "value2", "value3" };
        }

        // GET api/files/5
        public FileModel Get([FromUri]int folderid, [FromUri]string token)
        {
            var account = CheckPermissions(token);
            if (CheckCuenta(account))
            {
                
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
            if (access != null)
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
