using HospitalManagement.Core.Constant;
using HospitalManagement.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Repository
{
    public class DoctorRepository
    {
        private readonly object _lockDoctor;
        private readonly List<Doctor> _doctors;
        public DoctorRepository()
        {
            this._doctors = new List<Doctor>();
            this._lockDoctor = new object();
        }
        public void AddNewDoctor(Doctor doctor)
        {
            this._doctors.Add(doctor);
        }
        public IReadOnlyCollection<Doctor> GetAllDoctor()
        {
            return this._doctors;
        }
        public Doctor? GetDoctorById(Guid id)
        {
            return this._doctors
                .FirstOrDefault(doctor => doctor.Id == id);
        }
        public bool SetDoctorFree(Guid id)
        {
            Doctor? doctor = this.GetDoctorById(id);
            if(doctor == null)
            {
                return false;
            }

            lock(this._lockDoctor)
            {
                doctor.Status = DoctorStatus.Available;
            }
            return true;
        }
        public bool SetDoctorBusy(Guid id)
        {
            Doctor? doctor = this.GetDoctorById(id);
            if (doctor == null)
            {
                return false;
            }

            lock (this._lockDoctor)
            {
                doctor.Status = DoctorStatus.Busy;
            }
            return true;
        }
    }
}
