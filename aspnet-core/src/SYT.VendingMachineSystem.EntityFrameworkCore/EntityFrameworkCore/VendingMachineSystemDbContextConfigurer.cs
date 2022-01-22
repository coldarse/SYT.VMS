using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace SYT.VendingMachineSystem.EntityFrameworkCore
{
    public static class VendingMachineSystemDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<VendingMachineSystemDbContext> builder, string connectionString)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
            builder.UseMySql(connectionString, serverVersion);
        }

        public static void Configure(DbContextOptionsBuilder<VendingMachineSystemDbContext> builder, DbConnection connection)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
            builder.UseMySql(connection, serverVersion);
        }
    }
}
