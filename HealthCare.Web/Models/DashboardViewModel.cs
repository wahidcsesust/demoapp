namespace HealthCare.Web.Models
{
    public class DashboardViewModel
    {
        public long NumberOfUser { get; set; }
        public long NumberOfDoctor { get; set; }
        public long NumberOfPatient { get; set; }
        public long NumberOfDiagonosis { get; set; }
        public long NumberOfAppointment { get; set; }
        public long NumberOfProduct { get; set; }
        public long NumberOfAdvisor { get; set; }
        public long NumberOfExecutive { get; set; }
        public long NumberOfMember { get; set; }
        public long NumberOfHelp { get; set; }
        public long NumberOfTotalMember { get; set; }
        public long NumberOfAdmission { get; set; }

        //Asset
        public decimal PrettyCash { get; set; }
        public decimal LoanPrincipleReceivable { get; set; }
        public decimal LoanInterestReceivable { get; set; }
        public decimal SecurityAdvanceReceivable { get; set; }
        public decimal MedicalPaymentReceivable { get; set; }
        //Liability
        public decimal SanchayPrincipalPayable { get; set; }
        public decimal OwnersEquity { get; set; }
        public decimal BimaPayable { get; set; }
        public decimal SanchayInterestPayable { get; set; }
        //Income
        public decimal LoanPrincipalReceivableIncome { get; set; }
        public decimal LoanInterestReceivableIncome { get; set; }
        public decimal SanchayIncome { get; set; }
        public decimal OwnersEquityIncome { get; set; }
        public decimal MemberBookIncome { get; set; }
        public decimal BimaIncome { get; set; }
        public decimal OtherIncome { get; set; }

        public decimal DoctorVisitIncome { get; set; }
        public decimal MedicalTestIncome { get; set; }
        public decimal AdmissionIncome { get; set; }
        public decimal PharmacyIncome { get; set; }
        //Expense
        public decimal UtilityExpense { get; set; }
        public decimal LessAmountExpense { get; set; }
        public decimal SanchayPrincipalCashback { get; set; }
        public decimal SanchayInterestCashback { get; set; }
        public decimal LoanDisburse { get; set; }


        public long NumberOfMainbody { get; set; }
        public decimal SanchayCollection { get; set; }
        public decimal PreviousMonthCollection { get; set; }
        public decimal CurrentMonthCollection { get; set; }

        public decimal TodayCollection { get; set; }
        public decimal TodaySanchayCollection { get; set; }
        public decimal TodayMainBodyCollection { get; set; }
        public decimal TodayMiscellaneousCollection { get; set; }
        public decimal TodayExpenseCollection { get; set; }
        public decimal TodayCashBackCollection { get; set; }
        public decimal TodayCashBackIntCollection { get; set; }
        public decimal TodayLoanCollection { get; set; }

        public decimal MainbodyCollection { get; set; }
        public decimal MiscellaneousCollection { get; set; }
        public decimal ExpenseCollection { get; set; }
        public decimal SecurityAdvanceCollection { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal TotalCashBack { get; set; }
        public long NumberOfLoan { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal LoanCollection { get; set; }
        public decimal LoanTotalBaseCollection { get; set; }
        public decimal LoanTotalInterestCollection { get; set; }
        public decimal LoanBalanceAmount { get; set; }
    }
}
