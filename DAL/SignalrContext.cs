using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SignalrDemo.EFModels;

namespace SignalrDemo.DAL;

public partial class SignalrContext : DbContext
{
    public SignalrContext()
    {
    }

    public SignalrContext(DbContextOptions<SignalrContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Connection> Connections { get; set; }

    public virtual DbSet<Person> People { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:MyConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Connection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("connections_pkey");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("person_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
