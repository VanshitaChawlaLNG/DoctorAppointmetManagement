using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorAppointmentManagement.Contracts
{
    public class TimingSlots
    {
        [Key]
    public int Id { get; set; }

    [ForeignKey("Doctor")]
    public int DoctorId { get; set; }

    [Required(ErrorMessage = "Doctor is required")]
    public Doctor Doctor { get; set; }

    [Required(ErrorMessage = "Date is required")]
  
    public DateTime Date { get; set; }

    /*[Required(ErrorMessage = "Slots are required")]*/
   /* public List<TimeSlot> Slots { get; set; }*/
}

    public class Slots
    {
        public int Id { get; set; } 
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
       
        public int TimingSlotsId { get; set; }

        
    }
}
