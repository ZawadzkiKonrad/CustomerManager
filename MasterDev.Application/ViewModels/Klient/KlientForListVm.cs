using AutoMapper;
using MasterDev.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasterDev.Application.ViewModels.Klient
{
    public class KlientForListVm:IMapFrom<Domain.Model.Klient>    
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int BirthYear { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Model.Klient, KlientForListVm>();

        }
    }
}
