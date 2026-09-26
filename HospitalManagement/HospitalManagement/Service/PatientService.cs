using HospitalManagement.Core.Constant;
using HospitalManagement.Core.Model;
using HospitalManagement.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
        public string GetPatientName(Guid id)
        {
            return this._patientRepository.GetPatientName(id) ?? "Guest";
        }
    }
}
