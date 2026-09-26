using HospitalManagement.Core.Constant;
using HospitalManagement.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Repository
{
    public class DoctorRepository
    {
        private readonly object _lockDoctor;
        private readonly Dictionary<Spealization, Treatments[]> _treatments;
        private readonly List<Doctor> _doctors;
        public DoctorRepository()
        {
            this._doctors = this.SetDoctor();
            this._lockDoctor = new object();
            this._treatments = this.SetTreatmentPlan();
        }
        public Dictionary<Spealization, Treatments[]> SetTreatmentPlan()
        {
            return new Dictionary<Spealization, Treatments[]>
        {
        {
            Spealization.General,
            new[]
            {
                Treatments.Fever,
                Treatments.Cought
            }
        },
        {
            Spealization.Cancer,
            new[]
            {
                Treatments.LungCancer,
                Treatments.BrainTumor
            }
        },
        {
            Spealization.Bone,
            new[]
            {
                Treatments.Bone
            }
        },
        {
            Spealization.LowerBody,
            new[]
            {
                Treatments.Kidney
            }
        },
        {
            Spealization.Head,
            new[]
            {
                Treatments.Brain
            }
        },
        {
            Spealization.Nerves,
            new[]
            {
                Treatments.Nerve
            }
        }
        };
        }
        public List<Doctor> SetDoctor()
        {
            return new List<Doctor>()
        {
        new Doctor("Ram", Spealization.General),
        new Doctor("Arun", Spealization.General),

        new Doctor("Kumar", Spealization.Cancer),
        new Doctor("Priya", Spealization.Cancer),

        new Doctor("Vijay", Spealization.Bone),
        new Doctor("Divya", Spealization.Bone),

        new Doctor("Suresh", Spealization.LowerBody),
        new Doctor("Meena", Spealization.LowerBody),

        new Doctor("Ravi", Spealization.Head),
        new Doctor("Anitha", Spealization.Head),

        new Doctor("Karthik", Spealization.Nerves),
        new Doctor("Deepa", Spealization.Nerves)
        };
        }
        public void AddNewDoctor(Doctor doctor)
        {
            this._doctors.Add(doctor);
        }
        public List<Doctor> GetAllDoctor()
        {
            return this._doctors;
        }
        public Doctor? GetDoctorById(Guid id)
        {
            return this._doctors
                .FirstOrDefault(doctor => doctor.Id == id);
        }
        public bool SetDoctorFree(Guid id)
        {
            Doctor? doctor = this.GetDoctorById(id);
            if(doctor == null)
            {
                return false;
            }

            lock(this._lockDoctor)
            {
                doctor.Status = DoctorStatus.Available;
            }
            return true;
        }
        public bool SetDoctorBusy(Guid id)
        {
            Doctor? doctor = this.GetDoctorById(id);
            if (doctor == null)
            {
                return false;
            }

            lock (this._lockDoctor)
            {
                doctor.Status = DoctorStatus.Busy;
            }
            return true;
        }
        public Dictionary<Spealization, Treatments[]> GetTreatmentPlan()
        {
            return this._treatments;
        }
    }
}
