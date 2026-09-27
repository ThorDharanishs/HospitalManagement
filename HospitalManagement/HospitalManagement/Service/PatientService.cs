using HospitalManagement.Core.Constant;
using HospitalManagement.Core.Model;
using HospitalManagement.Repository;

namespace HospitalManagement.Service
{
    public class PatientService
    {
        private readonly PatientRepository _patientRepository;
        public PatientService(PatientRepository patientRepository)
        {
            this._patientRepository = patientRepository;
        }
        public void AddNewPatient(string name, Treatments treatment)
        {
            this._patientRepository.AddNewPatient(new Patient(name, treatment));
        }
        public List<Patient> GetAllPatient()
        {
            return this._patientRepository.GetAllPatient();
        }
        public string GetPatientName(Guid id)
        {
            return this._patientRepository.GetPatient(id)?.PatientName ?? "Guest";
        }
        public Treatments GetPatientTreatment(Guid id)
        {
            return this._patientRepository.GetPatient(id)!.Treatment;
        }
        public Patient? GetPatient(Guid id)
        {
            return this._patientRepository.GetPatient(id);
        }
    }
}
