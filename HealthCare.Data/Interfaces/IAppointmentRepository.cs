using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Data.Models;
using HealthCare.Data.Services;

namespace HealthCare.Data.Interfaces
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetAppointmentByDoctorId(long doctorId);

        Task<IEnumerable<Appointment>> GetAppointmentByDoctorId(long doctorId, DateTime? dateOfAppointment);
    }
}
