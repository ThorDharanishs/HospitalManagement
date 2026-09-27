using HospitalManagement.Core.Constant;
using HospitalManagement.Core.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

namespace HospitalManagement.Repository
{
    public class DoctorRepository
    {
        private readonly object _lockDoctor;
        private readonly Dictionary<Spealization, Treatments[]> _treatments;
        private readonly List<Doctor> _doctors;
        private readonly Dictionary<Treatments, TimeSpan> _treatmentsDurations;
        public DoctorRepository()
        {
            this._doctors = this.SetDoctor();
            this._lockDoctor = new object();
            this._treatments = this.SetTreatmentPlan();
            this._treatmentsDurations = this.SetTreatmentDuration();
        }
        public Dictionary<Treatments, TimeSpan> SetTreatmentDuration()
        {
            return new Dictionary<Treatments, TimeSpan>
        {
        { Treatments.Fever, TimeSpan.FromSeconds(15) },
        { Treatments.Cought, TimeSpan.FromSeconds(10) },
        { Treatments.Bone, TimeSpan.FromSeconds(30) },
        { Treatments.LungCancer, TimeSpan.FromSeconds(60) },
        { Treatments.BrainTumor, TimeSpan.FromSeconds(90) },
        { Treatments.Nerve, TimeSpan.FromSeconds(45) },
        { Treatments.Kidney, TimeSpan.FromSeconds(40) },
        { Treatments.Brain, TimeSpan.FromSeconds(50) }
        };
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
            lock(this._lockDoctor)
            {
                return this._doctors;
            }
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
        public Dictionary<Treatments, TimeSpan> GetTreatmentDuration()
        {
            return this._treatmentsDurations;
        }
        public bool TryAssignDoctor(Spealization specialization, out Doctor? doctor)
        {
            lock (this._lockDoctor)
            {
                doctor = this._doctors.FirstOrDefault(
                    d => d.Spealization == specialization &&
                         d.Status == DoctorStatus.Available);

                if (doctor == null)
                {
                    return false;
                }

                doctor.Status = DoctorStatus.Busy;
                return true;
            }
        }
    }
}
