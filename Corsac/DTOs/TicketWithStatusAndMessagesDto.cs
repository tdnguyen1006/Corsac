using Corsac.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Corsac.DTOs
{
    public class TicketWithStatusAndMessagesDto
    {
        public string TicketId { get; set; }
        public string TicketUID { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime UpdateAt { get; set; }
        public IEnumerable<TicketMessage> TicketMessages { get; set; }
    }
}
