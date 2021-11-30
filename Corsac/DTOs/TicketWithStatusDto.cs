using Corsac.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Corsac.DTOs
{
    public class TicketWithStatusDto
    {
        public string TicketId { get; set; }
        public string UserName { get; set; }
        public string TicketUID { get; set; }
        [Required]
        public string Subject { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public string Message { get; set; }
        public string StatusId { get; set; }
        public TicketStatus TicketStatus { get; set; }

        public List<TicketStatus> Statuses { get; set; }
    }
}
