using HospitalManagement.Core.Constant;

namespace HospitalManagement.Core.Model
{
    public class Doctor
    {
        public Doctor(string name, Spealization spealization)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Spealization = spealization;
            this.Status = DoctorStatus.Available;
        }
        public Guid Id { get; init; }
        public string Name { get; set; }
        public Spealization Spealization { get; set; }
        public DoctorStatus Status { get; set; }
    }
}
