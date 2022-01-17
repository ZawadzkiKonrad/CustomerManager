using MasterDev.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterDev.Domain.Interfaces
{
   public interface IKlientRepository
    {
        void DeleteKlient(int id);
        void DeleteKlients(IEnumerable<int> id);

        int AddKlient(Klient klient);

        Klient GetKlientById(int id);

        List<Klient> GetSelectKlients(List<Klient> emp);

        IQueryable<Klient> GetAllKlients();

        
        void UpdateKlient(Klient klient);
    }
}
