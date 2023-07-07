using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CurriculumSharingPortal.Persistence
{
	[Index(nameof(Name), IsUnique = true)]
    public class Subject
    {
        public Subject()
        {
            Curriculums = new HashSet<Curriculum>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Curriculum> Curriculums { get; set; }
    }
}
