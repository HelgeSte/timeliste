using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using timeliste.Models;

namespace timeliste.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Time> Time { get; set; }
        public DbSet<Prosjekt> Prosjekt { get; set; }
    }
}
