using HealthCare.Data.Models;
using HealthCare.Data.Services;
using System.Threading.Tasks;

namespace HealthCare.Data.Interfaces
{
    public interface IPatientTestRepository : IGenericRepository<PatientTest>
    {
        Task<PatientTest> GetPatientTestByAppointmentId(long appointmentId);
    }
}
