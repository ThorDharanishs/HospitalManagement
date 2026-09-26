using HospitalManagement.Core.Constant;
using HospitalManagement.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Service
{
    public class DoctorService
    {
        private readonly object _lock;
        private readonly DoctorRepository _doctorRepository;
        public DoctorService(DoctorRepository doctorRepository)
        {
            this._doctorRepository = doctorRepository;
            this._lock = new object();
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
        public Guid GetAvailableDoctor(Treatments treatment)
        {
            Spealization spealizationRequired = this.GetSpealization(treatment);
            lock(this._lock)
            {
                return this._doctorRepository.GetAllDoctor()
                    .FirstOrDefault(doctor => doctor.Status == DoctorStatus.Available && doctor.Spealization == spealizationRequired)
                    ?.Id ?? default;
            }
        }

    }
}
