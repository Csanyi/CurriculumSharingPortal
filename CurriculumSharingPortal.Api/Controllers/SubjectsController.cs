using AutoMapper;
using CurriculumSharingPortal.DTO;
using CurriculumSharingPortal.Persistence;
using CurriculumSharingPortal.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurriculumSharingPortal.Api.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SubjectsController : ControllerBase
    {
        private readonly ICurriculumSharingPortalService _service;
        private readonly IMapper _mapper;

        public SubjectsController(ICurriculumSharingPortalService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<SubjectDto> GetSubject(int id)
        {
            try
            {
                return _mapper.Map<SubjectDto>(_service.GetSubjectById(id));
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<SubjectDto>> GetSubjects()
        {
            return _service.GetSubjects().Select(subject => _mapper.Map<SubjectDto>(subject)).ToList();
        }

        [HttpPut("{id}")]
        public IActionResult PutSubject(int id, SubjectDto subject)
        {
            if(id != subject.Id)
            {
                return BadRequest();
            }

            if (_service.UpdateSubject(_mapper.Map<Subject>(subject)))
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public ActionResult<SubjectDto> PostSubject(SubjectDto subjectDto)
        {
            var subject = _service.CreateSubject(_mapper.Map<Subject>(subjectDto));

            if(subject is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                return CreatedAtAction("GetSubject", new {id = subject.Id}, _mapper.Map<SubjectDto>(subject));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSubject(int id)
        {
            if (_service.DeleteSubject(id))
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
