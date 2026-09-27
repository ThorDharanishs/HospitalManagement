using HospitalManagement.Core.Constant;
using HospitalManagement.Core.Model;
using HospitalManagement.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace HospitalManagement.Service
{
    public class AppointmentService
    {
        private readonly AppointmentRepository _appointmentRepository;
        private readonly NotificationService _notificationService;
        private readonly DoctorService _doctorService;
        private readonly PatientService _patientService;
        private readonly Channel<Appointment> _channel;
        public AppointmentService(AppointmentRepository appointmentRepository, NotificationService notificationService, DoctorService doctorService, PatientService patientService)
        {
            this._patientService = patientService;
            this._doctorService = doctorService;
            this._notificationService = notificationService;
            this._appointmentRepository = appointmentRepository;
            this._channel = Channel.CreateUnbounded<Appointment>();
        }
        public void BookAppointment(Guid patientId)
        {
            TimeSpan treatmentTime = this._doctorService.GetTreatmentDuration(this._patientService.GetPatientTreatment(patientId));
            Appointment newAppointment = new Appointment(patientId, treatmentTime);
            this._appointmentRepository.AddNewAppointment(newAppointment);
            this._channel.Writer.TryWrite(newAppointment);
            this._notificationService.Execute("Appointment booked", this._patientService.GetPatientName(patientId));
        }
        public async Task ProcessAppointment()
        {
            await foreach (Appointment appointment in
                           this._channel.Reader.ReadAllAsync())
            {
                _ = ProcessSingleAppointment(appointment);
            }
        }

            private async Task ProcessSingleAppointment(
            Appointment appointment)
        {
            Patient? patient = _patientService.GetPatient(appointment.PatientId);

            if (patient == null)
            {
                this._notificationService.Execute("Appointment booked", "Guest");
                return;
            }

            Doctor? doctor =
                _doctorService.GetAvailableDoctor(patient.Treatment);

            if (doctor == null)
            {
                await Task.Delay(1000);
                _channel.Writer.TryWrite(appointment);
                return;
            }

            _appointmentRepository.SetAppointmentStatus(appointment.Id,TreatmentStatus.Started);
            _appointmentRepository.SetTreatmentStartTime(appointment.Id,DateTime.Now);
            _notificationService.Execute("Treatment started",patient.PatientName);

            await Task.Delay(appointment.TreatmentTime);

            _appointmentRepository.SetTreatmentEndTime(appointment.Id,DateTime.Now);
            _appointmentRepository.SetAppointmentStatus(appointment.Id,TreatmentStatus.Completed);
            _doctorService.SetDoctorFree(doctor.Id);
            _notificationService.Execute("Treatment completed",patient.PatientName);
        }
    }
}
