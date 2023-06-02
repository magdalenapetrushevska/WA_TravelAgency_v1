using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using WA_TravelAgency_v1.Models.DomainModels;
using WA_TravelAgency_v1.Models.Identity;

namespace WA_TravelAgency_v1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Destination> Destinatons { get; set; }
        public virtual DbSet<Transport> Transport { get; set; }
        public virtual DbSet<Offer> Offers { get; set; }
        public virtual DbSet<OfferParameters> OfferParameters { get; set; }
        public virtual DbSet<Promotion> Promotions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Offer>()
                .HasOne(z => z.Transport)
                .WithMany(z => z.Offers)
                .HasForeignKey(z => z.TransportId);

            builder.Entity<Offer>()
                .HasOne(z => z.Destination)
                .WithMany(z => z.Offers)
                .HasForeignKey(z => z.DestinationId);

            builder.Entity<Offer>()
                .HasOne(e => e.OfferParameters)
                .WithOne(e => e.Offer)
                .HasForeignKey<OfferParameters>(e => e.OfferId)
                .IsRequired();

            builder.Entity<Reservation>()
                .HasOne(z => z.Offer)
                .WithMany(z => z.Reservations)
                .HasForeignKey(z => z.OfferId);

            builder.Entity<Reservation>()
                .HasOne(z => z.Passenger)
                .WithMany(z => z.Reservations)
                .HasForeignKey(z => z.UserId);

            builder.Entity<Offer>()
                .HasOne(e => e.Promotion)
                .WithOne(e => e.Offer)
                .HasForeignKey<Promotion>(e => e.OfferId);

        }

        public DbSet<WA_TravelAgency_v1.Models.DomainModels.Reservation>? Reservation { get; set; }

    }
}