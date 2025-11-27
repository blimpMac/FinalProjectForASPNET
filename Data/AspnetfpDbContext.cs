using System;
using System.Collections.Generic;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data;

public partial class AspnetfpDbContext : DbContext
{
    public AspnetfpDbContext()
    {
    }

    public AspnetfpDbContext(DbContextOptions<AspnetfpDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BookTbl> BookTbls { get; set; }

    public virtual DbSet<BorrowerTbl> BorrowerTbls { get; set; }

    public virtual DbSet<LibraryTransactionVw> LibraryTransactionVws { get; set; }

    public virtual DbSet<ReturneeTbl> ReturneeTbls { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=sql.bsite.net\\MSSQL2016;Database=aspnetfp_;User Id=aspnetfp_;Password=aspnetfinals;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookTbl>(entity =>
        {
            entity.HasKey(e => e.Isbnnumber).HasName("PK__Book_tbl__CFE40BFA65E861C2");

            entity.ToTable("Book_tbl");

            entity.Property(e => e.Isbnnumber)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("ISBNNumber");
            entity.Property(e => e.AuthorName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Availability)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.BookName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PrintedBy)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<BorrowerTbl>(entity =>
        {
            entity.HasKey(e => e.BorrowId).HasName("PK__Borrower__4295F85FBC063EC2");

            entity.ToTable("Borrower_tbl");

            entity.Property(e => e.BorrowId).HasColumnName("BorrowID");
            entity.Property(e => e.BookIsbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("BookISBN");
            entity.Property(e => e.Course)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Section)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.BookIsbnNavigation).WithMany(p => p.BorrowerTbls)
                .HasForeignKey(d => d.BookIsbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Borrowers_Book");
        });

        modelBuilder.Entity<LibraryTransactionVw>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("LibraryTransaction_vw");

            entity.Property(e => e.AuthorName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.BookName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.BorrowId).HasColumnName("BorrowID");
            entity.Property(e => e.BorrowerName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Condition)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Course)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Isbnnumber)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("ISBNNumber");
            entity.Property(e => e.LoanStatus)
                .HasMaxLength(18)
                .IsUnicode(false);
            entity.Property(e => e.ReturnId).HasColumnName("ReturnID");
            entity.Property(e => e.Section)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ReturneeTbl>(entity =>
        {
            entity.HasKey(e => e.ReturnId).HasName("PK__Returnee__F445E98819CE5B46");

            entity.ToTable("Returnee_tbl");

            entity.Property(e => e.ReturnId).HasColumnName("ReturnID");
            entity.Property(e => e.BorrowId).HasColumnName("BorrowID");
            entity.Property(e => e.Condition)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.BookTitle)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("BookTitle");

            entity.HasOne(d => d.Borrow).WithMany(p => p.ReturneeTbls)
                .HasForeignKey(d => d.BorrowId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Returned_Borrower");
        });

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
