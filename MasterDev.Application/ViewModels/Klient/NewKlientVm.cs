using AutoMapper;
using MasterDev.Application.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MasterDev.Application.ViewModels.Klient
{
   public class NewKlientVm:IMapFrom<Domain.Model.Klient>
    {
        
        public int Id { get; set; }
        
        [Required(ErrorMessage ="Musisz Podać imię")]
        [Display(Name = "Imię")]
        [StringLength(50)]
        
        public string Name { get; set; }

        [Required(ErrorMessage ="Musisz podać nazwisko")]
        [Display(Name = "Nazwisko")]
        [StringLength(50)]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Musisz Podać rok")]
        [Display(Name = "Rok urodzenia")]
        //[Range(typeof(int), "1901", "2019")]
        public int BirthYear { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<NewKlientVm, Domain.Model.Klient >().ReverseMap();

        }
    }
}
