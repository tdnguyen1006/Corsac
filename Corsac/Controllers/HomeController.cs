using Corsac.Data;
using Corsac.DTOs;
using Corsac.Models;
using Corsac.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Corsac.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            TicketWithStatusDto ticketWithStatusDto = new TicketWithStatusDto();
            List<TicketStatus> statuses = await _context.TicketStatus.ToListAsync();
            ticketWithStatusDto.Statuses = statuses;
            return View(ticketWithStatusDto);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index(TicketWithStatusDto ticketDto)
        {
            if (ModelState.IsValid)
            {
                Ticket ticket = new Ticket
                {
                    Id = Guid.NewGuid().ToString(),
                    UID = RandomUIDService.GenerateUID(10),
                    Email = ticketDto.Email,
                    Subject = ticketDto.Subject,
                    Name = ticketDto.Name,
                    Department = ticketDto.Department,
                    TicketStatusId = ticketDto.StatusId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                TicketHistory history = new TicketHistory
                {
                    Id = Guid.NewGuid().ToString(),
                    UID = ticket.UID,
                    Email = ticketDto.Email,
                    Subject = ticketDto.Subject,
                    Name = ticketDto.Name,
                    Department = ticketDto.Department,
                    TicketStatusId = ticketDto.StatusId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                TicketMessage message = new TicketMessage
                {
                    Id = Guid.NewGuid().ToString(),
                    Message = ticketDto.Message,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = ticketDto.Email,
                    TicketId = ticket.Id,
                    UserType = "Guest"
                };

                _context.Ticket.Add(ticket);
                _context.TicketHistory.Add(history);
                _context.TicketMessage.Add(message);
                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    string url = String.Format(@"https://{0}/Ticket/Details/{1}", Request.Host, ticket.Id);
                    string body = String.Format(@"Your submitted ticket UId is {0}.Please click here to view your ticket: {1}", ticket.UID, url);
                    EmailService.SendEmail("no-reply@corsac.com", ticket.Email, ticket.Subject, body);
                }

                return RedirectToAction("Details", "Ticket", new { id = ticket.Id });
            }


            return View(ticketDto);
        }
    }
}
