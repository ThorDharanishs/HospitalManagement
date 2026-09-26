using HospitalManagement.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Repository
{
    public class PatientRepository
    {
        private readonly List<Patient> _patients;
        public PatientRepository()
        {
            this._patients = new List<Patient>();
        }
        public void AddNewPatient(Patient patient)
        {
            this._patients.Add(patient);
        }
        public string? GetPatientName(Guid id)
        {
            return this._patients
                .FirstOrDefault(patient => patient.PatientId == id)
                ?.PatientName;
        }
    }
}
