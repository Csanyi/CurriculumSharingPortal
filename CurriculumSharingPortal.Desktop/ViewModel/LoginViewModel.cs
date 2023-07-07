using CurriculumSharingPortal.Desktop.Model;
using System;
using System.Net.Http;
using System.Windows.Controls;

namespace CurriculumSharingPortal.Desktop.ViewModel
{
	public class LoginViewModel : ViewModelBase
    {
        private readonly CurriculumSharingPortalApiService _service;

        public bool IsLoading { get; set; }

        public string UserName { get; set; } = null!;

        public DelegateCommand LoginCommand { get; private set; }

        public event EventHandler? LoginSucceeded;
        public event EventHandler? LoginFailed;

        public LoginViewModel(CurriculumSharingPortalApiService service) 
        {
            _service = service;

            IsLoading = false;

            LoginCommand = new DelegateCommand(_ => !IsLoading, param => LoginAsync(param as PasswordBox));
        }

        private async void LoginAsync(PasswordBox? passwordBox)
        {
            if(passwordBox == null)
            {
                return;
            }

            try
            {
                IsLoading = true;
                var result = await _service.LoginAsync(UserName, passwordBox.Password);

                if (result)
                {
                    LoginSucceeded?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    LoginFailed?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
            finally
            {
                passwordBox.Password = null!;
                IsLoading = false;
            }
        }
    }
}
