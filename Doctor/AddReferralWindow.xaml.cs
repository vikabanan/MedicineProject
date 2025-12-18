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
using MedicineProject.Doctor;

namespace MedicineProject
{
    /// <summary>
    /// Логика взаимодействия для AddReferralWindow.xaml
    /// </summary>
    public partial class AddReferralWindow : Window
    {
        private readonly int _visitId;
        private readonly MedDbContext _db = new MedDbContext();
        private readonly TimeSlotService _timeService;

        private readonly string[] ReferralTypes =
        {
            "Анализ крови",
            "УЗИ",
            "МРТ",
            "Флюорография",
            "Осмотр специалиста"
        };

        public AddReferralWindow(int visitId)
        {
            InitializeComponent();
            _visitId = visitId;
            _timeService = new TimeSlotService(_db);

            cbType.ItemsSource = ReferralTypes;

            var visit = _db.Visits.Find(_visitId);
            if (visit != null)
            {
                dpDate.SelectedDate = visit.VisitDate.Date;
                LoadAvailableTimes();
            }
        }

        private void DpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadAvailableTimes();
        }

        private void CbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadAvailableTimes();
        }

        private void LoadAvailableTimes()
        {
            if (!dpDate.SelectedDate.HasValue || cbType.SelectedItem == null)
            {
                cbTime.ItemsSource = new List<string>();
                return;
            }

            string type = cbType.SelectedItem.ToString()!;
            DateTime date = dpDate.SelectedDate.Value;

            cbTime.ItemsSource = _timeService.GetAvailableReferralTimes(date, type);

            if (cbTime.Items.Count > 0)
                cbTime.SelectedIndex = 0;
        }

        private void BtnCreateReferral_Click(object sender, RoutedEventArgs e)
        {
            if (cbType.SelectedItem == null || !dpDate.SelectedDate.HasValue || cbTime.SelectedItem == null)
            {
                MessageBox.Show("Выберите дату, тип и время!");
                return;
            }

            var visit = _db.Visits.Find(_visitId);
            if (visit == null) return;

            var selectedTime = TimeSpan.Parse(cbTime.SelectedItem.ToString()!);
            var scheduledDateTime = dpDate.SelectedDate.Value.Date + selectedTime;

            var referral = new Referral
            {
                VisitId = _visitId,
                ReferralType = cbType.SelectedItem.ToString()!,
                Status = ReferralStatus.New,
                ReferralDate = scheduledDateTime
            };

            _db.Referrals.Add(referral);
            _db.SaveChanges();

            MessageBox.Show("Направление добавлено!");
            DialogResult = true;
            Close();
        }
    }
}


