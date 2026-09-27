using HospitalManagement.Core.Constant;

namespace HospitalManagement.Core.Model
{
    public class Appointment
    {
        public Appointment(Guid patientId, TimeSpan treatmentTime)
        {
            this.PatientId = patientId;
            this.TreatmentTime = treatmentTime;
            this.CreatedDate = DateTime.Now;
            this.TreatmentStatus = TreatmentStatus.AppointmentBooked;
        }
        public Guid Id { get; } = Guid.NewGuid();
        public Guid PatientId {  get; set; }
        public Guid DoctorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public TimeSpan TreatmentTime { get; set; }
        public DateTime TreatmentStartTime { get; set; }
        public DateTime TreatmentCompletedTime { get; set; }
        public TreatmentStatus TreatmentStatus { get; set; }
    }
}
