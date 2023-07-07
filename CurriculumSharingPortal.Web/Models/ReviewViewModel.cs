using System.ComponentModel.DataAnnotations;
using CurriculumSharingPortal.Persistence;

namespace CurriculumSharingPortal.Web.Models
{
	public class ReviewViewModel
    {
        [Key]
        public int Id { get; set; }
        public int CurriculumId { get; set; }
        public string UserId { get; set; } = null!;
        [Range(MinValue, MaxValue)]
        public int Value { get; set; }

        public const int MinValue = 1;
        public const int MaxValue = 5;

        public static explicit operator Review(ReviewViewModel vm) => new Review
        {
            Id = vm.Id,
            CurriculumId = vm.CurriculumId,
            UserId = vm.UserId,
            Value = vm.Value,
        };

        public static explicit operator ReviewViewModel(Review r) => new ReviewViewModel
        {
            Id = r.Id,
            CurriculumId = r.CurriculumId,
            UserId = r.UserId,
            Value = r.Value,
        };
    }
}
