using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookQuoteApp.Api.Models;

namespace BookQuoteApp.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Publisher> Publisher { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublisherId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Quote>()
                .HasOne(q => q.Book)
                .WithMany(b => b.Quotes)
                .HasForeignKey(q => q.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Quote>()
                .HasOne(q => q.User)
                .WithMany()
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Quote>()
                .HasOne(q => q.Author)
                .WithMany(a => a.Quotes)
                .HasForeignKey(q => q.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
