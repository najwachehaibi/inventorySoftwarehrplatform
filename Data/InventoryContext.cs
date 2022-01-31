
using InventoryBeginners.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryBeginners.Data
{
    public class InventoryContext: IdentityDbContext
    {
        public InventoryContext(DbContextOptions options):base(options)
        {

        }

        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Conge>Conges { get; set; }

        public virtual DbSet<Candidat>Candidats { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }

        public DbSet<InventoryBeginners.Models.ProjectRole> ProjectRole { get; set; }









    }
}
