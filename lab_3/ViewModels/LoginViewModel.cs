using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Npgsql;
using ReactiveUI;

namespace lab_3.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly string _connectionString = "Host=trendz-dev-do-user-15469121-0.c.db.ondigitalocean.com;Port=25060;Database=app_db;Username=doadmin;Password=AVNS_q4icRH_f4m-CzUIeqKr;SslMode=Require;Trust Server Certificate=true";
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
        // Assuming this method is called upon successful login
        

    }
}