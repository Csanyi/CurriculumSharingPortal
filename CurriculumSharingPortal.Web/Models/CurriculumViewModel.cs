using System.ComponentModel.DataAnnotations;
using CurriculumSharingPortal.Persistence;

namespace CurriculumSharingPortal.Web.Models
{
	public class CurriculumViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int SubjectId { get; set; }
        public byte[]? File { get; set; } = null!;

        public static explicit operator Curriculum(CurriculumViewModel vm) => new Curriculum
        {
            Id = vm.Id,
            Title = vm.Title,
            SubjectId = vm.SubjectId,
            File = vm.File,
        };

        public static explicit operator CurriculumViewModel(Curriculum c) => new CurriculumViewModel
        {
            Id = c.Id,
            Title = c.Title,
            SubjectId = c.SubjectId,
            File = c.File,
        };
    }
}
