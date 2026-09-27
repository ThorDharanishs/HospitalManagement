using HospitalManagement.Core.Constant;
using HospitalManagement.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Repository
{
    public class AppointmentRepository
    {
        private readonly object _lock;
        private readonly List<Appointment> _appointments;
        public AppointmentRepository()
        {
            this._appointments = new List<Appointment>();
            this._lock = new object();
        }
        public void AddNewAppointment(Appointment appointment)
        {
            lock(_lock)
            {
                this._appointments.Add(appointment);
            }
        }
        public List<Appointment> GetAppointments()
        {
            lock(this._lock)
            {
                return this._appointments;
            }
        }
        public Appointment? GetAppointmentById(Guid id)
        {
            return this._appointments
                .FirstOrDefault(appointment => appointment.Id == id);
        }
        public bool SetAppointmentStatus(Guid id, TreatmentStatus status)
        {
            Appointment? appointment = this.GetAppointmentById(id);
            if (appointment == null)
            {
                return false;
            }

            lock(this._lock)
            {
                appointment.TreatmentStatus = status;
                return true;
            }
        }
        public bool SetTreatmentStartTime(Guid id, DateTime time)
        {
            Appointment? appointment = this.GetAppointmentById(id);
            if (appointment == null)
            {
                return false;
            }

            lock (this._lock)
            {
                appointment.TreatmentStartTime = time;
                return true;
            }
        }
        public bool SetTreatmentEndTime(Guid id, DateTime time)
        {
            Appointment? appointment = this.GetAppointmentById(id);
            if (appointment == null)
            {
                return false;
            }

            lock (this._lock)
            {
                appointment.TreatmentCompletedTime = time;
                return true;
            }
        }
    }
}
