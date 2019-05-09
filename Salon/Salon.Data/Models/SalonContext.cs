using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Salon.Data.Models
{
    public class SalonContext : DbContext
    {
        public SalonContext(DbContextOptions<SalonContext> options)
            :base(options)
        {
        }

        public DbSet<Salon_SPA> Salons { get; set; }
    }
}
