namespace DotNet_CSharp_React_EnergyApp_DS.Models;

public class DeviceMapping
{
    public int DeviceMappingId { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int DeviceId { get; set; }
    public Device Device { get; set; }
}