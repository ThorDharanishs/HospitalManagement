using HospitalManagement.Core.Constant;
using HospitalManagement.Core.Model;
using HospitalManagement.FileHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Repository
{
    public class PatientRepository
    {
        private readonly string _patientFile = "patient.csv";
        private readonly CsvFileHandler<Patient> _csvFile;
        private readonly List<Patient> _patients;
        public PatientRepository()
        {
            this._patients = new List<Patient>();
            _csvFile = new CsvFileHandler<Patient>(
                _patientFile,
                PatientToCsv,
                CsvToPatient);
        }
        public void AddNewPatient(Patient patient)
        {
            _csvFile.Write(patient);
        }
        public List<Patient> GetAllPatient()
        {
            return _csvFile.ReadAll();
        }
        public Patient? GetPatient(Guid id)
        {
            return GetAllPatient()
                .FirstOrDefault(x => x.PatientId == id);
        }
        private static string PatientToCsv(Patient patient)
        {
            return string.Join(",",
                patient.PatientId,
                CsvFileHandler<Patient>.Escape(
                    patient.PatientName),
                patient.Treatment);
        }

        private static Patient CsvToPatient(string line)
        {
            List<string> fields = CsvFileHandler<Patient>.Parse(line);
            Treatments treatment;
            Guid id;
            Guid.TryParse(fields[0], out id);
            Enum.TryParse<Treatments>(fields[2], out treatment);
            return new Patient(id, fields[1], treatment);
        }
    }
}
