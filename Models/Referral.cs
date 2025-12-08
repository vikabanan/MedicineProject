using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineProject.Models
{
    public class Referral
    {
        public int Id { get; set; }
        public string ReferralType { get; set; } = null!;
        public DateTime ReferralDate { get; set; }
        public string Status { get; set; } = "New"; // New, InProgress, Done
        public int VisitId { get; set; }
        public Visit Visit { get; set; } = null!;

        public List<LabResult> LabResults { get; set; } = new();
    }
}
