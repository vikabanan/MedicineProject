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
        private int _patientId;
        private int? _previousVisitId;

        public string[] ReferralTypes { get; } =
        {
            "Нет направления",
            "Анализ крови",
            "Флюорография",
            "МРТ",
            "УЗИ",
            "Осмотр специалиста"
        };
        public string SelectedReferralType => cbReferralType.SelectedItem?.ToString();
    

        public Visit CreatedVisit { get; private set; }

        public AddVisitWindow(int patientId, int? previousVisitId = null)
        {
            InitializeComponent();
            DataContext = this;

            _patientId = patientId;
            _previousVisitId = previousVisitId;

            dpDate.SelectedDate = DateTime.Now;
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (dpDate.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату визита.");
                return;
            }

            if (cbReferralType.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип направления.");
                return;
            }

            var referralType = cbReferralType.SelectedItem.ToString();

            var visit = new Visit
            {
                VisitDate = dpDate.SelectedDate.Value,
                Diagnosis = txtDiagnosis.Text,
                Prescriptions = txtPrescriptions.Text,
                PatientId = _patientId,
                ReferralType = referralType,
                PreviousVisitId = _previousVisitId
            };

            CreatedVisit = visit;

            DialogResult = true;
            Close();
        }

    }
}

