using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Core.Constant
{
    public enum Treatments
    {
        Fever = 1,
        Cought,
        Bone,
        LungCancer,
        BrainTumor,
        Nerve,
        Kidney,
        Brain,
    }
    public enum Spealization
    {
        General = 1,
        Cancer,
        Bone,
        LowerBody,
        Head,
        Nerves,
    }
    public enum TreatmentStatus
    {
        AppointmentBooked = 1,
        Started,
        Completed,
    }
    public enum DoctorStatus
    {
        Available = 1,
        Busy,
    }
}
