using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthCare.Data.Interfaces;
using HealthCare.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Data.Services
{
    public class AppointmentService : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly ApplicationDbContext context;
        public AppointmentService(ApplicationDbContext _context) : base(_context)
        {
            this.context = _context;
        }
        public async Task<IEnumerable<Appointment>> GetAppointmentByDoctorId(long doctorId)
        {
            var appointments = await context.Appointments.ToListAsync();

            var result = doctorId == 0 ? appointments : appointments.FindAll(a => a.DoctorId == doctorId);

            return result;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentByDoctorId(long doctorId, DateTime? dateOfAppointment)
        {
            var appointments = await context.Appointments.ToListAsync();

            var result = appointments.FindAll(a => a.DoctorId == doctorId && a.AppointmentDate.Value == dateOfAppointment);

            return result;
        }
    }
}
