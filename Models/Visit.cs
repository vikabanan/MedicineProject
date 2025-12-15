using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineProject.Models
{
     public class Visit
     {
        public int Id { get; set; }
        public DateTime VisitDate { get; set; }
        public string Diagnosis { get; set; } = null!;
        public string Prescriptions { get; set; } = null!;
        public string? ReferralType { get; set; }      
        public int? PreviousVisitId { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
        public List<Referral> Referrals { get; set; } = new();

     }

}
