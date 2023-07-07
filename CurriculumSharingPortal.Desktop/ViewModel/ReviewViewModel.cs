using CurriculumSharingPortal.DTO;

namespace CurriculumSharingPortal.Desktop.ViewModel
{
	public class ReviewViewModel : ViewModelBase
	{
		private int _id;

		public int Id
		{
			get { return _id; }
			set { _id = value; OnPropertyChanged(); }
		}

		private int _value;

		public int Value
		{
			get { return _value; }
			set { _value = value; OnPropertyChanged(); }
		}

		public static explicit operator ReviewViewModel(ReviewDto dto) => new ReviewViewModel
		{
			Id = dto.Id,
			Value = dto.Value,
		};

		public static explicit operator ReviewDto(ReviewViewModel vm) => new ReviewDto
		{
			Id = vm.Id,
			Value = vm.Value,
		};
	}
}
