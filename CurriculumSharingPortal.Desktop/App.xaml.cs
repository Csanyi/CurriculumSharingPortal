using CurriculumSharingPortal.Desktop.Model;
using CurriculumSharingPortal.Desktop.View;
using CurriculumSharingPortal.Desktop.ViewModel;
using System;
using System.Configuration;
using System.Windows;

namespace CurriculumSharingPortal.Desktop
{
	public partial class App : Application
    {
        private CurriculumSharingPortalApiService _service = null!;
        private MainViewModel _mainViewModel = null!;
        private LoginViewModel _loginViewModel = null!;
        private LoginWindow _loginView = null!;
        private MainWindow _mainView = null!;

        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _service = new CurriculumSharingPortalApiService(ConfigurationManager.AppSettings["baseAdress"]!);

            _loginViewModel = new LoginViewModel(_service);
            _loginViewModel.LoginSucceeded += LoginViewModel_LoginSucceeded;
            _loginViewModel.LoginFailed += LoginViewModel_LoginFailed;
            _loginViewModel.MessageApplication += ViewModel_MessageApplication;

            _loginView = new LoginWindow
            {
                DataContext = _loginViewModel
            };

            _mainViewModel = new MainViewModel(_service);
            _mainViewModel.MessageApplication += ViewModel_MessageApplication;
            _mainViewModel.LogoutSucceeded += MainViewModel_LogoutSucceeded;

            _mainView = new MainWindow()
            {
                DataContext = _mainViewModel
            };

            _loginView.Show();
        }

        private void MainViewModel_LogoutSucceeded(object? sender, EventArgs e)
        {
            _mainView.Hide();
            _loginView.Show();
        }

        private void LoginViewModel_LoginFailed(object? sender, EventArgs e)
        {
            MessageBox.Show("Login failed", "CurriculumSharingPortal", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void LoginViewModel_LoginSucceeded(object? sender, EventArgs e)
        {
            _loginView.Hide();
            _mainView.Show();
        }

        private void ViewModel_MessageApplication(object? sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "CurriculumSharingPortal", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}
