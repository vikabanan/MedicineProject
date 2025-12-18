using MedicineProject.Models;
using System.Configuration;
using System.Data;
using System.Windows;

namespace MedicineProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var db = new MedDbContext())
            {
                db.Database.EnsureCreated(); // создаёт БД, если её нет
                db.Seed();                   // добавляет тестовые данные
            }

        }
    }

}
