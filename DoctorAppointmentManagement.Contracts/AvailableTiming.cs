using System;
using System.ComponentModel.DataAnnotations;

namespace DoctorAppointmentManagement.Contracts
{
    public class AvailableTiming
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Date is required")]
      
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Start Time in Hours is required")]
        [Range(0, 23, ErrorMessage = "Start Time in Hours must be between 0 and 23")]
        public int StartTimeHours { get; set; }

        [Required(ErrorMessage = "Start Time in Minutes is required")]
        [Range(0, 59, ErrorMessage = "Start Time in Minutes must be between 0 and 59")]
        public int StartTimeMins { get; set; }

        [Required(ErrorMessage = "End Time in Hours is required")]
        [Range(0, 23, ErrorMessage = "End Time in Hours must be between 0 and 23")]
        public int EndTimeHours { get; set; }

        [Required(ErrorMessage = "End Time in Minutes is required")]
        [Range(0, 59, ErrorMessage = "End Time in Minutes must be between 0 and 59")]
        public int EndTimeMins { get; set; }

        
        public int DoctorId { get; set; }
    }
}
