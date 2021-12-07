using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HealthCare.Data.Models;

namespace HealthCare.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<HelpCollection> HelpCollections { get; set; }
        public DbSet<MiscellaneousCollection> MiscellaneousCollections { get; set; }
        public DbSet<CashBackCollection> CashBack { get; set; }
        public DbSet<MiscellaneousType> MiscellaneousType { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<PharmacyIncome> PharmacyIncomes { get; set; }
        public DbSet<SecurityAdvance> SecurityAdvances { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<CollectionHistory> CollectionHistory { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanCollection> LoanCollections { get; set; }
        public DbSet<AccountHeadType> AccountHeadTypes { get; set; }
        public DbSet<AccountHead> AccountHeads { get; set; }
        public DbSet<AccountHeadHistory> AccountHeadHistory { get; set; }
        public DbSet<FiscalYear> FiscalYears { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionDetail> TransactionDetails { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        
        public DbSet<Ward> Wards { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<PatientAdmission> PatientAdmissions { get; set; }

        public DbSet<TestCategory> TestCategories { get; set; }
        public DbSet<Diagnosis> Diagnosis { get; set; }
        public DbSet<MedicalTest> MedicalTests { get; set; }

        public DbSet<PatientTest> PatientTests { get; set; }
        public DbSet<PatientTestDetail> PatientTestDetails { get; set; }
        public DbSet<MedicalPayment> MedicalPayments { get; set; }
        public DbSet<MedicalPaymentDetail> MedicalPaymentDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Loan>().Property(l => l.InterestRate).HasColumnType("decimal(5,1)");

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users").HasKey(x => x.Id);

            builder.Entity<Role>(x => {
                x.ToTable("Roles");
                x.HasKey(y => y.Id);
            });
        }
    }
}
