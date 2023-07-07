using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CurriculumSharingPortal.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using CurriculumSharingPortal.Persistence;
using CurriculumSharingPortal.Web.Models;

namespace CurriculumSharingPortal.Web.Controllers
{
	[Authorize(Roles = "User")]
    public class CurriculumController : Controller
    {
        private readonly ICurriculumSharingPortalService _service;

        public CurriculumController(ICurriculumSharingPortalService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult DownloadFile(int id)
        {
            var curriculum = _service.GetCurriculumById(id);
            if(curriculum != null && curriculum.File != null)
            {
                bool result = _service.Download(id);

                if (result)
                {
                    return File(curriculum.File, "text/plain", curriculum.Title + curriculum.FileFormat);
                }
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Subjects = new SelectList(_service.GetSubjects(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CurriculumViewModel vm, IFormFile? file)
        {
            if(file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "The File Field is required");
                ViewBag.Subjects = new SelectList(_service.GetSubjects(), "Id", "Name", vm.SubjectId);
                return View(vm);
            }

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                vm.File = stream.ToArray();
            }
            
            if (ModelState.IsValid)
            {
                var curriculum = (Curriculum)vm;
                curriculum.TimeStamp = DateTime.Now;
                curriculum.FileFormat = Path.GetExtension(file.FileName);
                string? name =  User.Identity?.Name;

                if(name == null)
                {
                    return NotFound();
                }

                User? user = _service.GetUserByName(name);

                if (user == null)
                {
                    return NotFound();
                }

                curriculum.UserId = user.Id;

                var result = _service.CreateCurriculum(curriculum);

                if(result is not null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return NotFound();
                }
            }

            ViewBag.Subjects = new SelectList(_service.GetSubjects(), "Id", "Name", vm.SubjectId);
            return View(vm);
        }
    }
}
