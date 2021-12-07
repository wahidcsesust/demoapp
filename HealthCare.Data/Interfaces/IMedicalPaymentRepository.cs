using HealthCare.Data.Models;
using HealthCare.Data.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare.Data.Interfaces
{
    public interface IMedicalPaymentRepository : IGenericRepository<MedicalPayment>
    {
        Task<(int totalRecords, List<MedicalPayment>)> UpdateMedicalPayment(long transactionId,
            string transactionType, string paymentType, long patientId, decimal totalAmount,
            decimal paidAmount, decimal dueAmount, decimal lessAmount);
        Task<MedicalPayment> GetMedicalPaymentByTransactionId(long transactionId, string transactionType);
    }
}
