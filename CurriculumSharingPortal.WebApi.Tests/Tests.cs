using AutoMapper;
using CurriculumSharingPortal.Api.Controllers;
using CurriculumSharingPortal.DTO;
using CurriculumSharingPortal.Persistence;
using CurriculumSharingPortal.Persistence.Services;
using CurriculumSharingPortal.WebApi.MappingConfigurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CurriculumSharingPortal.WebApi.Tests
{
	public class Tests : IDisposable
    {
        private readonly CurriculumSharingPortalDbContext _context;
        private readonly CurriculumSharingPortalService _service;
		private readonly Mapper _mapper;
        private readonly SubjectsController _subjectsController;
		private readonly CurriculumsController _curriculumsController;

		public Tests()
        {
            var options = new DbContextOptionsBuilder<CurriculumSharingPortalDbContext>().UseInMemoryDatabase("TestDb").Options;

            _context = new CurriculumSharingPortalDbContext(options);

            TestDbInitializer.Initialize(_context);

			_context.ChangeTracker.Clear();
			_service = new CurriculumSharingPortalService(_context);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new SubjectProfile());
                cfg.AddProfile(new CurriculumProfile());
				cfg.AddProfile(new SubjectDtoProfile());
				cfg.AddProfile(new CurriculumDtoProfile());
			});
            _mapper = new Mapper(config);

			_subjectsController = new SubjectsController(_service, _mapper);
			_curriculumsController = new CurriculumsController(_service, _mapper);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

		[Fact]
		public void GetSubjectsTest()
		{
			var result = _subjectsController.GetSubjects();

			var content = Assert.IsAssignableFrom<IEnumerable<SubjectDto>>(result.Value);
			Assert.Equal(2, content.Count());
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		public void GetSubjectByIdTest(int id)
		{
			var result = _subjectsController.GetSubject(id);

			var content = Assert.IsAssignableFrom<SubjectDto>(result.Value);
			Assert.Equal(id, content.Id);
		}

		[Fact]
		public void GetInvalidSubjectTest()
		{
			var id = 3;

			var result = _subjectsController.GetSubject(id);

			Assert.IsAssignableFrom<NotFoundResult>(result.Result);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		public void PutSubjectTest(int id)
		{
			string newName = "Updated subject";
			var subjectDto = new SubjectDto { Id = id, Name = newName };

			var result = _subjectsController.PutSubject(id, subjectDto);

			var requestResult = Assert.IsAssignableFrom<OkResult>(result);
			var updatedList = _subjectsController.GetSubject(id);
			Assert.Equal(updatedList?.Value?.Name, newName);
		}

		[Fact]
		public void PostSubjectTest()
		{
			var newSubject = new SubjectDto { Name = "New test subject" };
			var count = _context.Subjects.Count();

			var result = _subjectsController.PostSubject(newSubject);

			var objectResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result.Result);
			var content = Assert.IsAssignableFrom<SubjectDto>(objectResult.Value);
			Assert.Equal(count + 1, _context.Subjects.Count());
		}

		[Fact]
		public void DeleteSubjectTest()
		{
			var count = _context.Subjects.Count();

			var result = _subjectsController.DeleteSubject(1);

			var objectResult = Assert.IsAssignableFrom<OkResult>(result);
			Assert.Equal(count - 1, _context.Subjects.Count());
		}



		[Fact]
		public void GetCurriculumsTest()
		{
			var result = _curriculumsController.GetCurriculums(1);

			var content = Assert.IsAssignableFrom<IEnumerable<CurriculumDto>>(result.Value);
			Assert.Equal(2, content.Count());
		}

		[Fact]
		public void GetCurriculumsByNameTest()
		{
			var result = _curriculumsController.GetCurriculumsByName("1");

			var content = Assert.IsAssignableFrom<IEnumerable<CurriculumDto>>(result.Value);
			Assert.Equal(2, content.Count());
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		public void PutCurriculumTest(int id)
		{
			string newTitle = "Updated curriculum";
			var curriculumDto = new CurriculumDto { Id = id, Title = newTitle, DownloadCount = id };

			var result = _curriculumsController.PutCurriculum(id, curriculumDto);

			var requestResult = Assert.IsAssignableFrom<OkResult>(result);
			var updatedList = _curriculumsController.GetCurriculumsByName(newTitle);

			var c = updatedList?.Value?.ToList()[0];

			Assert.Equal(c?.Title, newTitle);
			Assert.Equal(c?.DownloadCount, id);
		}


		[Fact]
		public void DeleteCurriculumTest()
		{
			var count = _context.Curriculums.Count();

			var result = _curriculumsController.DeleteCurriculum(1);

			var objectResult = Assert.IsAssignableFrom<OkResult>(result);
			Assert.Equal(count - 1, _context.Curriculums.Count());
		}

		[Fact]
		public void DeleteReviewsTest()
		{
			var result = _curriculumsController.DeleteReviews(1);

			var objectResult = Assert.IsAssignableFrom<OkResult>(result);

			var list = _curriculumsController.GetCurriculumsByName("Math1");

			var count = list?.Value?.ToList()[0].Reviews.Count;
			Assert.Equal(0, count);
		}
	}
}