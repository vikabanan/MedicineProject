using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineProject.Models
{
    public class LabResult
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public string Conclusion { get; set; } = null!;
        public DateTime ResultDate { get; set; } = DateTime.Now;

        public int ReferralId { get; set; }
        public Referral Referral { get; set; } = null!;
    }
}
