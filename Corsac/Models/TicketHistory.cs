using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Corsac.Models
{
    public class TicketHistory
    {
        public string Id { get; set; }
        public string UID { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string TicketStatusId { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
