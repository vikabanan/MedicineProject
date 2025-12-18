using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineProject.Models
{
    public class ReferralViewModel
    {
        public int ReferralId { get; set; }

        public string PatientName { get; set; } = "";
        public string ReferralType { get; set; } = "";
        public DateTime ReferralDate { get; set; }
        public string Status { get; set; } = "";
    }
}
