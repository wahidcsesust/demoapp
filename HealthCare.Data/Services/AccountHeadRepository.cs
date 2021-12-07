using Microsoft.EntityFrameworkCore;
using HealthCare.Data.Interfaces;
using HealthCare.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Data.Services
{
    public class AccountHeadRepository : GenericRepository<AccountHead>, IAccountHeadRepository
    {
        private readonly ApplicationDbContext context;
        public AccountHeadRepository(ApplicationDbContext _context) : base(_context)
        {
            this.context = _context;
        }
        public string GetNextAccountNo()
        {
            var model = context.AccountHeads.OrderByDescending(u => u.Id).FirstOrDefaultAsync().Result;
            long maxId = 0;
            if (model == null)
                maxId = +1;
            else
            {
                maxId = model.Id + 1;
            }

            return maxId.ToString().PadLeft(4, '0');
        }
        public async Task<(int totalRecords, List<AccountHead>)> UpdateAccountHeadCurrentBalance(string transactionType, decimal amount)
        {
            List<AccountHead> modelList = new List<AccountHead>();
            int totalRecords = 0;

            try
            {
                var paramTransactionType = new SqlParameter
                {
                    ParameterName = "TransactionType",
                    DbType = DbType.String,
                    Direction = ParameterDirection.Input,
                    Value = transactionType
                };
                var paramAmount = new SqlParameter
                {
                    ParameterName = "Amount",
                    DbType = DbType.Decimal,
                    Direction = ParameterDirection.Input,
                    Value = amount
                };

                //var paramOut = new SqlParameter
                //{
                //    ParameterName = "RecordCount",
                //    DbType = DbType.Int32,
                //    Direction = ParameterDirection.Output
                //};

                var sql = "Exec Accounting_Update_UpdateAccountCurrentBalance @TransactionType, @Amount";
                var result = await context.Database.ExecuteSqlCommandAsync(sql, paramTransactionType, paramAmount);

                //totalRecords = (int)paramOut.Value;


                //var result = db.Database.ExecuteSqlCommand(sql, in1, in2, out1, out2);

                //var out1Value = (long)out1.Value;
                //var out2Value = (string)out2.Value;
                //SqlParameter[] storeParams = new SqlParameter[3];


                //if (!string.IsNullOrEmpty(accountNo))
                //    storeParams[0] = new SqlParameter("AccountId", accountNo);
                //else
                //    storeParams[0] = new SqlParameter("AccountId", DBNull.Value);

                //storeParams[1] = new SqlParameter("Amount", amount);

                //storeParams[2] = new SqlParameter
                //{
                //    ParameterName = "RecordCount",
                //    Value = 0,
                //    Direction = ParameterDirection.Output
                //};

                //var r = context.Query<AccountHead>().FromSql("EXEC Accounting_Update_UpdateAccountCurrentBalance @AccountId, @Amount, @RecordCount OUT", storeParams);
                //modelList = await r.ToListAsync();

                //totalRecords = (int)storeParams[2].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return (totalRecords, modelList);
        }
        public async Task<(int totalRecords, List<AccountHead>)> DeleteAccountHeadCurrentBalance(string transactionType, decimal amount)
        {
            List<AccountHead> modelList = new List<AccountHead>();
            int totalRecords = 0;

            try
            {
                var paramTransactionType = new SqlParameter
                {
                    ParameterName = "TransactionType",
                    DbType = DbType.String,
                    Direction = ParameterDirection.Input,
                    Value = transactionType
                };
                var paramAmount = new SqlParameter
                {
                    ParameterName = "Amount",
                    DbType = DbType.Decimal,
                    Direction = ParameterDirection.Input,
                    Value = amount
                };

                var sql = "Exec Accounting_Delete_UpdateAccountCurrentBalance @TransactionType, @Amount";
                var result = await context.Database.ExecuteSqlCommandAsync(sql, paramTransactionType, paramAmount);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return (totalRecords, modelList);
        }
    }
}
