namespace CurriculumSharingPortal.DTO
{
	public class CurriculumDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public SubjectDto Subject { get; set; } = null!;
        public byte[]? File { get; set; } = null!;
        public string FileFormat { get; set; } = null!;
        public DateTime TimeStamp { get; set; }
        public int DownloadCount { get; set; }
        public UserDto User { get; set; } = null!;
        public ICollection<ReviewDto> Reviews { get; set; } = null!;
    }
}
