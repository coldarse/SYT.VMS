﻿using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using SYT.VendingMachineSystem.Authorization.Roles;
using SYT.VendingMachineSystem.Authorization.Users;
using SYT.VendingMachineSystem.MultiTenancy;
using SYT.VendingMachineSystem.VendingMachines;
using SYT.VendingMachineSystem.ActivityLogs;

namespace SYT.VendingMachineSystem.EntityFrameworkCore
{
    public class VendingMachineSystemDbContext : AbpZeroDbContext<Tenant, Role, User, VendingMachineSystemDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<VendingMachine> VendingMachines { get; set; }

        public DbSet<ActivityLog> ActivityLogs { get; set; }
        
        public VendingMachineSystemDbContext(DbContextOptions<VendingMachineSystemDbContext> options)
            : base(options)
        {
        }
    }
}
