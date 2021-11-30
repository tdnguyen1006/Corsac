using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Corsac.Models
{
    public class TicketMessage
    {
        public string Id { get; set; }
        public Ticket Ticket { get; set; }
        public string TicketId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string UserType { get; set; }
    }
}
