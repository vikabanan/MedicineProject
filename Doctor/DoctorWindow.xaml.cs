using MedicineProject.Models;
using Microsoft.EntityFrameworkCore;
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
    /// Логика взаимодействия для DoctorWindow.xaml
    /// </summary>
    public partial class DoctorWindow : Window
    {
        private MedDbContext _db = new MedDbContext();

        public DoctorWindow()
        {
            InitializeComponent();
            LoadPatients();
        }

        // загрузка пациентов, медкарты 
        private void LoadPatients()
        {
            lstPatients.ItemsSource = _db.Patients
             .Include(p => p.MedicalCard)
             .OrderBy(p => p.FullName)
             .ToList();
        }

        //выбор пациента
        private void lstPatients_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lstPatients.SelectedItem is not Patient p)
                return;

            txtFullName.Text = p.FullName;
            txtDOB.Text = p.BirthDate.ToShortDateString();
            txtGender.Text = p.Gender;
            txtInsurance.Text = p.Insurance;
            txtSNILS.Text = p.SNILS;
            txtPassport.Text = p.Passport;
            txtPhone.Text = p.Phone;

            //медкарта
            if (p.MedicalCard != null)
            {
                txtBloodGroup.Text = p.MedicalCard.BloodGroup;
                txtChronicDiseases.Text = p.MedicalCard.ChronicDiseases;
            }
            else
            {
                txtBloodGroup.Text = "";
                txtChronicDiseases.Text = "";
            }

            LoadVisits(p.Id);
        }

        //загрузка визитов
        private void LoadVisits(int patientId)
        {
            _db = new MedDbContext();

            dgVisits.ItemsSource = _db.Visits
               .Where(v => v.PatientId == patientId)
               .Include(v => v.Referrals)   // на будущее
               .OrderByDescending(v => v.VisitDate)
               .ToList();
        }

        //выбор визита
        private void dgVisits_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgVisits.SelectedItem is not Visit visit)
                return;

            // здесь можно будет выводить направления отдельно
        }


        private void btnAddVisit_Click(object sender, RoutedEventArgs e)
        {
            if (lstPatients.SelectedItem is not Patient patient)
            {
                MessageBox.Show("Выберите пациента.");
                return;
            }

            var win = new AddVisitWindow(patient.Id)
            { Owner = this };

            if (win.ShowDialog() == true)
            {
                LoadVisits(patient.Id);
            }
        }


        private void btnAddReferral_Click(object sender, RoutedEventArgs e)
        {
            if (lstPatients.SelectedItem is not Patient patient)
            {
                MessageBox.Show("Выберите пациента.");
                return;
            }

            if (dgVisits.SelectedItem is not Visit visit)
            {
                MessageBox.Show("Выберите визит, к которому хотите добавить направление.");
                return;
            }

            // окно добавления направления 
            var win = new AddReferralWindow(visit.Id)
            {
                Owner = this
            };

            if (win.ShowDialog() == true)
                LoadVisits(patient.Id);
        }

        private void dgVisits_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgVisits.SelectedItem is not Visit visit)
                return;

            var win = new VisitDetailsWindow(visit.Id)
            {
                Owner = this
            };

            if (win.ShowDialog() == true)
            {
                if (lstPatients.SelectedItem is Patient patient)
                    LoadVisits(patient.Id);
            }
        }

        private void btnAddPatient_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddPatientWindow();
            if (win.ShowDialog() == true)
            {
                LoadPatients(); 
            }
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



    
