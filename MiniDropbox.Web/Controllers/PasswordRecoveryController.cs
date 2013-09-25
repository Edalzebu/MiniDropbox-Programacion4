using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;
using MiniDropbox.Web.Utils;

namespace MiniDropbox.Web.Controllers
{
    public class PasswordRecoveryController : BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;

        public PasswordRecoveryController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }

        [HttpGet]
        public ActionResult PasswordRecovery()
        {
            return View(new PasswordRecoveryModel());
        }

        public ActionResult Cancel()
        {
            return Session["userId"]==null ? RedirectToAction("LogIn", "Account") : RedirectToAction("ListAllContent", "Disk");
        }

        [HttpPost]
        public ActionResult PasswordRecovery(PasswordRecoveryModel model)
        {
            var result = _readOnlyRepository.First<Account>(a => a.EMail == model.EMailAddress);

            if (result != null)
            {
                var fechaActual = DateTime.Now.Date;

                var pass = result.Password;
                var data = ""+fechaActual.Day + fechaActual.Month + fechaActual.Year;
                var token =pass+";"+ EncriptacionMD5.Encriptar(data);

                //var url = "http://minidropbox-1.apphb.com/PasswordReset/PasswordReset";
                var url = "http://minidropboxclase.apphb.com/PasswordReset/PasswordReset";

                var emailBody = new StringBuilder("<b>Go to the following link to change your password: </b>");
                emailBody.Append("<br/>");
                emailBody.Append("<br/>");
                emailBody.Append("<b>" + url + "?token=" +token + "<b>");
                emailBody.Append("<br/>");
                emailBody.Append("<br/>");
                emailBody.Append("<b>This link is only valid through " + fechaActual.Day + "/" + fechaActual.Month + "/" + fechaActual.Year + "</b>");

                AddActivity("Se ha hecho una peticion de recuperar contrasena",result);
                if (MailSender.SendEmail(model.EMailAddress,"Password Recovery" ,emailBody.ToString()))
                    return Cancel();
               
                Error("E-Mail failed to be sent, please try again!!!");
                return View(model);
               
            }

            Error("E-Mail address is not registered in this site!!!");
            return View(model);
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