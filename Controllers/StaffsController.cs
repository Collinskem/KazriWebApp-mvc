using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using ZamaraWebApp.Data;
using ZamaraWebApp.Models;

namespace ZamaraWebApp.Controllers
{
    [Authorize]
    public class StaffsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StaffsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Staffs
        
        public async Task<IActionResult> Index()
        {
              return View(await _context.Staffs.ToListAsync());
        }

        // GET: Staffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Staffs == null)
            {
                return NotFound();
            }

            var staffViewModel = await _context.Staffs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staffViewModel == null)
            {
                return NotFound();
            }

            return View(staffViewModel);
        }

        // GET: Staffs/Create

        public IActionResult Create()
        {
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StaffNumber,Name,Email,Department,Salary")] StaffViewModel staffViewModel)
        {
            if (ModelState.IsValid)
            {
                string fileName = null;
                if (staffViewModel.Photo != null)
                {

                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    fileName = Guid.NewGuid().ToString() + "_" + staffViewModel.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    staffViewModel.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                _context.Add(staffViewModel);
                    await _context.SaveChangesAsync();
                    await sendMail("Profile Notification #Created", $"Greetings {staffViewModel.Name}, We are Glad to Inform you that your staff Profile has been Created",$"{staffViewModel.Email}");

                    return RedirectToAction(nameof(Index));     
            }
            return View(staffViewModel);
        }

        // GET: Staffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Staffs == null)
            {
                return NotFound();
            }

            var staffViewModel = await _context.Staffs.FindAsync(id);
            if (staffViewModel == null)
            {
                return NotFound();
            }
            return View(staffViewModel);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StaffNumber,Name,Email,Department,Salary")] StaffViewModel staffViewModel)
        {
            if (id != staffViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staffViewModel);
                    await _context.SaveChangesAsync();
					await sendMail("Profile Notification #UPDATED", $"Greetings {staffViewModel.Name}, We are Glad to Inform you that your staff Profile has been UPDATED", $"{staffViewModel.Email}");
				}
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffViewModelExists(staffViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(staffViewModel);
        }

        // GET: Staffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Staffs == null)
            {
                return NotFound();
            }

            var staffViewModel = await _context.Staffs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staffViewModel == null)
            {
                return NotFound();
            }

            return View(staffViewModel);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Staffs == null)
            {
                return Problem("Entity set 'AppDbContext.Staffs'  is null.");
            }
            var staffViewModel = await _context.Staffs.FindAsync(id);
            if (staffViewModel != null)
            {
                _context.Staffs.Remove(staffViewModel);
            }
            
            await _context.SaveChangesAsync();
			await sendMail("Profile Notification #DELETED", $"Greetings {staffViewModel.Name}, We Regret to Inform you that your staff Profile has been DELETED", $"{staffViewModel.Email}");
			return RedirectToAction(nameof(Index));
        }

        private bool StaffViewModelExists(int id)
        {
          return _context.Staffs.Any(e => e.Id == id);
        }

        //photo upload method

        public IActionResult Upload()
        {
            return View(new StaffPhotoViewModel());
        }

        [HttpPost]
        public IActionResult Upload(StaffPhotoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Photo.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(stream);
                }

                return RedirectToAction("ViewPhoto", new { fileName });
            }

            return View(model);
        }

        public IActionResult ViewPhoto(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "image/jpeg");
        }
         
        public static async Task sendMail(string subject, string emailMessage, string emailer)
		{
            try
            {
				string fromMail = "yegojs200@gmail.com";
				string fromPassword = "zrmevgfolydfgmss";
				MailMessage message = new MailMessage();
				message.From = new MailAddress(fromMail, "Staff Alert");
				message.Subject = subject;
                message.To.Add(new MailAddress(emailer));
				message.Body = emailMessage;
				message.IsBodyHtml = true;

				var smtpClient = new SmtpClient("smtp.gmail.com")
				{
					Port = 587,
					Credentials = new NetworkCredential(fromMail, fromPassword),
					EnableSsl = true,

				};
				await smtpClient.SendMailAsync(message);
				//return Task.CompletedTask;

			}
            catch (Exception exc)
            {

                Console.WriteLine("Sending Mail Failed");
            }
			
		}
	}
}
