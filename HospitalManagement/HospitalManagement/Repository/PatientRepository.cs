using HospitalManagement.Core.Constant;
using HospitalManagement.Core.Model;
using HospitalManagement.FileHelper;

namespace HospitalManagement.Repository
{
    public class PatientRepository
    {
        private readonly string _patientFile = "patient.csv";
        private readonly CsvFileHandler<Patient> _csvFile;

        public PatientRepository()
        {
            _csvFile = new CsvFileHandler<Patient>(
                _patientFile,
                PatientToCsv,
                CsvToPatient);
        }

        public async Task AddNewPatientAsync(Patient patient, CancellationToken cancellationToken = default)
        {
            await _csvFile.WriteAsync(patient, cancellationToken);
        }

        public async Task<List<Patient>> GetAllPatientAsync(CancellationToken cancellationToken = default)
        {
            return await _csvFile.ReadAllAsync(cancellationToken);
        }

        public async Task<Patient?> GetPatientAsync(Guid id, CancellationToken cancellationToken = default)
        {
            List<Patient> patients =
                await GetAllPatientAsync(cancellationToken);

            return patients.FirstOrDefault(
                patient => patient.PatientId == id);
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
            List<string> fields =
                CsvFileHandler<Patient>.Parse(line);

            if (!Guid.TryParse(fields[0], out Guid id))
            {
                throw new InvalidDataException("Invalid patient ID in CSV file.");
            }

            if (!Enum.TryParse<Treatments>(
                    fields[2],
                    true,
                    out Treatments treatment))
            {
                throw new InvalidDataException("Invalid treatment in CSV file.");
            }

            return new Patient(
                id,
                fields[1],
                treatment);
        }
    }
}