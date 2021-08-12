﻿using MasterDev.Domain.Interfaces;
using MasterDev.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterDev.Infrastructure.Repositories
{
    
    public class KlientRepository : IKlientRepository
    {   
        private readonly Context _context;
        public KlientRepository(Context context)
        {
            _context = context;
        }
        public int AddKlient(Klient klient)
        {
           
                _context.Klients.Add(klient);
                _context.SaveChanges();
            
            return klient.Id;
        }

        public void DeleteKlient(int id)
        {
            var klient = _context.Klients.FirstOrDefault(p => p.Id == id);

            _context.Klients.Remove(klient);
               _context.SaveChanges();
            
            
        }

        public IQueryable<Klient> GetAllKlients()
        {
            var klients = _context.Klients;
            return klients;
        }

        public Klient GetKlientById(int id)
        {
            var klient = _context.Klients.FirstOrDefault(p => p.Id == id);
            return klient;
        }

        public void UpdateKlient(Klient klient)
        {
            _context.Attach(klient);
            _context.Entry(klient).Property("Name").IsModified = true;
            _context.Entry(klient).Property("Surname").IsModified = true;
            _context.Entry(klient).Property("BirthYear").IsModified = true;
            _context.SaveChanges();
        }
    }
}
