using DiplomService.Models;
using DiplomService.Models.EventsFolder.Division;
using DiplomService.Models.OrganizationFolder;
using DiplomService.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiplomService.Database
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        //readonly StreamWriter operationLogStream = new StreamWriter("logs/operationLog.txt", true);
        //readonly StreamWriter errorLogStream = new StreamWriter("logs/errorLog.txt", true);

        public DbSet<EventApplication> EventApplications { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Measure> Measures { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationApplication> OrganizationApplications { get; set; }
        public DbSet<News> News { get; set; }

        public DbSet<MobileUser> MobileUsers { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<WebUser> WebUsers { get; set; }
        public DbSet<OrganizationUsers> OrganizationUsers { get; set; }
        public DbSet<MeasureDivisionsInfo> MeasureDivisionsInfos { get; set; }


        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=192.168.1.6;Database=EventDB;User=diplomUser;Password=12332155;Encrypt=false;trusted_connection=false")
                .UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ChatConfiguration());

            modelBuilder.Entity<Event>()
                  .HasMany(c => c.Organizations)
                  .WithMany(s => s.Events)
                  .UsingEntity(j => j.ToTable("EventOrganization"));
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => l.UserId); // Устанавливаем UserId как первичный ключ 


            modelBuilder.Entity<Measure>()
                .HasOne(m => m.Event)
                .WithMany(e => e.Measures)
                .HasForeignKey(m => m.EventId);

            modelBuilder.Entity<Division>()
                .HasOne(d => d.Event)
                .WithMany(e => e.Divisions)
                .HasForeignKey(d => d.EventId);

            modelBuilder.Entity<MeasureDivisionsInfo>()
                    .HasOne(mdi => mdi.Division)
                    .WithMany(d => d.MeasureDivisionsInfos)
                    .HasForeignKey(mdi => mdi.DivisionId)
                    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MeasureDivisionsInfo>()
                .HasOne(mdi => mdi.Measure)
                .WithMany(m => m.MeasureDivisionsInfos)
                .HasForeignKey(mdi => mdi.MeasureId)
                .OnDelete(DeleteBehavior.NoAction);
        }


        public class ChatConfiguration : IEntityTypeConfiguration<Chat>
        {
            public void Configure(EntityTypeBuilder<Chat> builder)
            {
                builder.HasOne(c => c.FirstUser)
                    .WithOne() // Один к одному
                    .HasForeignKey<Chat>(c => c.FirstUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Ограничения внешнего ключа для второго пользователя
                builder.HasOne(c => c.SecondUser)
                    .WithOne() // Один к одному
                    .HasForeignKey<Chat>(c => c.SecondUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }


        public override void Dispose()
        {
            base.Dispose();
            //operationLogStream.Dispose();
            //errorLogStream.Dispose();
        }

        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();
            //await operationLogStream.DisposeAsync();
            //errorLogStream.Dispose();
        }

        public DbSet<DiplomService.Models.Message> Message { get; set; } = default!;
    }
}
