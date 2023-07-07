using CurriculumSharingPortal.Persistence.Services;
using Microsoft.AspNetCore.Mvc;
using CurriculumSharingPortal.Persistence;
using AutoMapper;
using CurriculumSharingPortal.DTO;
using Microsoft.AspNetCore.Authorization;

namespace CurriculumSharingPortal.Api.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CurriculumsController : ControllerBase
    {
        private readonly ICurriculumSharingPortalService _service;
        private readonly IMapper _mapper;

        public CurriculumsController(ICurriculumSharingPortalService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("{subjectId}")]
        public ActionResult<IEnumerable<CurriculumDto>> GetCurriculums(int subjectId) 
        {
            return _service.GetCurriCulumBySubject(subjectId).Select(curriculum => _mapper.Map<CurriculumDto>(curriculum)).ToList();
        }

        [AllowAnonymous]
        [HttpGet("Name/{name}")]
        public ActionResult<IEnumerable<CurriculumDto>> GetCurriculumsByName(string name)
        {
            return _service.GetCurriculumByName(name).Select(curriculum => _mapper.Map<CurriculumDto>(curriculum)).ToList();
        }

        [HttpPut("{id}")]
        public IActionResult PutCurriculum(int id, CurriculumDto curriculum)
        {
            if (id != curriculum.Id)
            {
                return BadRequest();
            }

            var c = _mapper.Map<Curriculum>(curriculum);

            c.Reviews.Clear();
            c.User = null!;
            c.Subject = null!;

			if (_service.UpdateCurriculum(c))
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCurriculum(int id)
        {
            if (_service.DeleteCurriculum(id))
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("Reviews/{id}")]
        public IActionResult DeleteReviews(int id)
        {
            if (_service.DeleteReviews(id))
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
