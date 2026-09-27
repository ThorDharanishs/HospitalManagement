using HospitalManagement.Core.Constant;
using HospitalManagement.Core.Model;
using HospitalManagement.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Service
{
    public class DoctorService
    {
        private readonly DoctorRepository _doctorRepository;
        public DoctorService(DoctorRepository doctorRepository)
        {
            this._doctorRepository = doctorRepository;
        }
        public string GetDoctorName(Guid id)
        {
            return this._doctorRepository.GetDoctorById(id)
                ?.Name ?? "Unknown";
        }
        public void SetDoctorBusy(Guid id)
        {
            this._doctorRepository.SetDoctorBusy(id);
        }
        public void SetDoctorFree(Guid id)
        {
            this._doctorRepository.SetDoctorFree(id);
        }
        public Spealization GetSpealization(Treatments treatment)
        {
            return this._doctorRepository.GetTreatmentPlan()
                .FirstOrDefault(plan => plan.Value.Contains(treatment))
                .Key;
        }
        public Doctor? GetAvailableDoctor(Treatments treatment)
        {
            Spealization specialization = GetSpealization(treatment);

            bool assigned = _doctorRepository.TryAssignDoctor(
                    specialization,
                    out Doctor? doctor);

            return assigned ? doctor : null;
        }
        public TimeSpan GetTreatmentDuration(Treatments treatment)
        {
            return this._doctorRepository.GetTreatmentDuration()
                .FirstOrDefault(time => time.Key == treatment)
                .Value;
        }
    }
}
