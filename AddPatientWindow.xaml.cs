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
        public AddPatientWindow()
        {
            InitializeComponent();

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
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Введите ФИО");
                txtFullName.Focus();
                return;
            }

            if (!dpBirthDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Выберите дату рождения");
                return;
            }

            if (cbGender.SelectedItem == null)
            {
                MessageBox.Show("Выберите пол");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Введите адрес");
                return;
            }

            if (cbBloodGroup.SelectedItem == null)
            {
                MessageBox.Show("Выберите группу крови");
                return;
            }

            if (txtPhone.Text.Length < 11 || !txtPhone.Text.All(char.IsDigit))
            {
                MessageBox.Show("Телефон должен содержать минимум 11 цифр");
                return;
            }

            if (txtSNILS.Text.Length != 11 || !txtSNILS.Text.All(char.IsDigit))
            {
                MessageBox.Show("СНИЛС должен содержать 11 цифр");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtInsurance.Text))
            {
                MessageBox.Show("Введите номер полиса");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassport.Text))
            {
                MessageBox.Show("Введите паспортные данные");
                return;
            }

            using var db = new MedDbContext();

            var patient = new Patient
            {
                FullName = txtFullName.Text.Trim(),
                BirthDate = dpBirthDate.SelectedDate.Value,
                Gender = (cbGender.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Phone = txtPhone.Text,
                Address = txtAddress.Text,
                Insurance = txtInsurance.Text,
                SNILS = txtSNILS.Text,
                Passport = txtPassport.Text,
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

    }
}
