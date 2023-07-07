using Microsoft.AspNetCore.Identity;

namespace CurriculumSharingPortal.Persistence
{
	public class User : IdentityUser
    {
        public User()
        {
            Curriculums = new HashSet<Curriculum>();
            Reviews = new HashSet<Review>();
        }

        public string DisplayName { get; set; } = null!;

        public virtual ICollection<Curriculum> Curriculums { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
