using Microsoft.EntityFrameworkCore;

namespace CurriculumSharingPortal.Persistence.Services
{
	public class CurriculumSharingPortalService : ICurriculumSharingPortalService
    {
        private readonly CurriculumSharingPortalDbContext _context;

        public CurriculumSharingPortalService(CurriculumSharingPortalDbContext context)
        {
            _context = context;
        }

        // GET

        public List<Subject> GetSubjects()
        {
            return _context.Subjects.ToList();
        }

        public Subject GetSubjectById(int id)
        {
            return _context.Subjects.Single(s => s.Id == id);
        }

        public List<Curriculum> GetCurriculums()
        {
            return _context.Curriculums.ToList();
        }

        public List<Curriculum> GetCurriCulumBySubject(int id)
        {
            return _context.Curriculums.Where(c => c.SubjectId == id).ToList();
        }

        public List<Curriculum> GetCurriculumByName(string? name)
        {
            return _context.Curriculums.Where(c => c.Title.Contains(name ?? "")).ToList();
        }

        public Curriculum? GetCurriculumById(int id)
        {
            return _context.Curriculums.Where(c => c.Id == id).FirstOrDefault();
        }

        public User? GetUserByName(string name)
        {
            return _context.Members.FirstOrDefault(u => u.UserName == name);
        }

        public Review? GetReviewById(int id)
        {
            return _context.Reviews.FirstOrDefault(r => r.Id == id);
        }

        public int? GetReviewId(User user, Curriculum curriculum)
        {
            return user.Reviews.FirstOrDefault(r => r.CurriculumId == curriculum.Id)?.Id;
        }

        // CREATE
        public Subject? CreateSubject(Subject subject)
        {
            try
            {
                _context.Add(subject);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return null;
            }

            return subject;
        }

        public Curriculum? CreateCurriculum(Curriculum curriculum)
        {
            try
            {
                _context.Add(curriculum);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return null;
            }

            return curriculum;
        }

        public Review? CreateReview(Review review)
        {
            try
            {
                _context.Add(review);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return null;
            }

            return review;
        }


        // UPDATE
        public bool UpdateSubject(Subject subject)
        {
            try
            {
                _context.Update(subject);
                _context.SaveChanges();
            }
            catch(DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public bool UpdateCurriculum(Curriculum curriculum)
        {
            try
            {
                _context.Update(curriculum);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public bool UpdateReview(Review review)
        {
            try
            {
                _context.Update(review);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        // DELETE
        public bool DeleteSubject(int id)
        {
            var subject = _context.Subjects.Find(id);
            if (subject == null)
            {
                return false;
            }

            try
            {
                _context.Remove(subject);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public bool DeleteCurriculum(int id)
        {
            var curriculum = _context.Curriculums.Find(id);
            if (curriculum == null)
            {
                return false;
            }

            try
            {
                _context.Remove(curriculum);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public bool DeleteReviews(int id)
        {
            var curriculum = _context.Curriculums.Find(id);
            if (curriculum == null)
            {
                return false;
            }

            try
            {
                foreach(Review r in curriculum.Reviews)
                {
                    _context.Remove(r);
                }
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }



        public bool Download(int id)
        {
            Curriculum? c = GetCurriculumById(id);

            if(c == null) 
            {
                return false;
            }

            try
            {
                ++c.DownloadCount;
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }
    }
}
