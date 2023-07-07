using System.ComponentModel.DataAnnotations;

namespace CurriculumSharingPortal.Persistence
{
	public class Curriculum
    {
        public Curriculum()
        {
            Reviews = new HashSet<Review>();
        }

        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int SubjectId { get; set; }
        public byte[]? File { get; set; } = null!;
        public string FileFormat { get; set; } = null!;
        public DateTime TimeStamp { get; set; }
        public int DownloadCount { get; set; }
        public string UserId { get; set; } = null!;

        public virtual Subject Subject { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Review> Reviews { get; set; }

        public double Rating
        {
            get
            {
                if (Reviews.Any())
                {
                    return Math.Round(Reviews.Average(r => r.Value), 1);
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
