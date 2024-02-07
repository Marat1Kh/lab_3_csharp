using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using lab_3.Models;
using lab_3.Views;
using ReactiveUI;

namespace lab_3.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private bool _isMainScreenVisible = true; 
        public bool IsMainScreenVisible
        {
            get => _isMainScreenVisible;
            private set => this.RaiseAndSetIfChanged(ref _isMainScreenVisible, value);
        }
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            private set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
        }
        private readonly ChampionService _championService;
    // 
        public ICommand NavigateCommand { get; private set; }
        public MainWindowViewModel()
        {
            NavigateCommand = ReactiveCommand.Create<string>(ExecuteNavigateCommand);
            InitializeViewModels();
        }
     
       
        private void InitializeViewModels()
        {
            ShowMainMenu();
        }
        // navigates screens
        private void ExecuteNavigateCommand(string destination)
        {
            Console.WriteLine($"Attempting to navigate to: {destination}");
            IsMainScreenVisible = false; 
            switch (destination)
            {
                case "Login":
                    ShowLoginView(); 
                    break;
                case "Register":
                    ShowRegisterView(); 
                    break;
                default:
                    ShowMainMenu(); 
                    break;
            }
        }

        private void OnLoginSuccess(object sender, bool isSuccess)
        {
            if (isSuccess)
            {
                CurrentViewModel = new ChampionViewModel();
                if (sender is LoginViewModel loginViewModel)
                {
                    loginViewModel.LoginResult -= OnLoginSuccess;
                }
            }
            else
            {
                Console.WriteLine("Login failed");
            }
        }

        private void OnRegisterSuccess(object sender, bool isSuccess)
        {
            if (isSuccess)
            {
                CurrentViewModel = new LoginViewModel();
                if (sender is RegisterViewModel registerViewModel)
                {
                    registerViewModel.RegistrationComplete -= OnRegisterSuccess;
                }
                else
                {
                    Console.WriteLine("Registration failed");
                }
            }
        }
        private void ShowLoginView()
        {
            var loginViewModel = new LoginViewModel();
            loginViewModel.NavigateBack += ShowMainMenu;
            loginViewModel.LoginResult += OnLoginSuccess;
            CurrentViewModel = loginViewModel;
        }

        private void ShowRegisterView()
        {
            var registerViewModel = new RegisterViewModel();
            registerViewModel.NavigateBack += ShowMainMenu;
            registerViewModel.RegistrationComplete += OnRegisterSuccess;
            CurrentViewModel = registerViewModel;
        }
        private void ShowMainMenu()
        {
            IsMainScreenVisible = true;
            CurrentViewModel = null;
        }
    }
}
