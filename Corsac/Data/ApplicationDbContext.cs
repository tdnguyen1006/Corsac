using Corsac.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Corsac.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<TicketStatus> TicketStatus { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<TicketHistory> TicketHistory { get; set; }
        public DbSet<TicketMessage> TicketMessage { get; set; }
    }
}
