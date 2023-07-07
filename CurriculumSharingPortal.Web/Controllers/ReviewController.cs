using CurriculumSharingPortal.Persistence;
using CurriculumSharingPortal.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CurriculumSharingPortal.Web.Models;

namespace CurriculumSharingPortal.Web.Controllers
{
	[Authorize(Roles = "User")]
    public class ReviewController : Controller
    {
        private readonly ICurriculumSharingPortalService _service;

        public ReviewController(ICurriculumSharingPortalService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Create(int? curriculumId)
        {
            if (curriculumId == null)
            {
                return NotFound();
            }

            string? name = User.Identity?.Name;

            if (name == null)
            {
                return NotFound();
            }

            User? user = _service.GetUserByName(name);

            if (user == null)
            {
                return NotFound();
            }

            Curriculum? curriculum = _service.GetCurriculumById((int)curriculumId);

            if (curriculum == null)
            {
                return NotFound();
            }

            int? reviewId = _service.GetReviewId(user, curriculum);
            ViewBag.CurriculumTitle = curriculum.Title;

            if (reviewId == null)
            {
                ViewBag.UserId = user.Id;
                return View();
            }
            else
            {
                //return RedirectToAction(nameof(Edit), new {id = reviewId});
                return View("AlreadyReviewed");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int curriculumId, ReviewViewModel vm)
        {
            if (_service.GetCurriculumById(curriculumId) == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var review = (Review)vm;
                var result = _service.CreateReview(review);
                if (result is not null)
                {
                    return RedirectToAction("Browse", "Home");
                }
                else
                {
                    return NotFound();
                }
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Review? review = _service.GetReviewById((int)id);
            if(review == null)
            {
                return NotFound();
            }

            return View((ReviewViewModel)review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ReviewViewModel vm)
        {
            if(id != vm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Review review = (Review)vm;
                bool result = _service.UpdateReview(review);

                if(result)
                {
                    return RedirectToAction("Browse", "Home");
                }
                else
                {
                    return NotFound();
                }
            }

            return View(vm);
        }
    }
}
