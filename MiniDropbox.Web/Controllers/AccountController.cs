using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;


namespace MiniDropbox.Web.Controllers
{
    public class AccountController : BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;

        public AccountController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View(new AccountLoginModel());
        }

        [HttpPost]
        public ActionResult LogIn(AccountLoginModel model)
        {
            var passwordEncripted = EncriptacionMD5.Encriptar(model.Password);
            var result =
                _readOnlyRepository.First<Account>(x => x.EMail == model.EMail && x.Password == passwordEncripted);

            if (result != null)
            {
                if (result.IsBlocked)
                {
                    Error(
                        "Your account has been blocked by the Admin due to violation of the terms of usage of this site!");
                    return View();
                }

                if (!result.Isconfirmed)
                {
                    Error(
                        "Your account has not been confirmed!");
                    return View();
                }

                if (result.IsArchived)
                {
                    Error("Your account is inactive, to activate it again send an e-mail to support@minidropbox.com");
                    return View();
                }

                var roles = result.IsAdmin
                    ? new List<string>(new[] {"Admin"})
                    : new List<string>(new[] {"User"});

                FormsAuthentication.SetAuthCookie(model.EMail, model.RememberMe);
                SetAuthenticationCookie(model.EMail, roles);


                if (result.IsAdmin)
                {
                    return RedirectToAction("RegisteredUsersList", "RegisteredUsersList");
                }

                Session["ActualPath"] = result.EMail;
                Session["ActualFolder"] = result.EMail;
                return RedirectToAction("ListAllContent", "Disk");
            }


            Error("E-Mail or Password is incorrect!!!");
            return View();
        }

        public ActionResult SignUp()
        {
            return RedirectToAction("AccountSignUp", "AccountSignUp", new {token = 0});
        }

        public ActionResult PasswordRecovery()
        {
            return RedirectToAction("PasswordRecovery", "PasswordRecovery");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LogIn", "Account");
        }

        [HttpGet]
        public ActionResult Confirmed(string token)
        {
            if (token == "ErrorPostback")
            {
                Error("Link has expired!!!");
                return RedirectToAction("LogIn");
            }

            var fechaActual = DateTime.Now.Date;

            var data = token.Split(';');
            var password = data[0];
            var linkDate = data[1];

            var currentDate = "" + fechaActual.Day + fechaActual.Month + fechaActual.Year;
            var currentDateMd5 = EncriptacionMD5.Encriptar(currentDate);

            var user = _readOnlyRepository.Query<Account>(a => a.Password == password);
            var model = new AccountLoginModel();

            var firstOrDefault = user.FirstOrDefault();
            if (firstOrDefault != null)
                model.EMail = firstOrDefault.EMail;

            var orDefault = user.FirstOrDefault();
            if (orDefault != null)
                model.Password = orDefault.Password;

            if (linkDate == currentDateMd5 && user.Any())
            {

                var result =
                    _readOnlyRepository.First<Account>(x => x.EMail == model.EMail && x.Password == model.Password);

                if (result != null)
                {
                    if (result.IsBlocked)
                    {
                        Error(
                            "Your account has been blocked by the Admin due to violation of the terms of usage of this site!");
                        return RedirectToAction("LogIn");
                    }

                    if (result.Isconfirmed)
                    {
                        Error("Your account has already been confirmed");
                        return RedirectToAction("LogIn");
                    }

                    if (result.IsArchived)
                    {
                        Error("Your account is inactive, to activate it again send an e-mail to support@minidropbox.com");
                        return RedirectToAction("LogIn");
                    }

                    var roles = result.IsAdmin
                        ? new List<string>(new[] {"Admin"})
                        : new List<string>(new[] {"User"});

                    FormsAuthentication.SetAuthCookie(model.EMail, model.RememberMe);
                    SetAuthenticationCookie(model.EMail, roles);

                    Session["ActualPath"] = result.EMail;
                    Session["ActualFolder"] = result.EMail;

                    result.Isconfirmed = true;
                    _writeOnlyRepository.Update<Account>(result);

                    Success("Your Account it is Confirmed");
                    return View("Marketing");
                }

                return RedirectToAction("Confirmed", new {token = "ErrorPostBack"});
            }
            return RedirectToAction("LogIn");
        }
    }
}