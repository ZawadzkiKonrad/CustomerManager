using MasterDev.Application.ViewModels.Klient;
using MasterDev.Domain.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterDev.Application.Interfaces
{
   public  interface IKlientService
    {
        IQueryable<KlientForListVm> GetAllKlients();
        int AddKlient(NewKlientVm klient);
        NewKlientVm GetKlientForEdit(int id);
        void UpdateKlient(NewKlientVm model);
        void DeleteKlient(int id);
        void DeleteKlients(IEnumerable<int> id);
        void Import(IFormFile file);
        void ImportExcel(IFormFile file);
        


    }
}
