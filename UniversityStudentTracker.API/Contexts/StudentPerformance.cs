using Microsoft.EntityFrameworkCore;
using UniversityStudentTracker.API.Models.Domains;

namespace UniversityStudentTracker.API.Contexts;

public class StudentPerformance : DbContext
{
    public StudentPerformance(DbContextOptions<StudentPerformance> dbContextOptions) : base(dbContextOptions)
    {
    }

    public DbSet<StudySession> StudySessions { get; set; }
    public DbSet<Break> Breaks { get; set; }
    public DbSet<Prediction> Predictions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<StudySession>().HasKey(s => s.StudySessionID);
        modelBuilder.Entity<Break>().HasKey(b => b.BreakID);
        modelBuilder.Entity<Prediction>().HasKey(p => p.PredictionID);

        modelBuilder.Entity<StudySession>()
            .Property(s => s.StudySessionID)
            .HasDefaultValueSql("NEWID()");

        modelBuilder.Entity<Break>()
            .Property(b => b.BreakID)
            .HasDefaultValueSql("NEWID()");

        modelBuilder.Entity<Prediction>()
            .Property(p => p.PredictionID)
            .HasDefaultValueSql("NEWID()");
        
    }
}