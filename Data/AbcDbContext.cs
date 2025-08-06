using ABC.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ABC.Data
{

    public class AbcDbContext : IdentityDbContext
    {
        public AbcDbContext(DbContextOptions<AbcDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var superadminRoleId = "38b8084d-f00c-4dec-a00f-af76dd615fc7";
            var adminRoleId = "c1b16938-89c9-4f4b-bf83-0f62ff92c077";
            var readonlyRoleId = "0EC32696-7FCF-4A1E-AE27-FE859A23BC1E";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id= superadminRoleId,
                    ConcurrencyStamp= superadminRoleId,
                    Name= "SuperAdmin",
                    NormalizedName= "SuperAdmin".ToUpper()
                },
                new IdentityRole
                {
                    Id= adminRoleId,
                    ConcurrencyStamp= adminRoleId,
                    Name= "Admin",
                    NormalizedName= "Admin".ToUpper()
                },
                new IdentityRole
                {
                    Id= readonlyRoleId,
                    ConcurrencyStamp= readonlyRoleId,
                    Name= "ReadOnly",
                    NormalizedName= "ReadOnly".ToUpper()
                }

            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);
            modelBuilder.Entity<MultiSelect>()
            .Property(ms => ms.SelectionType)
            .HasDefaultValue("");

            ////////Country////////////////////////////////////////////
            ///
            modelBuilder.Entity<Country>()
            .Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

            modelBuilder.Entity<Country>()
                .Property(c => c.Currency)
                .HasMaxLength(10)
                .IsRequired();

            modelBuilder.Entity<Country>()
                .Property(c => c.CurrencySymbol)
                .HasMaxLength(5)
                .IsRequired();

            modelBuilder.Entity<Country>()
                .Property(c => c.IsdCode)
                .HasMaxLength(10)
                .IsRequired();

            modelBuilder.Entity<Country>()
                .Property(c => c.ShortCode)
                .HasMaxLength(10)
                .IsRequired();

            //////////////////////Client/////////////////////
            ///
            modelBuilder.Entity<Client>()
            .Property(c => c.Name)
            .HasMaxLength(255)
            .IsRequired();

            modelBuilder.Entity<Client>()
                .Property(c => c.ContactPerson)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Client>()
                .Property(c => c.Address)
                .HasMaxLength(500)
                .IsRequired();

            modelBuilder.Entity<Client>()
                .Property(c => c.Email)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Client>()
                .Property(c => c.C_Variable)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<Client>()
                .Property(c => c.CreatedById)
                .HasMaxLength(450)
                .IsRequired();

            modelBuilder.Entity<Client>()
                .Property(c => c.ClientTypeId)
                .HasDefaultValue(Guid.Empty);

            // Configure foreign keys with cascading delete behavior
            modelBuilder.Entity<Client>()
                .HasOne(c => c.CreatedBy)
                .WithMany()  // Assuming AspNetUser has a collection of Clients if desired
                .HasForeignKey(c => c.CreatedById)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasOne(c => c.ClientType)
                .WithMany()  // Assuming MultiSelect has a collection of Clients if desired
                .HasForeignKey(c => c.ClientTypeId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserInfo>()
            .Property(u => u.FirstName)
            .IsRequired();

            modelBuilder.Entity<UserInfo>()
                .Property(u => u.LastName)
                .IsRequired();

            modelBuilder.Entity<UserInfo>()
                .Property(u => u.AspNetUsersId)
                .IsRequired();

            modelBuilder.Entity<UserInfo>()
                .Property(u => u.ImageUrl)
                .IsRequired();

            // Configure relationship with AspNetUsers if applicable
            modelBuilder.Entity<UserInfo>()
                .HasOne(u => u.AspNetUser)
                .WithMany()  // Assuming AspNetUser does not have a collection of UserInfoes; adjust if needed
                .HasForeignKey(u => u.AspNetUsersId);

            // Survey to ProjectManager relation, restrict cascading delete
            modelBuilder.Entity<ProjectManager>()
                .HasOne(pm => pm.Survey)
                .WithMany(s => s.ProjectManagers)
                .HasForeignKey(pm => pm.SurveyId)
                .OnDelete(DeleteBehavior.Restrict); // Avoid cascade delete

            // Survey to SalesManager relation, restrict cascading delete
            modelBuilder.Entity<SalesManager>()
                .HasOne(sm => sm.Survey)
                .WithMany(s => s.SalesManagers)
                .HasForeignKey(sm => sm.SurveyId)
                .OnDelete(DeleteBehavior.Restrict); // Avoid cascade delete

            //modelBuilder.Entity<ProjectManager>()
            //    .HasOne<Survey>()
            //    .WithMany(s => s.ProjectManagers)
            //    .HasForeignKey(pm => pm.SurveyId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<SalesManager>()
            //    .HasOne<Survey>()
            //    .WithMany(s => s.SalesManagers)
            //    .HasForeignKey(sm => sm.SurveyId)
            //    .OnDelete(DeleteBehavior.Cascade);

        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<MultiSelect> MultiSelects { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<UserInfo> UserInfoes { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designation { get; set; }
        public DbSet<EmailTemplate> EmailTemplate { get; set; }
        public DbSet<MailQueue> MailQueues { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SalesManager> salesManagers { get; set; }
        public DbSet<ProjectManager> projectManagers { get; set; }
        public DbSet<SurveyResponse> surveyResponses { get; set; }
        public DbSet<PartnerSurvey> partnerSurveys { get; set; }
        public DbSet<Currency> currencies { get; set; }
        public DbSet<SurveyPreScreener> SurveyPreScreeners { get; set; }
        public DbSet<SurveyResponsesPreScreener> SurveyResponsesPreScreeners { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
    }


}
