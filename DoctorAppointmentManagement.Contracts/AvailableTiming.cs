using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorAppointmentManagement.Contracts
{
    public class AvailableTiming
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int StartTimeHours { get; set; }

        [Required]
        public int StartTimeMins { get; set; }

        [Required]
        public int EndTimeHours { get; set; }

        [Required]
        public int EndTimeMins { get; set; }

       public int DoctorId {  get; set; }

    }
}