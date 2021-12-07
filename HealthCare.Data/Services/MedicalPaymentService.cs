using HealthCare.Data.Interfaces;
using HealthCare.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace HealthCare.Data.Services
{
    public class MedicalPaymentService : GenericRepository<MedicalPayment>, IMedicalPaymentRepository
    {
        private readonly ApplicationDbContext context;
        public MedicalPaymentService(ApplicationDbContext _context) : base(_context)
        {
            this.context = _context;
        }
        public async Task<MedicalPayment> GetMedicalPaymentByTransactionId(long transactionId, string transactionType)
        {
            var medicalPayment = await context.MedicalPayments.FirstOrDefaultAsync(m => m.TransactionId == transactionId && m.TransactionType.Equals(transactionType) && !m.IsDeleted);
            return medicalPayment;
        }
        public async Task<(int totalRecords, List<MedicalPayment>)> UpdateMedicalPayment(
            long transactionId,
            string transactionType, string paymentType, long patientId, decimal totalAmount,
            decimal paidAmount, decimal dueAmount, decimal lessAmount)
        {
            List<MedicalPayment> modelList = new List<MedicalPayment>();
            int totalRecords = 0;

            try
            {
                var paramTransactionId = new SqlParameter
                {
                    ParameterName = "TransactionId", DbType = DbType.Int64, Direction = ParameterDirection.Input,
                    Value = transactionId
                };

                var paramTransactionType = new SqlParameter
                {
                    ParameterName = "TransactionType", DbType = DbType.String, Direction = ParameterDirection.Input,
                    Value = transactionType
                };
                var paramPaymentType = new SqlParameter
                {
                    ParameterName = "PaymentType",
                    DbType = DbType.String,
                    Direction = ParameterDirection.Input,
                    Value = paymentType
                };
                var paramPatientId = new SqlParameter
                {
                    ParameterName = "PatientId",
                    DbType = DbType.Int64,
                    Direction = ParameterDirection.Input,
                    Value = patientId
                };
                var paramTotalAmount = new SqlParameter
                {
                    ParameterName = "TotalAmount",
                    DbType = DbType.Decimal,
                    Direction = ParameterDirection.Input,
                    Value = totalAmount
                };
                var paramPaidAmount = new SqlParameter
                {
                    ParameterName = "PaidAmount",
                    DbType = DbType.Decimal,
                    Direction = ParameterDirection.Input,
                    Value = paidAmount
                };
                var paramDueAmount = new SqlParameter
                {
                    ParameterName = "DueAmount",
                    DbType = DbType.Decimal,
                    Direction = ParameterDirection.Input,
                    Value = dueAmount
                };
                var paramLessAmount = new SqlParameter
                {
                    ParameterName = "LessAmount",
                    DbType = DbType.Decimal,
                    Direction = ParameterDirection.Input,
                    Value = lessAmount
                };

                var sql = "Exec Sp_Update_MedicalPayment @TransactionId, @TransactionType, @PaymentType, @PatientId, @TotalAmount, @PaidAmount, @DueAmount, @LessAmount";
                var result = await context.Database.ExecuteSqlCommandAsync(sql, paramTransactionId, paramTransactionType, paramPaymentType,
                    paramPatientId, paramTotalAmount, paramPaidAmount, paramDueAmount, paramLessAmount);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return (totalRecords, modelList);
        }
    }
}
