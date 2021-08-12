using MasterDev.Domain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasterDev.Infrastructure
{
    public class Context:IdentityDbContext
    {
        public DbSet<Klient> Klients { get; set; }
        public Context(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.UseSerialColumns();
            builder.Entity<Klient>().Property(b => b.Id)
        .HasIdentityOptions(startValue: 100);

            //builder.Entity<Klient>()
            //       .Property(b => b.Id)
            //       .UseHiLo();
        //    builder.Entity<Klient>().Property(b => b.Id)
        //.HasIdentityOptions(startValue: 100);
           

            
            
            base.OnModelCreating(builder);
        }

    }
}
