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
using Microsoft.EntityFrameworkCore;

namespace MedicineProject
{
    /// <summary>
    /// Логика взаимодействия для LaborantWindow.xaml
    /// </summary>
    public partial class LaborantWindow : Window
    {
        private readonly MedDbContext _db = new();
        private List<ReferralViewModel> _allReferrals = new();

        public LaborantWindow()
        {
            InitializeComponent();
            LoadReferrals();
        }

        private void LoadReferrals()
        {
            _allReferrals = _db.Referrals
                .Include(r => r.Visit)
                    .ThenInclude(v => v.Patient)
                .Select(r => new ReferralViewModel
                {
                    ReferralId = r.Id,
                    PatientName = r.Visit.Patient.FullName,
                    ReferralType = r.ReferralType,
                    ReferralDate = r.ReferralDate,
                    Status = r.Status.ToString()
                })
                .ToList();

            dgReferrals.ItemsSource = _allReferrals;
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            string patient = txtPatientSearch.Text.Trim().ToLower();
            string type = txtTypeSearch.Text.Trim().ToLower();

            var filtered = _allReferrals.Where(r =>
                (string.IsNullOrEmpty(patient) || r.PatientName.ToLower().Contains(patient)) &&
                (string.IsNullOrEmpty(type) || r.ReferralType.ToLower().Contains(type))
            ).ToList();

            dgReferrals.ItemsSource = filtered;
        }

        private void btnResetFilter_Click(object sender, RoutedEventArgs e)
        {
            txtPatientSearch.Clear();
            txtTypeSearch.Clear();
            dgReferrals.ItemsSource = _allReferrals;
        }

        private void btnSaveResults_Click(object sender, RoutedEventArgs e)
        {
            if (dgReferrals.SelectedItem is not ReferralViewModel selected)
            {
                MessageBox.Show("Выберите направление");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtResultDescription.Text) ||
                string.IsNullOrWhiteSpace(txtConclusion.Text))
            {
                MessageBox.Show("Заполните описание и заключение");
                return;
            }

            using var db = new MedDbContext();

            var referral = db.Referrals
                .Include(r => r.LabResults)
                .First(r => r.Id == selected.ReferralId);

            // 🔴 ГЛАВНАЯ ПРОВЕРКА
            if (referral.LabResults.Any())
            {
                MessageBox.Show(
                    "Результаты для этого направления уже добавлены.\nПовторное добавление невозможно.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            var result = new LabResult
            {
                Description = txtResultDescription.Text.Trim(),
                Conclusion = txtConclusion.Text.Trim(),
                ReferralId = referral.Id
            };

            db.LabResults.Add(result);

            referral.Status = ReferralStatus.Done;

            db.SaveChanges();

            MessageBox.Show("Результаты сохранены");

            txtResultDescription.Clear();
            txtConclusion.Clear();

            LoadReferrals(); // обновляем таблицу
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            loginWindow.Show();
            this.Close();
        }

    }
}
