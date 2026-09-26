using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Service
{
    public class NotificationService
    {
        public delegate void Notification(string message, string patientName);
        public event Notification? Notifier;
        public void Execute(string message, string patientName)
        {
            Notifier?.Invoke(message, patientName);
        }
    }
}
