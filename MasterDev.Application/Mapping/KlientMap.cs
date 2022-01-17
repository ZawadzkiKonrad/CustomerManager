using CsvHelper.Configuration;
using MasterDev.Domain.Model;
using CsvHelper;
using System;
using System.Collections.Generic;

using System.Text;

namespace MasterDev.Application.Mapping
{
   public sealed class KlientMap:ClassMap<Klient>
    {
        public KlientMap()
        {
            Map(m => m.Name).Index(1).Name("Name");
            Map(m => m.Surname).Index(2).Name("Surname");
            Map(m => m.BirthYear).Index(3).Name("BirthYear");
        }

    }
}
