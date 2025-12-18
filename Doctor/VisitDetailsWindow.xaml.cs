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
    /// Логика взаимодействия для VisitDetailsWindow.xaml
    /// </summary>
    public partial class VisitDetailsWindow : Window
    {
        private readonly MedDbContext _db;
        private readonly Visit _visit;

        public VisitDetailsWindow(int visitId)
        {
            InitializeComponent();

            _db = new MedDbContext();

            _visit = _db.Visits
                .Include(v => v.Referrals)
                .ThenInclude(r => r.LabResults)
                .First(v => v.Id == visitId);

            DataContext = _visit;
        }

        private void BtnSaveVisit_Click(object sender, RoutedEventArgs e)
        {
            _db.SaveChanges();
            MessageBox.Show("Изменения сохранены");
            DialogResult = true;
        }

        private void BtnDeleteReferral_Click(object sender, RoutedEventArgs e)
        {
            if (dgReferrals.SelectedItem is not Referral referral)
            {
                MessageBox.Show("Выберите направление");
                return;
            }

            if (MessageBox.Show(
                "Удалить выбранное направление?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            _db.Referrals.Remove(referral);
            _db.SaveChanges();

            _visit.Referrals.Remove(referral);
            dgReferrals.Items.Refresh();
            DialogResult = true;
        }
    }
}

