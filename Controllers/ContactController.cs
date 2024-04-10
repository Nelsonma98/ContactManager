using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactManager.Context;
using ContactManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using ContactManager.helpers;

namespace ContactManager.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/contacts")]
    public class ContactController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShowContact>>> GetContacts()
        {
            var contacts = await _context.Contacts.ToListAsync();
            var showContacts = new List<ShowContact>();
            foreach (var contact in contacts)
            {
                showContacts.Add(new ShowContact{
                    id = contact.id,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    DateOfBirth = contact.DateOfBirth,
                    Phone = contact.Phone,
                    Owner = contact.Owner,
                    Age = ContactAge.CalculateAge(contact.DateOfBirth)
                });
            }

            return showContacts;
        }

        // GET: api/contacts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ShowContact>> GetContact(Guid id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            return new ShowContact{
                id = contact.id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                DateOfBirth = contact.DateOfBirth,
                Phone = contact.Phone,
                Owner = contact.Owner,
                Age = ContactAge.CalculateAge(contact.DateOfBirth)
            };
        }

        //PUT: api/contacts/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutContact(Guid id, Contact contact)
        {
            if (id != contact.id)
            {
                return BadRequest();
            }

            _context.Entry(contact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //POST: api/contacts
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(Contact contact)
        {
            var age = ContactAge.CalculateAge(contact.DateOfBirth);
            if(age < 18){
                return BadRequest("The contact must be over 18 years old.");
            }

            var contactDB = _context.Contacts.FirstOrDefault(x => x.Email == contact.Email);
            if (contactDB != null){
                return BadRequest("The email is already in use.");
            }
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContact", new { id = contact.id }, contact);
        }

        //DELETE: api/contacts/{id}
        [Authorize(Policy = "RequireAdminCu")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContactExists(Guid id)
        {
            return _context.Contacts.Any(e => e.id == id);
        }
    }
}