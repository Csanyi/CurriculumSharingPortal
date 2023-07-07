using System.ComponentModel.DataAnnotations;

namespace CurriculumSharingPortal.Persistence
{
	public class Review
    {
        [Key]
        public int Id { get; set; }
        public int CurriculumId { get; set; }
        public string UserId { get; set; } = null!;
        [Range(1, 5)]
        public int Value { get; set; }

        public virtual Curriculum Curriculum { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
