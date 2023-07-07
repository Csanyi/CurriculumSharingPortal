using CurriculumSharingPortal.DTO;

namespace CurriculumSharingPortal.Desktop.ViewModel
{
	public class SubjectViewModel : ViewModelBase
    {
		private int _id;

		public int Id
		{
			get { return _id; }
			set { _id = value; OnPropertyChanged(); }
		}

		private string _name = null!;

		public string Name
		{
			get { return _name; }
			set { _name = value; OnPropertyChanged(); }
		}

		public static explicit operator SubjectViewModel(SubjectDto dto) => new SubjectViewModel
		{
			Id = dto.Id,
			Name = dto.Name,
		};

		public static explicit operator SubjectDto(SubjectViewModel vm) => new SubjectDto
		{
			Id = vm.Id,
			Name = vm.Name,
		};
    }
}
