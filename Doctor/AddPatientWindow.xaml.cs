using MedicineProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MedicineProject
{
    /// <summary>
    /// Логика взаимодействия для AddPatientWindow.xaml
    /// </summary>
    public partial class AddPatientWindow : Window
    {
        private readonly Brush _defaultBorderBrush;
        private readonly Brush _defaultComboBoxBorderBrush;

        public AddPatientWindow()
        {
            InitializeComponent();

            _defaultBorderBrush = txtFullName.BorderBrush;
            _defaultComboBoxBorderBrush = borderGender.BorderBrush;

            cbBloodGroup.ItemsSource = new[]
            {
                "I (0) Rh+",
                "I (0) Rh-",
                "II (A) Rh+",
                "II (A) Rh-",
                "III (B) Rh+",
                "III (B) Rh-",
                "IV (AB) Rh+",
                "IV (AB) Rh-"
            };
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
                return;

            using var db = new MedDbContext();

            var patient = new Patient
            {
                FullName = txtFullName.Text.Trim(),
                BirthDate = dpBirthDate.SelectedDate.Value,
                Gender = (cbGender.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Phone = txtPhone.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Insurance = txtInsurance.Text.Trim(),
                SNILS = txtSNILS.Text.Trim(),
                Passport = txtPassport.Text.Trim(),
                MedicalCard = new MedicalCard
                {
                    BloodGroup = cbBloodGroup.SelectedItem.ToString(),
                    ChronicDiseases = txtChronicDiseases.Text
                }
            };

            db.Patients.Add(patient);
            db.SaveChanges();

            MessageBox.Show("Пациент успешно добавлен");
            DialogResult = true;
        }

        private bool ValidateInputs()
        {
            ClearValidation();

            // Порядок проверки соответствует порядку полей в XAML
            // 1. ФИО
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
                return SetError(txtFullName, "Введите ФИО");

            // 2. Дата рождения
            if (!dpBirthDate.SelectedDate.HasValue)
                return SetError(dpBirthDate, "Выберите дату рождения");

            // 3. Пол
            if (cbGender.SelectedItem == null)
                return SetComboBoxError(borderGender, cbGender, "Выберите пол");

            // 4. Телефон
            if (txtPhone.Text.Length < 11 || !txtPhone.Text.All(char.IsDigit))
                return SetError(txtPhone, "Телефон должен содержать 11 цифр");

            // 5. Адрес
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
                return SetError(txtAddress, "Введите адрес");

            // 6. Полис
            if (string.IsNullOrWhiteSpace(txtInsurance.Text))
                return SetError(txtInsurance, "Введите номер полиса");

            // 7. СНИЛС
            if (txtSNILS.Text.Length != 11 || !txtSNILS.Text.All(char.IsDigit))
                return SetError(txtSNILS, "СНИЛС должен содержать 11 цифр");

            // 8. Паспорт
            if (string.IsNullOrWhiteSpace(txtPassport.Text))
                return SetError(txtPassport, "Введите паспортные данные");

            // 9. Группа крови
            if (cbBloodGroup.SelectedItem == null)
                return SetComboBoxError(borderBloodGroup, cbBloodGroup, "Выберите группу крови");

            return true;
        }

        private void ClearValidation()
        {
            // Очистка TextBox и DatePicker
            foreach (var control in new Control[]
                     { txtFullName, dpBirthDate, txtAddress, txtPhone, txtSNILS, txtInsurance, txtPassport })
            {
                control.BorderBrush = _defaultBorderBrush;
                control.ClearValue(Border.BorderThicknessProperty);
                control.ToolTip = null;
            }

            // Очистка ComboBox (через Border)
            borderGender.BorderBrush = _defaultComboBoxBorderBrush;
            borderGender.ClearValue(Border.BorderThicknessProperty);
            borderGender.ToolTip = null;
            cbGender.ToolTip = null;

            borderBloodGroup.BorderBrush = _defaultComboBoxBorderBrush;
            borderBloodGroup.ClearValue(Border.BorderThicknessProperty);
            borderBloodGroup.ToolTip = null;
            cbBloodGroup.ToolTip = null;
        }

        private bool SetError(Control control, string message)
        {
            control.BorderBrush = Brushes.Red;
            control.BorderThickness = new Thickness(1.5);
            control.ToolTip = message;
            control.Focus();
            return false;
        }

        private bool SetComboBoxError(Border border, ComboBox comboBox, string message)
        {
            border.BorderBrush = Brushes.Red;
            border.BorderThickness = new Thickness(1.5);
            border.ToolTip = message;
            comboBox.ToolTip = message;
            comboBox.Focus();
            return false;
        }

    }
}
