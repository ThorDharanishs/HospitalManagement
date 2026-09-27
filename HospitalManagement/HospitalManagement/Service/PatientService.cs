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
            _= this._patientRepository.AddNewPatientAsync(new Patient(name, treatment));
        }
        public async Task<List<Patient>> GetAllPatient()
        {
            return await this._patientRepository.GetAllPatientAsync();
        }
        public async Task<string> GetPatientName(Guid id)
        {
            return this._patientRepository.GetPatientAsync(id).Result!.PatientName ?? "Guest";
        }
        public Treatments GetPatientTreatment(Guid id)
        {
            return this._patientRepository.GetPatientAsync(id).Result!.Treatment;
        }
        public Patient? GetPatient(Guid id)
        {
            return this._patientRepository.GetPatientAsync(id).Result;
        }
    }
}
