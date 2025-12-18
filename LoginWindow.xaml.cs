using MedicineProject.Models;
using MedicineProject.Services;
using System.Windows;
using System.Windows.Input;

namespace MedicineProject
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            txtLogin.Focus();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            AttemptLogin();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AttemptLogin();
            }
        }

        private void AttemptLogin()
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                ShowError("Введите логин и пароль");
                return;
            }

            using (var db = new MedDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Login == login);

                if (user == null || !PasswordHasher.VerifyPassword(password, user.PasswordHash))
                {
                    ShowError("Неверный логин или пароль");
                    txtPassword.Clear();
                    return;
                }

                // Успешный вход - открываем соответствующее окно
                OpenRoleWindow(user.Role);
                this.Close();
            }
        }

        private void OpenRoleWindow(string role)
        {
            Window? window = null;

            switch (role)
            {
                case "Doctor":
                    window = new DoctorWindow();
                    break;
                case "Laborant":
                case "Lab": // Поддержка обоих вариантов названия роли
                    window = new LaborantWindow();
                    break;
                default:
                    MessageBox.Show($"Неизвестная роль: {role}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }

            if (window != null)
            {
                window.Show();
            }
        }

        private void ShowError(string message)
        {
            txtError.Text = message;
            txtError.Visibility = Visibility.Visible;
        }

        private void HideError()
        {
            txtError.Visibility = Visibility.Collapsed;
            txtError.Text = "";
        }

        // ---------- Регистрация ----------

        private void btnRegisterNewUser_Click(object sender, RoutedEventArgs e)
        {
            AttemptRegistration();
        }

        private void txtPasswordReg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtPasswordConfirmReg.Focus();
            }
        }

        private void txtPasswordConfirmReg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AttemptRegistration();
            }
        }

        private void AttemptRegistration()
        {
            // Очищаем предыдущие ошибки
            HideError();

            // Получаем данные из формы регистрации
            string login = txtLoginReg.Text.Trim();
            string password = txtPasswordReg.Password;
            string passwordConfirm = txtPasswordConfirmReg.Password;
            string fullName = txtFullNameReg.Text.Trim();

            var selectedRole = cmbRoleReg.SelectedItem as System.Windows.Controls.ComboBoxItem;
            string role = selectedRole?.Tag?.ToString() ?? "Doctor";

            // Валидация
            if (string.IsNullOrWhiteSpace(login))
            {
                ShowError("Введите логин");
                txtLoginReg.Focus();
                return;
            }

            if (login.Length < 3)
            {
                ShowError("Логин должен содержать минимум 3 символа");
                txtLoginReg.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Введите пароль");
                txtPasswordReg.Focus();
                return;
            }

            if (password.Length < 6)
            {
                ShowError("Пароль должен содержать минимум 6 символов");
                txtPasswordReg.Focus();
                return;
            }

            if (password != passwordConfirm)
            {
                ShowError("Пароли не совпадают");
                txtPasswordConfirmReg.Focus();
                txtPasswordConfirmReg.Clear();
                return;
            }

            if (string.IsNullOrWhiteSpace(fullName))
            {
                ShowError("Введите ФИО");
                txtFullNameReg.Focus();
                return;
            }

            // Проверка на существование пользователя и создание нового
            using (var db = new MedDbContext())
            {
                if (db.Users.Any(u => u.Login == login))
                {
                    ShowError("Пользователь с таким логином уже существует");
                    txtLoginReg.Focus();
                    return;
                }

                var newUser = new User
                {
                    Login = login,
                    PasswordHash = PasswordHasher.HashPassword(password),
                    FullName = fullName,
                    Role = role
                };

                try
                {
                    db.Users.Add(newUser);
                    db.SaveChanges();

                    MessageBox.Show(
                        "Регистрация успешно завершена! Теперь вы можете войти, используя свои данные.",
                        "Успех",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    // Автозаполнение полей входа
                    txtLogin.Text = login;
                    txtPassword.Clear();
                }
                catch (System.Exception ex)
                {
                    ShowError($"Ошибка при регистрации: {ex.Message}");
                }
            }
        }
    }
}
