using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Entities;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;
using MiniDropbox.Web.Models.Api;

namespace MiniDropbox.Web.Controllers.API
{
    public class AuthController : ApiController
    {
        private const int MinsForTimeOut = 20;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;


        public AuthController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            if (readOnlyRepository == null)
            {
                throw new ArgumentNullException("readOnlyRepository");
            }
            if (writeOnlyRepository == null)
            {
                throw new ArgumentNullException("writeOnlyRepository");
            }
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }

        // GET api/auth
        public AccountProfileModel Get([FromUri] string token)
        {
            var account = CheckPermissions(token);
            if (checkCuenta(account))
            {
                return Mapper.Map<Account, AccountProfileModel>(account);
            }
            
            return null;
        }

        
        // POST api/auth
        public string Post([FromBody]AuthenticationModel authenticationModel)
        {

            if (checkCredenciales(authenticationModel.Username, authenticationModel.Password))
            {
                return CreateTokenForUser(authenticationModel.Username);
            }
            return "ERROR";
        }
        
        

       private bool checkCuenta(Account account)
        {
            if (account == null)
                return false;
            return true;
        }

        private bool checkCredenciales(string userName, string password)
        {
            var account = _readOnlyRepository.First<Account>(x => x.EMail == userName);
            if (checkCuenta(account))
            {

                if (account.Password == EncriptacionMD5.Encriptar(password))
                {
                    return true;
                }
            }
            return false;
        }

        private DateTime MinutesPermission()
        {
            return DateTime.Now.AddMinutes(MinsForTimeOut);
        }

        private string CreateTokenForUser(string userName)
        {
            var account = _readOnlyRepository.First<Account>(x => x.EMail == userName);
            if (checkCuenta(account))
            {
                var tokenString = EncriptacionMD5.Encriptar(userName) + GetHashCode();
                var key = new ApiKeys();
                key.ExpirationTime = MinutesPermission();
                key.UserId = account.Id;
                key.Token = tokenString;
                _writeOnlyRepository.Create(key);
                return key.Token;
            }
            return "Credenciales invalidas";
        }
        private Account CheckPermissions(string token) // Hace un check si el token existe, si existe devuelve una cuenta, sino null;
        {
            var access = _readOnlyRepository.First<ApiKeys>(x => x.Token == token);
            if (access.IsTokenActive())
            {
                if (access != null)
                {
                    var account = _readOnlyRepository.First<Account>(x => x.Id == access.UserId);
                    return account;
                }
            }
            
            return null;
        }

       
    }
}
