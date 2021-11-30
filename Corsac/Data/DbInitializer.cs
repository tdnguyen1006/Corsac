using Corsac.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Corsac.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new ApplicationUser
                {
                    UserName = "john",
                    Email = "john@corsac.com"
                };

                var user2 = new ApplicationUser
                {
                    UserName = "jane",
                    Email = "jane@corsac.com"
                };

                await userManager.CreateAsync(user, "Corsac123!@#");
                await userManager.CreateAsync(user2, "Corsac123!@#");
            }

            if(!context.TicketStatus.Any())
            {
                List<TicketStatus> statuses = new List<TicketStatus>
                {
                    new TicketStatus { Id = Guid.NewGuid().ToString(), Status = "Waiting for Staff Response"},
                    new TicketStatus { Id = Guid.NewGuid().ToString(), Status = "Waiting for Customer"},
                    new TicketStatus { Id = Guid.NewGuid().ToString(), Status = "On Hold"},
                    new TicketStatus { Id = Guid.NewGuid().ToString(), Status = "Cancelled"},
                    new TicketStatus { Id = Guid.NewGuid().ToString(), Status = "Completed"}
                };

                await context.AddRangeAsync(statuses);
            }

            await context.SaveChangesAsync();
        }
    }
}
