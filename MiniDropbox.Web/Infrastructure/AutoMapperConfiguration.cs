using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using MiniDropbox.Domain;
using MiniDropbox.Web.Controllers;
using MiniDropbox.Web.Models;
using Ninject.Modules;

namespace MiniDropbox.Web.Infrastructure
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<AccountSignUpModel, Account>();
            Mapper.CreateMap<Account, AccountSignUpModel>();
            
            Mapper.CreateMap<Account, AccountProfileModel>();
            Mapper.CreateMap<AccountProfileModel, Account>();
            
            Mapper.CreateMap<Account, RegisteredUsersListModel>();
            Mapper.CreateMap<RegisteredUsersListModel, Account>();
            
            Mapper.CreateMap<Account, ChangeUserSpaceLimitModel>();
            Mapper.CreateMap<ChangeUserSpaceLimitModel, Account>();

            Mapper.CreateMap<File,DiskContentModel>();
            Mapper.CreateMap<DiskContentModel, File>();

            Mapper.CreateMap<Package, PackageModel>();
            Mapper.CreateMap<PackageModel, Package>();

            Mapper.CreateMap<Package,CreateEditPackageController>();
            Mapper.CreateMap<CreateEditPackageController, Package>();
            
            Mapper.CreateMap<File, FileSearchResult>();
            Mapper.CreateMap<FileSearchResult, File>();

            Mapper.CreateMap<Actividades, ActividadesModel>().ForMember(x=>x.Actividad,y=>y.MapFrom(o=>o.Actividad)).ForMember(x=>x.Usuario,y=>y.MapFrom(o=>o.User)).ForMember(x=>x.Hora,y=>y.MapFrom(o=>o.hora));
            Mapper.CreateMap<ActividadesModel, Actividades>();
        }
    }
}