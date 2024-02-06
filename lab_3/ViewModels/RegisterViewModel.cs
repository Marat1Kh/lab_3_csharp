using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using lab_3.Views;
using Npgsql;
using ReactiveUI;

namespace lab_3.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        public ICommand GoBackCommand { get; }
        public event Action NavigateBack;
        private readonly string _connectionString =
            "Host=trendz-dev-do-user-15469121-0.c.db.ondigitalocean.com;Port=25060;Database=app_db;Username=doadmin;Password=AVNS_q4icRH_f4m-CzUIeqKr;SslMode=Require;Trust Server Certificate=true";

        private string _username;
        private string _password;

        public string Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }
    
        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }
   
        public ICommand RegisterCommand { get; }

        public event EventHandler<bool> RegistrationComplete;

        public RegisterViewModel()
        {
            RegisterCommand = ReactiveCommand.CreateFromTask(ExecuteRegisterCommandAsync);
            GoBackCommand = ReactiveCommand.Create(() => NavigateBack?.Invoke());
        }

        private async Task ExecuteRegisterCommandAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Check if username already exists
                using (var checkCommand =
                       new NpgsqlCommand("SELECT COUNT(*) FROM users WHERE username = @username", connection))
                {
                    checkCommand.Parameters.AddWithValue("username", Username);
                    var userExists = (long)await checkCommand.ExecuteScalarAsync() > 0;

                    if (userExists)
                    {
                        RegistrationComplete?.Invoke(this, false);
                        return;
                    }
                }

                // Insert new user if username does not exist
                using (var command =
                       new NpgsqlCommand("INSERT INTO users (username, password) VALUES (@username, @password)",
                           connection))
                {
                    command.Parameters.AddWithValue("username", Username);
                    // Hash the password before storing it
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);
                    command.Parameters.AddWithValue("password", hashedPassword);
                    var result = await command.ExecuteNonQueryAsync();
                    RegistrationComplete?.Invoke(this, true);
                }
            }
        }

    }
}