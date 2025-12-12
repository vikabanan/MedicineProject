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
    /// Логика взаимодействия для AddReferralWindow.xaml
    /// </summary>
    public partial class AddReferralWindow : Window
    {
        private readonly int _visitId;
        private readonly MedDbContext _db = new MedDbContext();

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

            cbType.ItemsSource = ReferralTypes;
        }

        private void BtnCreateReferral_Click(object sender, RoutedEventArgs e)
        {
            if (cbType.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип направления!");
                return;
            }

            var referral = new Referral
            {
                ReferralDate = DateTime.Now,
                ReferralType = cbType.SelectedItem.ToString(),
                Status = "New",
                VisitId = _visitId
            };

            _db.Referrals.Add(referral);
            _db.SaveChanges();

            MessageBox.Show("Направление добавлено!");

            DialogResult = true;
            Close();
        }
    }
}
