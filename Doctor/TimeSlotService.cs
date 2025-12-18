using MedicineProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineProject.Doctor
{
    public class TimeSlotService
    {
        private readonly MedDbContext _db;

        public TimeSlotService(MedDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Возвращает доступные слоты для визита.
        /// </summary>
        public List<string> GetAvailableVisitTimes(DateTime date)
        {
            var startTime = new TimeSpan(9, 0, 0);
            var endTime = new TimeSpan(17, 0, 0);
            var interval = TimeSpan.FromMinutes(30);

            var allSlots = new List<TimeSpan>();
            for (var t = startTime; t < endTime; t += interval)
                allSlots.Add(t);

            var occupiedSlots = _db.Visits
                .Where(v => v.VisitDate.Date == date.Date)
                .Select(v => v.VisitDate.TimeOfDay)
                .ToList();

            return allSlots
                .Where(slot => !occupiedSlots.Contains(slot))
                .Select(ts => ts.ToString(@"hh\:mm"))
                .ToList();
        }

        /// <summary>
        /// Возвращает доступные слоты для направления по типу и дате.
        /// </summary>
        public List<string> GetAvailableReferralTimes(DateTime date, string referralType)
        {
            var startTime = new TimeSpan(9, 0, 0);
            var endTime = new TimeSpan(17, 0, 0);
            var interval = TimeSpan.FromMinutes(30);

            var allSlots = new List<TimeSpan>();
            for (var t = startTime; t < endTime; t += interval)
                allSlots.Add(t);

            var occupiedSlots = _db.Referrals
                .Where(r => r.ReferralDate.Date == date.Date && r.ReferralType == referralType)
                .Select(r => r.ReferralDate.TimeOfDay)
                .ToList();

            return allSlots
                .Where(slot => !occupiedSlots.Contains(slot))
                .Select(ts => ts.ToString(@"hh\:mm"))
                .ToList();
        }
    }
}
