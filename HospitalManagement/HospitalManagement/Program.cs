using HospitalManagement.Controller;
using HospitalManagement.Repository;
using HospitalManagement.Service;
using System.Net.WebSockets;

namespace HospitalManagement
{
    public class Program
    {
        public static void Main()
        {
            PatientRepository patientRepository = new PatientRepository();
            DoctorRepository doctorRepository = new DoctorRepository();
            AppointmentRepository appointmentRepository = new AppointmentRepository();

            NotificationService notificationService = new NotificationService();

            PatientService patientService = new PatientService(patientRepository);
            DoctorService doctorService = new DoctorService(doctorRepository);
            AppointmentService appointmentService = new AppointmentService(appointmentRepository, notificationService, doctorService, patientService);

            HospitalController hospitalController = new HospitalController(appointmentService, notificationService, patientService);
            hospitalController.Start();
        }
    }
}
