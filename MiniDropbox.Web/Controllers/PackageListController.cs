using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Entities;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;
using MiniDropbox.Web.Utils;
using NHibernate.Properties;

namespace MiniDropbox.Web.Controllers
{
    public class PackageListController : BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;

        public PackageListController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }

        //
        // GET: /PackageList/
        [HttpGet]
        public ActionResult PackageList()
        {
            //if (Session["userType"].ToString() != "Admin")
            //{
            //    return null;
            //}

            var packages = _readOnlyRepository.GetAll<Package>();
            var packagesModelList = new List<PackageModel>();

            if (!packages.Any())
            {
                packagesModelList.Add(new PackageModel {Name = "Create packages"});
                return View(packagesModelList);
            }

            foreach (var package in packages)
            {
                packagesModelList.Add(new PackageModel
                {
                    Id = package.Id,
                    IsArchived = package.IsArchived,
                    Name = package.Name,
                    Description = package.Description,
                    Price = package.Price,
                    DaysAvailable = package.DaysAvailable,
                    SpaceLimit = package.SpaceLimit
                });
            }

            return View(packagesModelList);
        }

        public ActionResult CreatePackage()
        {
            return RedirectToAction("CreateEditPackage", "CreateEditPackage", new {id = 0});
        }

        public ActionResult EditPackage(long packageId)
        {
            return RedirectToAction("CreateEditPackage", "CreateEditPackage", new {id = packageId});
        }

        public ActionResult DeactivatePackage(long packageId)
        {
            var packageData = _readOnlyRepository.GetById<Package>(packageId);

            packageData.IsArchived = !packageData.IsArchived;

            _writeOnlyRepository.Update(packageData);

            return RedirectToAction("PackageList");
        }

        [HttpGet]
        public ActionResult Succes(string tx, string st, string amt, string cm, string item_number)
        {
            Session["ActualFolder"]=User.Identity.Name;
            Session["ActualPath"] = User.Identity.Name;

            if (PostPaypal(tx,item_number))
            {
                Success("Pago Correcto, Disfrute de su paquete Premium..:=) ");
            }
            else
            {
                Error("Pago no se realizo, se activo una cuenta free");
            }
            return RedirectToAction("ListAllContent", "Disk");
        }

        internal bool PostPaypal(string tx, string producto)
        {
            var httpRequest = WebRequest.Create("https://www.sandbox.paypal.com/cgi-bin/webscr") as HttpWebRequest;
            if (httpRequest != null)
            {
                httpRequest.Method = "POST";
                httpRequest.ProtocolVersion = HttpVersion.Version11;
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                using (Stream requestStream = httpRequest.GetRequestStream())
                {
                    byte[] parametersBuffer =
                        Encoding.ASCII.GetBytes(
                            "cmd=_notify-synch&tx=" + tx +
                            "&at=wm1BBPJzvOKS7VC0u3pUVnlwwwHHlRBtYGNcowtpX-hOr6PLVQIIbeMhYQa");
                    requestStream.Write(parametersBuffer, 0, parametersBuffer.Length);
                }
                var httpResponse = httpRequest.GetResponse();
                var streamReader = new StreamReader(httpResponse.GetResponseStream());
                var resultHtml = streamReader.ReadToEnd();
                if (resultHtml.Contains("SUCCES"))
                {
                    var account = _readOnlyRepository.First<Account>(x => x.EMail == User.Identity.Name);
                    var _tranVent = new RecibosVentas
                    {
                        Transaccion = tx,
                        UserCompro = account.EMail,
                        IsArchived = false,
                        Fecha = DateTime.Now
                    };
                    
                    if (producto == "200 GB")
                    {
                        account.SpaceLimit = account.SpaceLimit + 204800;
                        _tranVent.Total = 30;
                        _tranVent.Descripcion = "Compra de 200 GB";
                    }
                    else if (producto == "500 GB")
                    {
                        account.SpaceLimit = account.SpaceLimit + 512000;
                        _tranVent.Total = 100;
                        _tranVent.Descripcion = "Compra de 500 GB";
                    }
                    _tranVent = _writeOnlyRepository.Create<RecibosVentas>(_tranVent);

                    account.RecibosVentas_Id = _tranVent.Id;

                    _writeOnlyRepository.Update<Account>(account);

                    return true;
                }
                else
                {
                    #region EnvioMail

                    var emailBody = new StringBuilder("<b>paypal payment failure, so active an account is free </b>");
                    emailBody.Append("<br/>");
                    emailBody.Append("<br/>");
                    emailBody.Append("<br/>");
                    emailBody.Append("<br/>");
                    emailBody.Append("<b>paypal payment failure, so active an account is free " + DateTime.Now.Day + "/" +
                                     DateTime.Now.Month +
                                     "/" + DateTime.Now.Year + "</b>");

                    MailSender.SendEmail(User.Identity.Name, "Activation Free", emailBody.ToString());

                    #endregion

                    return false;
                }
            }
            return false;
        }

        [HttpGet]
        public ActionResult Marketing()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RecibosVentas()
        {
            var userContent = _readOnlyRepository.Query<RecibosVentas>(x => x.UserCompro == User.Identity.Name).ToList();
            return View(userContent);
        }

    }
}
