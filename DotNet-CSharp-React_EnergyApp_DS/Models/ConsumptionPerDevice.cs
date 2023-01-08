namespace DotNet_CSharp_React_EnergyApp_DS.Models;

public class ConsumptionPerDevice
{
    public int ConsumptionPerDeviceId { get; set; }

    public int DeviceMappingId { get; set; }
    public DeviceMapping DeviceMapping { get; set; }

    public DateTime DateTime { get; set; }

    public double ConsumptionPerHour { get; set; }
}