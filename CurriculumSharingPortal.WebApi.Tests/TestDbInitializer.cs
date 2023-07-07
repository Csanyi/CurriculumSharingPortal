using CurriculumSharingPortal.Persistence;

namespace CurriculumSharingPortal.WebApi.Tests
{
	public static class TestDbInitializer
    {
        public static void Initialize(CurriculumSharingPortalDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Subjects.Any())
            {
                return;
            }

            IList<Subject> subjects = new List<Subject>
            {
                new Subject
                {
                    Id = 1,
                    Name = "Math"
                },
                new Subject
                {
                    Id = 2,
                    Name = "History"
                },
            };

            context.Subjects.AddRange(subjects);

            IList<Curriculum> curriculums = new List<Curriculum>
            {
                new Curriculum
                {
                    Id = 1,
                    Title = "Math1",
                    SubjectId = 1,
                    FileFormat = "png",
                    TimeStamp = DateTime.Now,
                    DownloadCount = 10,
                    UserId = "3",
                },
                new Curriculum
                {
                    Id = 2,
                    Title = "Math2",
                    SubjectId = 1,
                    FileFormat = "jpg",
                    TimeStamp = DateTime.Now,
                    DownloadCount = 20,
                    UserId = "2",
                }, 
                new Curriculum
                {
                    Id = 3,
                    Title = "History1",
                    SubjectId = 2,
                    FileFormat = "pdf",
                    TimeStamp = DateTime.Now,
                    DownloadCount = 30,
                    UserId = "1",
                },
            };

            context.Curriculums.AddRange(curriculums);


            IList<Review> reviews = new List<Review>
            {
                new Review
                {
                    Id = 1,
                    CurriculumId = 1,
                    UserId = "1",
                    Value = 5,
                },
                new Review
                {
                    Id = 2,
                    CurriculumId = 1,
                    UserId = "1",
                    Value = 2,
                },
                new Review
                {
                    Id = 3,
                    CurriculumId = 2,
                    UserId = "1",
                    Value = 3,
                },
            };

            context.Reviews.AddRange(reviews);

            context.SaveChanges();
        }
    }
}