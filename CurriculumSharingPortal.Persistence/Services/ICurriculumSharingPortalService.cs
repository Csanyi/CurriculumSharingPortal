namespace CurriculumSharingPortal.Persistence.Services
{
	public interface ICurriculumSharingPortalService
    {
        // GET
        List<Subject> GetSubjects();
        Subject GetSubjectById(int id);

        List<Curriculum> GetCurriculums();
        List<Curriculum> GetCurriCulumBySubject(int id);
        List<Curriculum> GetCurriculumByName(string? name);
        Curriculum? GetCurriculumById(int id);

        int? GetReviewId(User user, Curriculum curriculum);
        Review? GetReviewById(int id);

        public User? GetUserByName(string name);

        // CREATE
        Subject? CreateSubject(Subject subject);
        Curriculum? CreateCurriculum(Curriculum curriculum);
        Review? CreateReview(Review review);

        // UPDATE
        bool UpdateSubject(Subject subject);
        bool UpdateCurriculum(Curriculum curriculum);
        bool UpdateReview(Review review);

        // DELETE
        bool DeleteSubject(int id);
        bool DeleteCurriculum(int id);
        bool DeleteReviews(int id);


        bool Download(int id);
    }
}
