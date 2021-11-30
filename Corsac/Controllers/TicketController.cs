using Corsac.Data;
using Corsac.DTOs;
using Corsac.Models;
using Corsac.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Corsac.Controllers
{    
    [Authorize]
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _context;
        public TicketController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var ticketWithStatusDto = await _context.Ticket.Include(x => x.TicketStatus).Include(x => x.User).Select(x => new TicketWithStatusDto 
            { 
                TicketId = x.Id,
                TicketUID = x.UID,
                TicketStatus = x.TicketStatus,
                Subject = x.Subject,
                Department = x.Department,
                Name = x.Name,
                Email = x.Email,
                UserName = x.User != null ? x.User.UserName : "",
            }).ToListAsync();
            return View(ticketWithStatusDto);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.Include(x => x.TicketStatus).Include(x => x.User).Where(x => x.Id == id).SingleOrDefaultAsync();
            if (ticket == null)
            {
                return NotFound();
            }

            var ticketMessages = await _context.TicketMessage.Where(x => x.TicketId == id).ToListAsync();

            TicketWithStatusAndMessagesDto ticketWithStatusAndMessagesDto = new TicketWithStatusAndMessagesDto
            {
                Status = ticket.TicketStatus.Status,
                TicketId = ticket.Id,
                TicketUID = ticket.UID,
                TicketMessages = ticketMessages,
                UserName = ticket.User != null ? ticket.User.UserName : "",
            };

            return View(ticketWithStatusAndMessagesDto);
        }

        [AllowAnonymous]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Details(string id, TicketWithStatusAndMessagesDto dto)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.Include(x => x.TicketStatus).Where(x => x.Id == id).SingleOrDefaultAsync();
            if (ticket == null)
            {
                return NotFound();
            }

            TicketMessage ticketMessage = new TicketMessage 
            {
                Id = Guid.NewGuid().ToString(),
                Message = dto.Message,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UpdatedBy = ticket.Email,
                TicketId = ticket.Id,
                UserType = String.IsNullOrEmpty(dto.UserId) ? "Guest" : "Admin",
            };

            _context.TicketMessage.Add(ticketMessage);

            TicketHistory history = new TicketHistory
            {
                Id = Guid.NewGuid().ToString(),
                UID = ticket.UID,
                Email = ticket.Email,
                Subject = ticket.Subject,
                Name = ticket.Name,
                Department = ticket.Department,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            TicketStatus status = new TicketStatus();
            ticket.UpdatedAt = DateTime.Now;
            
            if(!String.IsNullOrEmpty(dto.UserId))
            {
                status = await _context.TicketStatus.Where(x => x.Status == "Waiting for Customer").SingleOrDefaultAsync();
                history.UserId = dto.UserId;
                ticket.UserId = dto.UserId;                
            }
            else
            {
                status = await _context.TicketStatus.Where(x => x.Status == "Waiting for Staff Response").SingleOrDefaultAsync();
            }

            ticket.TicketStatusId = status.Id;
            history.TicketStatusId = status.Id;

            _context.TicketHistory.Add(history);
            _context.Ticket.Update(ticket);

            int result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                string url = String.Format(@"https://{0}/Ticket/Details/{1}", Request.Host, ticket.Id);
                string body = String.Format(@"Your submitted ticket UId is {0}.Please click here to view your ticket: {1}", ticket.UID, url);
                EmailService.SendEmail("no-reply@corsac.com", ticket.Email, ticket.Subject, body);
                return RedirectToAction("Details", new { id = id });
            }

            return View(dto);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.Where(x => x.Id == id).Select(x => new TicketEditDto { TicketId = x.Id, StatusId = x.TicketStatusId, UserId = x.UserId, TicketUID = x.UID}).SingleOrDefaultAsync();

            if (ticket == null)
            {
                return NotFound();
            }

            var users = await _context.Users.ToListAsync();
            var statuses = await _context.TicketStatus.ToListAsync();

            ViewData["user"] = new SelectList(users, "Id", "UserName", ticket.UserId);
            ViewData["status"] = new SelectList(statuses, "Id", "Status", ticket.StatusId);

            return View(ticket);
        }
        
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(string id, TicketEditDto dto)
        {
            if(String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.Where(x => x.Id == id).SingleOrDefaultAsync();

            if(ticket == null)
            {
                return NotFound();
            }

            ticket.TicketStatusId = dto.StatusId;
            ticket.UserId = dto.UserId;
            ticket.UpdatedAt = DateTime.Now;

            TicketHistory history = new TicketHistory
            {
                Id = Guid.NewGuid().ToString(),
                UID = ticket.UID,
                Email = ticket.Email,
                Subject = ticket.Subject,
                Name = ticket.Name,
                Department = ticket.Department,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                TicketStatusId = dto.StatusId,
                UserId = dto.UserId,
            };

            _context.TicketHistory.Add(history);

            _context.Ticket.Update(ticket);

            int result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                return RedirectToAction("Details", new { id = ticket.Id });
            }

            return View(dto);
        }
    }
}
