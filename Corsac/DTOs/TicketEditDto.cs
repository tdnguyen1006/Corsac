using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Corsac.DTOs
{
    public class TicketEditDto
    {
        public string TicketId { get; set; }
        public string TicketUID { get; set; }
        public string UserId { get; set; }
        public string StatusId { get; set; }
    }
}
