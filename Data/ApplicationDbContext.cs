using BreathingFree.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace BreathingFree.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Chat"); // Tên bảng trong SQL

                entity.HasKey(m => m.ChatID);

                entity.Property(m => m.ChatID).HasColumnName("ChatID");
                entity.Property(m => m.FromUserID).HasColumnName("FromUserID");
                entity.Property(m => m.ToUserID).HasColumnName("ToUserID");
                entity.Property(m => m.MessageContent).HasColumnName("Message");
                entity.Property(m => m.SentAt).HasColumnName("SentAt");

                entity.HasOne(m => m.Sender)
                    .WithMany()
                    .HasForeignKey(m => m.FromUserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Receiver)
                    .WithMany()
                    .HasForeignKey(m => m.ToUserID)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
