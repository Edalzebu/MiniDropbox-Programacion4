using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BootstrapMvcSample.Controllers;
using FluentNHibernate.Conventions;
using MiniDropbox.Data.AutoMappingOverride;
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
            var modelo = new List<ActividadesModel>();
            if (cuenta.History.Count != 0)
            {
                foreach (var Actividad in cuenta.History)
                {
                    modelo.Add(Mapper.Map<Actividades, ActividadesModel>(Actividad)); 
                }
                
                return View(modelo);
            }
            var actividad = new ActividadesModel();
            actividad.Actividad = "";
            modelo.Add(actividad);
            return View(modelo);
        }

        [HttpGet]
        public ActionResult Volver()
        {
            return RedirectToAction("ListAllContent", "Disk");
        }

        public ActionResult CleanActivity()
        {
            var account = _readOnlyRepository.First<Account>(x => x.EMail == User.Identity.Name);
            if (account != null)
            {
                account.History.Clear();
                _writeOnlyRepository.Update(account);
            }
            return RedirectToAction("Actividades");
        }

        public ActionResult SearchActivity( string searchTxt)
        {
            var account = _readOnlyRepository.First<Account>(x => x.EMail == User.Identity.Name);
            var lista = new List<ActividadesModel>();
            foreach (var story in account.History)
            {
                if (story.Actividad.Contains(searchTxt))
                {
                    lista.Add(Mapper.Map<Actividades,ActividadesModel>(story));
                }
            }
            if (lista.IsEmpty())
            {
                var model = new ActividadesModel();
                model.Actividad = "No se encontro nada con esa busqueda";
                model.Hora = DateTime.Now;
                lista.Add(model );
            }
            return View(lista);
        }
    }
}
