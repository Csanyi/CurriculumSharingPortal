using CurriculumSharingPortal.DTO;

namespace CurriculumSharingPortal.Desktop.ViewModel
{
	public class UserViewModel : ViewModelBase
	{
		private string _id = null!;

		public string Id
		{
			get { return _id; }
			set { _id = value; OnPropertyChanged(); }
		}

		private string _userName = null!;

		public string UserName
		{
			get { return _userName; }
			set { _userName = value; OnPropertyChanged(); }
		}

		public static explicit operator UserViewModel(UserDto dto) => new UserViewModel
		{
			Id = dto.Id,
			UserName = dto.UserName,
		};

		public static explicit operator UserDto(UserViewModel vm) => new UserDto
		{
			Id = vm.Id,
			UserName = vm.UserName,
		};
	}
}
