using CurriculumSharingPortal.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurriculumSharingPortal.Desktop.ViewModel
{
	public class CurriculumViewModel : ViewModelBase
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        private string _title = null!;

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }

        private SubjectViewModel _subject = null!;

        public SubjectViewModel Subject
        {
            get { return _subject; }
            set { _subject = value; OnPropertyChanged(); }
        }

        private byte[]? _file;

        public byte[]? File 
        { 
            get { return _file; } 
            set { _file = value; OnPropertyChanged(); } 
        }

        private string _fileFormat = null!;

        public string FileFormat
        {
            get { return _fileFormat; }
            set { _fileFormat = value; OnPropertyChanged(); }
        }

        private DateTime _timeStamp;

        public DateTime TimeStamp 
        { 
            get { return _timeStamp; }
            set { _timeStamp = value; OnPropertyChanged(); }
        }

        private int _downloadCount;

        public int DownloadCount
        {
            get { return _downloadCount; }
            set { _downloadCount = value; OnPropertyChanged(); }
        }

        private UserViewModel _user = null!;

        public UserViewModel User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged(); }
        }

        public ICollection<ReviewViewModel> _reviews = null!;

        public ICollection<ReviewViewModel> Reviews 
        { 
            get { return _reviews; }
            set { _reviews = value; OnPropertyChanged(); OnPropertyChanged(nameof(Rating)); }
        }

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


        public static explicit operator CurriculumViewModel(CurriculumDto dto) => new CurriculumViewModel
        {
            Id = dto.Id,
            Title = dto.Title,
            Subject = (SubjectViewModel)dto.Subject,
            File = dto.File,
            FileFormat = dto.FileFormat,
            TimeStamp = dto.TimeStamp,
            DownloadCount = dto.DownloadCount,
            User = (UserViewModel)dto.User,
            Reviews = dto.Reviews.Select(r => (ReviewViewModel)r).ToHashSet(),
        };

        public static explicit operator CurriculumDto(CurriculumViewModel vm) => new CurriculumDto
        {
            Id = vm.Id,
            Title = vm.Title,
            Subject = (SubjectDto)vm.Subject,
            File = vm.File,
            FileFormat = vm.FileFormat,
            TimeStamp = vm.TimeStamp,
            DownloadCount = vm.DownloadCount,
            User = (UserDto)vm.User,
			Reviews = vm.Reviews.Select(r => (ReviewDto)r).ToHashSet(),
		};
    }
}
