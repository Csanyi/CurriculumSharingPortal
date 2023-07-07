using CurriculumSharingPortal.Desktop.Model;
using CurriculumSharingPortal.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CurriculumSharingPortal.Desktop.ViewModel
{
	public class StringNotJustSpaceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string s = (string)value;
                if (s.All(c => c == ' '))
                {
                    return DependencyProperty.UnsetValue;
                }

                return s;
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }

    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private readonly CurriculumSharingPortalApiService _service;
        private SubjectViewModel? _selectedSubject;
        private CurriculumViewModel? _selectedCurriculum;
        private ObservableCollection<SubjectViewModel> _subjects;
        private ObservableCollection<CurriculumViewModel> _curriculums;
        private string? _selectedSubjectName;
        private string? _selectedCurriculumName;
        private string? _searchString;

        #endregion

        #region Properties

        public ObservableCollection<SubjectViewModel> Subjects
        {
            get { return _subjects; }
            set { _subjects = value; OnPropertyChanged(); }
        }

        public ObservableCollection<CurriculumViewModel> Curriculums
        {
            get { return _curriculums; }
            set { _curriculums = value; OnPropertyChanged(); }

        }

        public SubjectViewModel? SelectedSubject 
        { 
            get { return _selectedSubject; } 
            set { _selectedSubject = value; OnPropertyChanged(); }
        }

        public CurriculumViewModel? SelectedCurriculum
        {
            get { return _selectedCurriculum; }
            set { _selectedCurriculum = value; OnPropertyChanged(); }
        }

        public string? SelectedSubjectName
        {
            get { return _selectedSubjectName; }
            set { _selectedSubjectName = value; OnPropertyChanged(); }
        }

        public string? SelectedCurriculumName
        {
            get { return _selectedCurriculumName; }
            set { _selectedCurriculumName = value; OnPropertyChanged(); }
        }

        public string? SearchString
        {
            get { return _searchString; }
            set { _searchString = value; OnPropertyChanged(); }
        }

        public DelegateCommand LoadSubjectsCommand { get; private set; }
        public DelegateCommand SelectCommand { get; private set; }
        public DelegateCommand LogoutCommand { get; private set; }
        public DelegateCommand AddSubjectCommand { get; private set; }
        public DelegateCommand EditSubjectCommand { get; private set; }
        public DelegateCommand DeleteSubjectCommand { get; private set; }
        public DelegateCommand EditCurriculumCommand { get; private set; }
        public DelegateCommand ResetDownLoadCountCommand { get; private set; }
        public DelegateCommand DeleteReviewsCommand { get; private set; }
        public DelegateCommand DeleteCurriculumCommand { get; private set; }
        public DelegateCommand SearchCommand { get; private set; }


        #endregion

        #region Events

        public event EventHandler? LogoutSucceeded;

        #endregion

        #region Constructors

        public MainViewModel(CurriculumSharingPortalApiService service)
        {
            _service = service;

            LoadSubjectsCommand = new DelegateCommand(_ => LoadSubjectsAsync());
            SelectCommand = new DelegateCommand(_ => LoadCurriculumsAsync());
            LogoutCommand = new DelegateCommand(_ => LogoutAsync());

            AddSubjectCommand = new DelegateCommand(_ => AddSubjectAsync());
            EditSubjectCommand = new DelegateCommand(_ => SelectedSubject is not null,_ => { EditSubjectAsync().Wait(); });
            DeleteSubjectCommand = new DelegateCommand(_ => SelectedSubject is not null && Curriculums.Count == 0, _ => DeleteSubjectAsync());

            EditCurriculumCommand = new DelegateCommand(_ => SelectedCurriculum is not null, _ => EditCurrciculumAsync());
            ResetDownLoadCountCommand = new DelegateCommand(_ => SelectedCurriculum is not null, _ => ResetDownLoadCountAsync());
            DeleteReviewsCommand = new DelegateCommand(_ => SelectedCurriculum is not null, _ => DeleteReviewsAsync());
            DeleteCurriculumCommand = new DelegateCommand(_ => SelectedCurriculum is not null, _ => DeleteCurriculumAsync());

            SearchCommand = new DelegateCommand(_ => SearchAsync());

            _subjects = new ObservableCollection<SubjectViewModel>();
            _curriculums = new ObservableCollection<CurriculumViewModel>();
        }

        #endregion

        #region Authorization

        private async void LogoutAsync()
        {
            try
            {
                await _service.LogoutAsync();

                LogoutSucceeded?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        #endregion

        #region Subjects

        private async Task LoadSubjectsAsync()
        {
            try
            {
                Subjects = new ObservableCollection<SubjectViewModel>((await _service.LoadSubjectsAsync()).Select(s => (SubjectViewModel)s));
            } 
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        private async Task AddSubjectAsync()
        {
            if(SelectedSubjectName is null)
            {
                return;
            }

            var newSubject = new SubjectViewModel
            {
                Name = SelectedSubjectName,
            };

            SelectedSubjectName = null;

            var subjectDto = (SubjectDto)newSubject;

            try
            {
                await _service.CreateSubjectAsync(subjectDto);
                newSubject.Id = subjectDto.Id;
                Subjects.Add(newSubject);
                SelectedSubject = newSubject;
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
        }

        private async Task EditSubjectAsync()
        {
            if(SelectedSubjectName is null || SelectedSubject is null)
            {
                return;
            }

            try
            {
                SelectedSubject.Name = SelectedSubjectName;
                await _service.UpdateSubjectAsync((SubjectDto)SelectedSubject);
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }

            SelectedSubjectName = null;
        }

        private async Task DeleteSubjectAsync()
        {
            if(SelectedSubject is null)
            {
                return;
            }

            await LoadCurriculumsAsync();

            if(Curriculums.Any()) { return; }

            try
            {
                await _service.DeleteSubjectAsync(SelectedSubject.Id);
                Subjects.Remove(SelectedSubject);
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
           
        }

        #endregion

        #region Curriculums

        private async Task LoadCurriculumsAsync()
        {
            if(SelectedSubject is null) { return; }

            try
            {
                Curriculums = new ObservableCollection<CurriculumViewModel>((await _service.LoadCurriculumsAsync(SelectedSubject.Id)).Select(c => (CurriculumViewModel)c));
				SearchString = null;
			}
			catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        private async Task SearchAsync()
        {
            if (SearchString is null) { return; }

            try
            {
                Curriculums = new ObservableCollection<CurriculumViewModel>((await _service.LoadCurriculumsAsync(SearchString)).Select(c => (CurriculumViewModel)c));
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        private async Task DeleteCurriculumAsync()
        {
            if(SelectedCurriculum is null) { return; }

            try
            {
                await _service.DeleteCurriculumAsync(SelectedCurriculum.Id);
                Curriculums.Remove(SelectedCurriculum);
                SelectedCurriculum = null;
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        private async Task DeleteReviewsAsync()
        {
            if (SelectedCurriculum is null) { return; }

            try
            {
                await _service.DeleteReviewsAsync(SelectedCurriculum.Id);
                SelectedCurriculum.Reviews = new HashSet<ReviewViewModel>();
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
        }

        private async Task ResetDownLoadCountAsync()
        {
            if (SelectedCurriculum is null) { return; }

            try
            {
                SelectedCurriculum.DownloadCount = 0;
                await _service.UpdateCurriculumAsync((CurriculumDto)SelectedCurriculum);
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
        }

        private async Task EditCurrciculumAsync()
        {
            if(SelectedCurriculum is null || SelectedCurriculumName is null) { return; }

            try
            {
                SelectedCurriculum.Title = SelectedCurriculumName;
                await _service.UpdateCurriculumAsync((CurriculumDto)SelectedCurriculum);
				SelectedCurriculumName = null;
			}
			catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
        }

        #endregion
    }
}
