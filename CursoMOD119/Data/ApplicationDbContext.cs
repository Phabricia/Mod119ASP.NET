﻿

using CursoMOD119.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CursoMOD119.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Item> Items { get; set; } = default!;
        public DbSet<StockMovement> StockMovements { get; set; } = default!;
        public DbSet<Sale> Sales { get; set; } = default!;
        public DbSet<CursoMOD119.Models.Client> Client { get; set; } = default!;
    }
}