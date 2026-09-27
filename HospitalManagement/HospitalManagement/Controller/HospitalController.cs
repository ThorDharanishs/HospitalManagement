using HospitalManagement.Core.Constant;
using HospitalManagement.Core.Model;
using HospitalManagement.Helper;
using HospitalManagement.Service;
using HospitalManagement.View;

namespace HospitalManagement.Controller
{
    public class HospitalController
    {
        private readonly AppointmentService _appointmentService;
        private readonly NotificationService _notificationService;
        private readonly PatientService _patientService;
        public HospitalController(AppointmentService appointmentService, NotificationService notificationService, PatientService patientService)
        {
            this._appointmentService = appointmentService;
            this._notificationService = notificationService;
            this._patientService = patientService;
            this._notificationService.Notifier += ConsoleActivity.DisplayNotification;
        }
        public void Start()
        {
            _ = _appointmentService.ProcessAppointment();
            MenuItem userChoice;
            do
            {
                ConsoleActivity.ShowMenu("Hospital Management", new[] { "Add new patient", "Add New Appointment", "View All Treatment", "View Dashboard", "Exit" });
                userChoice = (MenuItem)ConsoleActivity.GetIntegerInput("option");
                switch(userChoice)
                {
                    case MenuItem.AddPatient:
                        this.ExecuteAddPatient();
                        break;
                    case MenuItem.AddAppointment:
                        this.AddAppointment();
                        break;
                }
            }
            while (userChoice != MenuItem.Exit);
        }
        public void ExecuteAddPatient()
        {
            ConsoleActivity.ShowHeader("Add New Patient");
            string? name = ConsoleActivity.GetStringInput("name");
            if (!Validator.IsValidName(name))
            {
                ConsoleActivity.PrintAndWait("Invalid patient name!!");
                return;
            }

            ConsoleActivity.PrintInConsole("Choose the treatment plan :");
            ConsoleActivity.DisplayEnums<Treatments>();
            Treatments patientTreatment = (Treatments)ConsoleActivity.GetIntegerInput("option");
            this._patientService.AddNewPatient(name!, patientTreatment);
            ConsoleActivity.PrintAndWait("Patient added successfully");
        }
        public void AddAppointment()
        {
            ConsoleActivity.ShowHeader("Book Appointment");
            List<Patient> patients = this._patientService.GetAllPatient().Result;
            ConsoleActivity.DisplayPatients(patients);

            int choosenPatient = ConsoleActivity.GetIntegerInput("option");
            if (choosenPatient <= 0 || choosenPatient > patients.Count)
            {
                ConsoleActivity.PrintAndWait($"Choose patient with 1 - {patients.Count()}");
                return;
            }

            Guid patientId = patients[choosenPatient - 1].PatientId;
            this._appointmentService.BookAppointment( patientId );
            ConsoleActivity.PrintAndWait("Appointment booked successfully for patient - " + this._patientService.GetPatientName(patientId).Result);
        }
    }
}
