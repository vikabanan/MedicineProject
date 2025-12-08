using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineProject.Models
{
     public class Patient
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Insurance { get; set; } = null!;
        public string SNILS { get; set; } = null!;
        public string Passport { get; set; } = null!;

        public MedicalCard? MedicalCard { get; set; }
        public List<Visit> Visits { get; set; } = new();
    }
}
