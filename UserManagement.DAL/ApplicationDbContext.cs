﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserManagement.DAL.Entities;

namespace UserManagement.DAL;

/// <summary>
/// Auth database context
/// </summary>
public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid, IdentityUserClaim<Guid>,
    IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>> {

    /// <summary>
    /// Users table
    /// </summary>
    public new DbSet<User> Users { get; set; }

    /// <summary>
    /// Users table
    /// </summary>
    public new DbSet<UserGroup> UserGroups { get; set; }

    /// <summary>
    /// Users table
    /// </summary>
    public new DbSet<UserState> UserStates { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        builder.Entity<User>().HasOne(u => u.UserGroup).WithMany(g => g.Users);
        builder.Entity<User>().HasOne(u => u.UserState).WithMany(g => g.Users);
    }
}