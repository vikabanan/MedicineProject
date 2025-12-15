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
    /// Логика взаимодействия для AddVisitWindow.xaml
    /// </summary>
    public partial class AddVisitWindow : Window
    {
        private readonly MedDbContext _db = new MedDbContext();
        private readonly int _patientId;
        public Visit CreatedVisit { get; private set; }

        private readonly string[] ReferralTypes =
        {
            "Анализ крови",
            "УЗИ",
            "МРТ",
            "Флюорография",
            "Осмотр специалиста"
        };

        public AddVisitWindow(int patientId)
        {
            InitializeComponent();
            _patientId = patientId;

            dpDate.SelectedDate = DateTime.Now;

            cbReferralType.ItemsSource = ReferralTypes;

            chkCreateReferral.Checked += (s, e) => EnableReferralControls(true);
            chkCreateReferral.Unchecked += (s, e) => EnableReferralControls(false);

            dpDate.SelectedDateChanged += DpDate_SelectedDateChanged;
            cbReferralType.SelectionChanged += CbReferralType_SelectionChanged;

            cbReferralTime.IsEnabled = false;
        }

        private void EnableReferralControls(bool enabled)
        {
            cbReferralType.IsEnabled = enabled;
            cbReferralTime.IsEnabled = enabled && cbReferralType.SelectedItem != null && dpDate.SelectedDate.HasValue;
        }

        private void DpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAvailableTimes();
        }

        private void CbReferralType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAvailableTimes();
        }

        private void UpdateAvailableTimes()
        {
            if (chkCreateReferral.IsChecked == true && cbReferralType.SelectedItem != null && dpDate.SelectedDate.HasValue)
            {
                cbReferralTime.IsEnabled = true;

                var timeService = new TimeSlotService(_db);
                cbReferralTime.ItemsSource = timeService.GetAvailableReferralTimes(dpDate.SelectedDate.Value, cbReferralType.SelectedItem.ToString()!);

                if (cbReferralTime.Items.Count > 0)
                    cbReferralTime.SelectedIndex = 0;
            }
            else
            {
                cbReferralTime.ItemsSource = new List<string>();
                cbReferralTime.IsEnabled = false;
            }
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (dpDate.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату!");
                return;
            }

            var visit = new Visit
            {
                VisitDate = dpDate.SelectedDate.Value,
                Diagnosis = txtDiagnosis.Text,
                Prescriptions = txtPrescriptions.Text,
                PatientId = _patientId
            };

            _db.Visits.Add(visit);
            _db.SaveChanges();

            if (chkCreateReferral.IsChecked == true && cbReferralType.SelectedItem != null && cbReferralTime.SelectedItem != null)
            {
                var scheduledTime = TimeSpan.Parse(cbReferralTime.SelectedItem.ToString()!);
                var referralDateTime = dpDate.SelectedDate.Value.Date + scheduledTime;

                var referral = new Referral
                {
                    VisitId = visit.Id,
                    ReferralType = cbReferralType.SelectedItem.ToString()!,
                    Status = ReferralStatus.New,
                    ReferralDate = referralDateTime
                };

                _db.Referrals.Add(referral);
                _db.SaveChanges();
            }

            CreatedVisit = visit;

            MessageBox.Show("Визит успешно создан!");
            DialogResult = true;
            Close();
        }
    }
}

