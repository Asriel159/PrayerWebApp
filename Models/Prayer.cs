using System.ComponentModel.DataAnnotations;

namespace PrayerWebApp.Models
{
    public class Prayer
    {
        public int ID { get; set; }

        [Display(Name = "Prayer Description")]
        public string PrayerDescription { get; set; }

        [Display(Name = "Prayer More Details")]
        public string PrayerMoreDetails { get; set; }

        public Prayer() {

        }
    }
}
