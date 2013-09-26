using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Amazon.S3.Model;
using AutoMapper;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;
using MiniDropbox.Web.Utils;
using File = System.IO.File;

namespace MiniDropbox.Web.Controllers
{
    public class AccountSignUpController : BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;
        

        public AccountSignUpController( IWriteOnlyRepository writeOnlyRepository, IReadOnlyRepository readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }
        
        [HttpGet]
        public ActionResult AccountSignUp(long token)
        {
            Session["userReferralId"] = token;
            
            return View(new AccountSignUpModel());
        }
       
        public ActionResult Cancelar()
        {
            return RedirectToAction("LogIn","Account");
        }

        [HttpPost]
        public ActionResult AccountSignUp(AccountSignUpModel model)
        {
            var result = _readOnlyRepository.Query<Account>(a=>a.EMail==model.EMail);

            if (result.Any())
            {
                Error("Email account is already registered in this site!!!");
                return View(model);
            }

            var account = Mapper.Map<Account>(model);
            account.IsArchived = false;
            account.IsAdmin = false;
            account.IsBlocked = false;
            account.SpaceLimit = 2408;
            account.Password = EncriptacionMD5.Encriptar(model.Password);
            account.Isconfirmed = false;
            account.BucketName = string.Format("mdp.{0}", Guid.NewGuid());

            //var account = new Account
            //{
            //    Name = accountModel.Name,
            //    LastName = accountModel.LastName,
            //    EMail = accountModel.EMail,
            //    IsArchived = false,
            //    IsBlocked = false,
            //    SpaceLimit = 500,
            //    UsedSpace = 0,
            //    Password = EncriptacionMD5.Encriptar(accountModel.Password) 
            //};
            //account.AddRole(new Role{Name = "User",IsArchived = false});

           var createdAccount= _writeOnlyRepository.Create(account);

            var token = Convert.ToInt64(Session["userReferralId"]);

            if (token != 0)
            {
                var userReferring = _readOnlyRepository.GetById<Account>(token);
                userReferring.Referrals.Add(createdAccount);
                _writeOnlyRepository.Update(userReferring);
            }

            var serverFolderPath = Server.MapPath("~/App_Data/UploadedFiles/" + account.EMail);
            Directory.CreateDirectory(serverFolderPath);
            
            var newBucket = new PutBucketRequest { BucketName = account.BucketName };
            AWSClient.PutBucket(newBucket);
           
            var putFolder = new PutObjectRequest{BucketName = account.BucketName, Key = "Shared/",ContentBody = string.Empty};
            AWSClient.PutObject(putFolder);

            var sharedDirectory =serverFolderPath + "/Shared";
            Directory.CreateDirectory(sharedDirectory);
            //var serverFolderPath = Server.MapPath("~/App_Data/UploadedFiles/" + account.EMail);
            //Directory.CreateDirectory(serverFolderPath);

            //var sharedDirectory =serverFolderPath + "/Shared";
            //Directory.CreateDirectory(sharedDirectory);

            if (createdAccount.Files == null)
            {
                createdAccount.Files= new List<Domain.File>();
            }
            if (createdAccount.History == null)
            {
                createdAccount.History= new List<Actividades>();
            }

            createdAccount.Files.Add(new Domain.File
            {
                CreatedDate = DateTime.Now,
                FileSize = 0,
                IsArchived = false,
                IsDirectory = true,
                Name = "Shared",
                Url = "",
                Type = "",
                ModifiedDate = DateTime.Now

            });
            _writeOnlyRepository.Update(createdAccount);





            AddActivity("El usuario se registro.", createdAccount);


            // ESTOOOOOOO
            #region EnvioCorreoParaNotificacion

            var fechaActual = DateTime.Now.Date;

            var pass = result.FirstOrDefault().Id;
            var data = "" + fechaActual.Day + fechaActual.Month + fechaActual.Year;
            var tokenConfir = pass + ";" + EncriptacionMD5.Encriptar(data);

            //var url = "http://minidropbox-1.apphb.com/PasswordReset/PasswordReset";
            var url = "http://minidropboxclase.apphb.com/Account/Confirmed";

            var emailBody = new StringBuilder("<b>Confirm your account of MiniDropbox</b>");
            emailBody.Append("<br/>");
            emailBody.Append("<br/>");
            emailBody.Append("<b>" + url + "?token=" + tokenConfir + "<b>");
            emailBody.Append("<br/>");
            emailBody.Append("<br/>");
            emailBody.Append("<b>This link is only valid through " + DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year + "</b>");

            if (MailSender.SendEmail(model.EMail, "Confirm your account of MiniDropbox", emailBody.ToString()))
                return Cancelar();

            Error("E-Mail failed to be sent, please try again!!!");
            return View(model);
            #endregion


            return Cancelar();
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
        public void AddActivity(string actividad, Account cuenta)
        {
            var account = _readOnlyRepository.First<Account>(x => x.EMail == cuenta.EMail);
            var act = new Actividades();
            act.Actividad = actividad;
            act.hora = DateTime.Now;
            account.History.Add(act);
            _writeOnlyRepository.Update(account);

        }

       

    }
}
