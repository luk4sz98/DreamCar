using DreamCar.Model.DataModels;
using DreamCar.Model.DataModels.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DreamCar.DAL.EF
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        public virtual DbSet<Advert> Adverts { get; set; }
        public virtual DbSet<AdvertThread> AdvertThreads { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<CarEquipment> CarEquipments { get; set; }
        public virtual DbSet<FollowAdvert> FollowAdverts { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Equipment> Equipment { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies(); // <-- enable lazy loading
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Fluent API commands
            modelBuilder.Entity<User>()
                .ToTable("AspNetUsers")
                .HasDiscriminator<int>("UserType")
                .HasValue<User>((int)RoleType.User);

            modelBuilder.Entity<FollowAdvert>()
                .HasKey(fa => new { fa.UserId, fa.AdvertId });

            modelBuilder.Entity<Advert>()
                .HasMany(ad => ad.Images)
                .WithOne(img => img.Advert)
                .HasForeignKey(img => img.AdvertId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Advert>()
                .HasMany(ad => ad.AdvertThreads)
                .WithOne(at => at.Advert)
                .HasForeignKey(at => at.AdvertId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Advert>()
                .HasOne(ad => ad.Car)
                .WithOne(c => c.Advert)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Advert>()
                .HasOne(ad => ad.User)
                .WithMany(cl => cl.Adverts)
                .HasForeignKey(ad => ad.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AdvertThread>()
                .HasMany(at => at.Messages)
                .WithOne(msg => msg.AdvertThread)
                .HasForeignKey(msg =>  msg.AdvertThreadId)
                .OnDelete(DeleteBehavior.Cascade);
            

            modelBuilder.Entity<CarEquipment>()
                .HasKey(carEqu => new { carEqu.CarId, carEqu.EquipmentId });

            modelBuilder.Entity<CarEquipment>()
                .HasOne(carEqu => carEqu.Car)
                .WithMany(car => car.CarEquipment)
                .HasForeignKey(carEqu => carEqu.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CarEquipment>()
                .HasOne(carEqu => carEqu.Equipment)
                .WithMany(equ => equ.CarEquipment)
                .HasForeignKey(carEqu => carEqu.EquipmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FollowAdvert>()
                .HasOne(fa => fa.User)
                .WithMany(cl => cl.FollowAdverts)
                .HasForeignKey(fa => fa.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FollowAdvert>()
                .HasOne(fa => fa.Advert)
                .WithMany(ad => ad.FollowAdverts)
                .HasForeignKey(fa => fa.AdvertId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(mg => mg.User)
                .WithMany(cl => cl.Messages)
                .HasForeignKey(mg => mg.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
