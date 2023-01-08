using Microsoft.VisualBasic.FileIO;

namespace RabbitMQ;

public class ParseCSV
{
    public static List<double> readCSV()
    {
        var csvValues = new List<double>();
        using (var parser =
               new TextFieldParser(
                   @"/Users/acsatlos/Projects/UTCN-DS-Project/dotnet-backend/DotNet-CSharp-React_EnergyApp_DS/DotNet-CSharp-React_EnergyApp_DS/RabbitMQ/sensor (1).csv"))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            while (!parser.EndOfData)
            {
                //Process row
                var fields = parser.ReadFields();
                if (fields != null)
                    foreach (var field in fields)
                        csvValues.Add(Convert.ToDouble(field));
                // Console.WriteLine(field);
            }
        }

        return csvValues;
    }

    public static void Main()
    {
        // readCSV();
    }
}