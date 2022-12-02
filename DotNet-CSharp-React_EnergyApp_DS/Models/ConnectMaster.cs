namespace DotNet_CSharp_React_EnergyApp_DS.Models;

public class ConnectMaster
{
    public int ConnectMasterId { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}