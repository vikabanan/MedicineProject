using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineProject.Models
{
    class MedicalCard
    {
        public int Id { get; set; }
        public string BloodGroup { get; set; } = null!;
        public string ChronicDiseases { get; set; } = null!;
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
    }
}
