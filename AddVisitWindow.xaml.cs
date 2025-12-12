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

            chkCreateReferral.Checked += (s, e) => cbReferralType.IsEnabled = true;
            chkCreateReferral.Unchecked += (s, e) => cbReferralType.IsEnabled = false;
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

            // создание направления
            if (chkCreateReferral.IsChecked == true && cbReferralType.SelectedItem != null)
            {
                var referral = new Referral
                {
                    ReferralDate = DateTime.Now,
                    ReferralType = cbReferralType.SelectedItem.ToString(),
                    Status = "New",
                    VisitId = visit.Id
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
