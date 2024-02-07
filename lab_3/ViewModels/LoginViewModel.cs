using System;
using System.Threading.Tasks;
using System.Windows.Input;
using lab_3.Models;
using Npgsql;
using ReactiveUI;

namespace lab_3.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly string _connectionString =  DbHelper.ConnectionString;
        public string Username { get; set; }
        public event Action NavigateBack;
        public string Password { get; set; }
        public ICommand LoginCommand { get; }
        public ICommand GoBackCommand { get; }
        public event EventHandler<bool> LoginResult;

        public LoginViewModel()
        {
            LoginCommand = ReactiveCommand.CreateFromTask(ExecuteLoginCommandAsync);
            GoBackCommand = ReactiveCommand.Create(() =>
            {
                NavigateBack?.Invoke();
            });
        }

        private async Task ExecuteLoginCommandAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand("SELECT * FROM users WHERE username = @username AND password = @password", connection))
                {
                    command.Parameters.AddWithValue("username", Username);
                    command.Parameters.AddWithValue("password", Password);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var result = await reader.ReadAsync();
                        LoginResult?.Invoke(this, true);
                    }
                }
            }
        }
   
    }
}