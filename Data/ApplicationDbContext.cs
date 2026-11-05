using CampusPlacementManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<College> Colleges => Set<College>();
    public DbSet<PlacementOfficer> PlacementOfficers => Set<PlacementOfficer>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<CompanyHR> CompanyHRs => Set<CompanyHR>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<EligibilityCriteria> EligibilityCriteria => Set<EligibilityCriteria>();
    public DbSet<JobTargetCollege> JobTargetColleges => Set<JobTargetCollege>();
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();
    public DbSet<RecruitmentDrive> RecruitmentDrives => Set<RecruitmentDrive>();
    public DbSet<InterviewRound> InterviewRounds => Set<InterviewRound>();
    public DbSet<InterviewSchedule> InterviewSchedules => Set<InterviewSchedule>();
    public DbSet<ApplicationStatusHistory> ApplicationStatusHistories => Set<ApplicationStatusHistory>();
    public DbSet<PlacementStatistic> PlacementStatistics => Set<PlacementStatistic>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(e =>
        {
            e.Property(x => x.FullName).HasMaxLength(200).IsRequired();
            e.Property(x => x.AccountStatus).HasConversion<string>().HasMaxLength(30);
            e.Property(x => x.RefreshTokenHash).HasMaxLength(128);
            e.HasIndex(x => x.Email).IsUnique();
        });

        builder.Entity<College>(e =>
        {
            e.HasKey(x => x.CollegeId);
            e.Property(x => x.CollegeName).HasMaxLength(200).IsRequired();
            e.Property(x => x.CollegeCode).HasMaxLength(50).IsRequired();
            e.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
            e.HasIndex(x => x.CollegeCode).IsUnique();
            e.HasIndex(x => x.OfficialEmail).IsUnique();
        });

        builder.Entity<PlacementOfficer>(e =>
        {
            e.HasKey(x => x.PlacementOfficerId);
            e.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
            e.HasIndex(x => x.UserId).IsUnique();
            e.HasOne(x => x.User).WithOne(x => x.PlacementOfficer).HasForeignKey<PlacementOfficer>(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.College).WithMany(x => x.PlacementOfficers).HasForeignKey(x => x.CollegeId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Approver).WithMany().HasForeignKey(x => x.ApprovedBy).OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Company>(e =>
        {
            e.HasKey(x => x.CompanyId);
            e.Property(x => x.CompanyName).HasMaxLength(200).IsRequired();
            e.Property(x => x.CompanyEmail).HasMaxLength(255).IsRequired();
            e.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
            e.HasIndex(x => x.CompanyEmail).IsUnique();
        });

        builder.Entity<CompanyHR>(e =>
        {
            e.HasKey(x => x.CompanyHRId);
            e.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
            e.HasIndex(x => x.UserId).IsUnique();
            e.HasOne(x => x.User).WithOne(x => x.CompanyHR).HasForeignKey<CompanyHR>(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Company).WithMany(x => x.CompanyHRs).HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Approver).WithMany().HasForeignKey(x => x.ApprovedBy).OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Student>(e =>
        {
            e.HasKey(x => x.StudentId);
            e.Property(x => x.RegisterNumber).HasMaxLength(50).IsRequired();
            e.Property(x => x.CGPA).HasPrecision(4, 2);
            e.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
            e.HasIndex(x => new { x.CollegeId, x.RegisterNumber }).IsUnique();
            e.HasIndex(x => x.UserId).IsUnique();
            e.HasOne(x => x.User).WithOne(x => x.Student).HasForeignKey<Student>(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.College).WithMany(x => x.Students).HasForeignKey(x => x.CollegeId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.PlacementOfficer).WithMany(x => x.Students).HasForeignKey(x => x.PlacementOfficerId).OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Job>(e =>
        {
            e.ToTable("JobOpenings");
            e.HasKey(x => x.JobId);
            e.Property(x => x.SalaryPackage).HasPrecision(12, 2);
            e.Property(x => x.MinimumCGPA).HasPrecision(4, 2);
            e.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
            e.HasOne(x => x.Company).WithMany(x => x.Jobs).HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.CreatedByHR).WithMany(x => x.Jobs).HasForeignKey(x => x.CreatedByHRId).OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(x => new { x.CompanyId, x.Status });
        });

        builder.Entity<EligibilityCriteria>(e =>
        {
            e.HasKey(x => x.EligibilityCriteriaId);
            e.Property(x => x.MinimumCGPA).HasPrecision(4, 2);
            e.HasOne(x => x.Job).WithMany(x => x.EligibilityCriteria).HasForeignKey(x => x.JobId).OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<JobTargetCollege>(e =>
        {
            e.HasKey(x => x.JobTargetCollegeId);
            e.HasIndex(x => new { x.JobId, x.CollegeId }).IsUnique();
            e.HasOne(x => x.Job).WithMany(x => x.TargetColleges).HasForeignKey(x => x.JobId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.College).WithMany(x => x.JobTargetColleges).HasForeignKey(x => x.CollegeId).OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<JobApplication>(e =>
        {
            e.HasKey(x => x.ApplicationId);
            e.Property(x => x.Status).HasConversion<string>().HasMaxLength(40);
            e.HasIndex(x => new { x.StudentId, x.JobId }).IsUnique();
            e.HasOne(x => x.Job).WithMany(x => x.Applications).HasForeignKey(x => x.JobId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Student).WithMany(x => x.Applications).HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<RecruitmentDrive>(e =>
        {
            e.HasKey(x => x.DriveId);
            e.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
            e.HasOne(x => x.Job).WithMany(x => x.RecruitmentDrives).HasForeignKey(x => x.JobId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.College).WithMany(x => x.RecruitmentDrives).HasForeignKey(x => x.CollegeId).OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<InterviewRound>(e =>
        {
            e.HasKey(x => x.InterviewRoundId);
            e.HasOne(x => x.Job).WithMany(x => x.InterviewRounds).HasForeignKey(x => x.JobId).OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(x => new { x.JobId, x.RoundNumber }).IsUnique();
        });

        builder.Entity<InterviewSchedule>(e =>
        {
            e.HasKey(x => x.InterviewScheduleId);
            e.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
            e.HasOne(x => x.Application).WithMany(x => x.InterviewSchedules).HasForeignKey(x => x.ApplicationId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.InterviewRound).WithMany(x => x.Schedules).HasForeignKey(x => x.InterviewRoundId).OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<ApplicationStatusHistory>(e =>
        {
            e.HasKey(x => x.HistoryId);
            e.Property(x => x.PreviousStatus).HasConversion<string>().HasMaxLength(40);
            e.Property(x => x.NewStatus).HasConversion<string>().HasMaxLength(40);
            e.HasOne(x => x.Application).WithMany(x => x.StatusHistory).HasForeignKey(x => x.ApplicationId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.UpdatedByUser).WithMany().HasForeignKey(x => x.UpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<PlacementStatistic>(e =>
        {
            e.HasKey(x => x.PlacementStatisticId);
            e.Property(x => x.PlacementPercentage).HasPrecision(5, 2);
            e.HasOne(x => x.College).WithMany().HasForeignKey(x => x.CollegeId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
