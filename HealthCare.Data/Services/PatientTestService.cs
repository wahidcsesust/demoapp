using HealthCare.Data.Interfaces;
using HealthCare.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HealthCare.Data.Services
{
    public class PatientTestService : GenericRepository<PatientTest>, IPatientTestRepository
    {
        private readonly ApplicationDbContext context;
        public PatientTestService(ApplicationDbContext _context) : base(_context)
        {
            this.context = _context;
        }
        public async Task<PatientTest> GetPatientTestByAppointmentId(long appointmentId)
        {
            var patientTest = await context.PatientTests.FirstOrDefaultAsync(m => m.AppointmentId == appointmentId && !m.IsDeleted);
            return patientTest;
        }
    }
}
