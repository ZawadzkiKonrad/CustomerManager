﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MasterDev.Domain.Model
{
    public class Klient
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Surname { get; set; }
        public int BirthYear { get; set; }
    }
}
