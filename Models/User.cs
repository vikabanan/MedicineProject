using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineProject.Models
{
    class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string PasswordHash { get; set; } = null!; // не хранить пароль в явном виде
        public string FullName { get; set; } = null!;
        public string Role { get; set; } = null!; // e.g. "Doctor", "Lab"
    }
}
