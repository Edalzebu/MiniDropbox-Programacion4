using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;

namespace MiniDropbox.Web.Controllers
{
    public class BitacoraController :BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;

        public BitacoraController(IWriteOnlyRepository writeOnlyRepository, IReadOnlyRepository readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }

        bool Bitacora(string activida)
        {
            var usuario = _readOnlyRepository.First<Account>(x => x.EMail == User.Identity.Name); 
            var pasos = new Actividades();
            pasos.Actividad=activida;
            pasos.hora = DateTime.Now;
            usuario.History.Add(pasos);
            usuario = _writeOnlyRepository.Update(usuario);
            return true;
        }

        [HttpGet]
        public ActionResult Actividades()
        {
            var cuenta = _readOnlyRepository.First<Account>(x => x.EMail == User.Identity.Name);
            if (cuenta.History.Count != 0)
            {

                return View(cuenta.History);
            }
            var actividad = new Actividades();
            actividad.Actividad = "";

            return View();
        }

        [HttpGet]
        public ActionResult Volver()
        {
            return RedirectToAction("ListAllContent", "Disk");
        }

    }
}
