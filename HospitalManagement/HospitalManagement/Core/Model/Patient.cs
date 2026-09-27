using HospitalManagement.Core.Constant;

namespace HospitalManagement.Core.Model
{
    public class Patient
    {
        public Patient(Guid id, string name, Treatments treatment)
        {
            this.PatientId = id;
            this.PatientName = name;
            this.Treatment = treatment;
        }
        public Patient(string name, Treatments treatment)
        {
            this.PatientId = Guid.NewGuid();
            this.PatientName = name;
            this.Treatment = treatment;
        }
        public Guid PatientId { get; init; }
        public string PatientName { get; set; }
        public Treatments Treatment { get; set; }
    }
}
