using Microsoft.EntityFrameworkCore;

namespace DotNet_CSharp_React_EnergyApp_DS.Models;

public class DeviceManagerContext : DbContext
{
    public DeviceManagerContext(DbContextOptions<DeviceManagerContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Device> Devices { get; set; }

    public DbSet<ConsumptionPerDevice> ConsumptionPerDevice { get; set; }
    public DbSet<DeviceMapping> DeviceMappings { get; set; }

    public DbSet<ConnectMaster>? ConnectMaster { get; set; }
}