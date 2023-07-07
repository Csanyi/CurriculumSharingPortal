using CurriculumSharingPortal.Persistence;
using CurriculumSharingPortal.Persistence.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CurriculumSharingPortal.Web.Models;

namespace CurriculumSharingPortal.Web.Controllers
{
	public enum SortOrder { TIME_DESC, TIME_ASC, RATING_DESC, RATING_ASC, TITLE_DESC, TITLE_ASC }

    public class HomeController : Controller
    {
        private readonly ICurriculumSharingPortalService _service;
        private const int pageSize = 20;

        public HomeController(ICurriculumSharingPortalService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var c = _service.GetCurriculums().OrderByDescending(c => c.TimeStamp);

            return View(c);
        }

        public IActionResult Browse(
            string currentFilter, 
            string? searchString,
            int? pageNumber,
            int? subjectFilter = -1,
            SortOrder sortOrder = SortOrder.TITLE_ASC)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TimeSortParam"] = sortOrder == SortOrder.TIME_DESC ? SortOrder.TIME_ASC : SortOrder.TIME_DESC;
            ViewData["TitleSortParam"] = sortOrder == SortOrder.TITLE_DESC ? SortOrder.TITLE_ASC : SortOrder.TITLE_DESC;
            ViewData["RatingSortParam"] = sortOrder == SortOrder.RATING_DESC ? SortOrder.RATING_ASC : SortOrder.RATING_DESC;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurretnSubject"] = subjectFilter;

            List<Curriculum> c = _service.GetCurriculumByName(searchString);

            if(subjectFilter != null && subjectFilter != -1)
            {
                c = c.Where(c => c.SubjectId == subjectFilter).ToList();
            }

            switch (sortOrder)
            {
                case SortOrder.TIME_ASC:
                    c = c.OrderBy(c => c.TimeStamp).ToList();
                    break;
                case SortOrder.TIME_DESC:
                    c = c.OrderByDescending(c => c.TimeStamp).ToList();
                    break;
                case SortOrder.TITLE_ASC:
                    c = c.OrderBy(c => c.Title).ToList();
                    break;
                case SortOrder.TITLE_DESC:
                    c = c.OrderByDescending(c => c.Title).ToList();
                    break;
                case SortOrder.RATING_ASC:
                    c = c.OrderBy(c => c.Rating).ToList();
                    break;
                case SortOrder.RATING_DESC:
                    c = c.OrderByDescending(c => c.Rating).ToList();
                    break;
                default:
                    break;
            }

            var subjects = _service.GetSubjects();
            subjects.Add(new Subject
            {
                Id = -1,
                Name = "All"
            });

            ViewBag.Subjects = new SelectList(subjects, "Id", "Name", subjectFilter);
            return View(PaginatedList<Curriculum>.Create(c, pageNumber ?? 1, pageSize));
        }
    }
}