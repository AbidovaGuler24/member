using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication7.DAL;
using WebApplication7.Models;
using WebApplication7.Utils.Extentions;

namespace WebApplication7.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MemberController : Controller
    {
      
            AppDbContext _context;
            private readonly IWebHostEnvironment _environment;

            public MemberController(AppDbContext context, IWebHostEnvironment environment)
            {
                _context = context;
                _environment = environment;
            }
            public async Task<IActionResult> Index()
            {
                var members = await _context.members.ToListAsync();
                return View(members);
            }
            public IActionResult Create()
            {

                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Create(Member member)
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                if (member.File.Length > 2097152)
                {
                    ModelState.AddModelError("File","max 2mb ola biler");
                    return View();
                }
                if (!member.File.ContentType.Contains("image"))
                {
                    ModelState.AddModelError("File", "duzgun fayl daxil et");
                    return View();

                }

            member.ImgUrl = FileCreateExtentions.CreaterFile(member.File, "C:\\Users\\I Novbe\\source\\repos\\WebApplication7\\WebApplication7\\wwwroot", "Upload/Member");
                await _context.members.AddAsync(member);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
        public async Task<IActionResult> UpdateAsync(int? id)
        {
            if(id== null)
            {
                return NotFound();
            }
            var member = await _context.members.FindAsync(id);

            if (member == null) return NotFound(); 

            return View(member);


        }
        [HttpPost]
        public async Task<IActionResult> Update( Member member)
        {
         


            if (member.File != null)
            {
                if (!ModelState.IsValid)
                {
                    return View();

                }

                if (member.File.Length > 2097152)
                {
                    ModelState.AddModelError("File", "max 2mb ola biler");
                    return View();
                }
                if (!member.File.ContentType.Contains("image"))
                {
                    ModelState.AddModelError("File", "duzgun fayl daxil et");
                    return View();

                }
            }
            member.ImgUrl = FileCreateExtentions.CreaterFile(member.File, "C:\\Users\\I Novbe\\source\\repos\\WebApplication7\\WebApplication7\\wwwroot", "Upload/Member");
            _context.members.Update(member);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? Id)
            {
                var category = await _context.members.FirstOrDefaultAsync(x => x.Id == Id);
                if (category == null) { return BadRequest(); }
                _context.members.Remove(category);

            
            await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

        }
    }

