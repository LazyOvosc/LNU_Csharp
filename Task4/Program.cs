using ConsoleTables;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Markup;

enum Localization { en, ua}
interface IProducts
{
    List<string> Values();
}
class Food: IProducts
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public DateOnly ExpirationDate { get; set; }

    public Food(string name, decimal price, DateOnly expirationDate)
    {
        Name = name;
        Price = price;
        ExpirationDate = expirationDate;
    }

    public List<string> Values()
    {
        List<string> values = new List<string>();
        values.Add(Name);
        values.Add(Price.ToString());
        values.Add(ExpirationDate.ToString());
        return values;
    }
}

class HouseholdAppliance: IProducts
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int WarrantyPeriodInMonth { get; set; }

    public HouseholdAppliance(string name, decimal price, int warrantyPeriodInMonth)
    {
        Name = name;
        Price = price;
        WarrantyPeriodInMonth = warrantyPeriodInMonth;
    }

    public List<string> Values()
    {
        List<string> values = new List<string>();
        values.Add(Name);
        values.Add(Price.ToString());
        values.Add("-");
        values.Add(WarrantyPeriodInMonth.ToString());
        return values;
    }
}

class SmartOutput
{
    public List<string> ColumnNames {  get; set; }

    public SmartOutput(List<string> columnNames)
    {
        ColumnNames = columnNames;
    }

    private List<List<string>> NonParsedList(List<IProducts> products)
    {
        List<List<string>> values = new List<List<string>>();
        foreach (var product in products)
        {
            List<string> rowValues = new List<string>();
            foreach (var columnName in ColumnNames)
            {
                Type type = product.GetType();
                PropertyInfo property = type.GetProperty(columnName);
                if (property != null)
                {
                    object value = property.GetValue(product);
                    rowValues.Add(value?.ToString() ?? "-");
                }
                else
                {
                    rowValues.Add("-");
                }
            }
            while (rowValues.Count < ColumnNames.Count)
            {
                rowValues.Add("-");
            }
            values.Add(rowValues);
        }
        return values;
    }

    private List<List<string>> Parser(List<IProducts> products, string localization)
    {
        List<List<string>> data = NonParsedList(products);

        for (int i = 0; i < data.Count; i++)
        {
            for (int j = 0; j < data[i].Count; j++)
            {
                if (DateOnly.TryParse(data[i][j], out DateOnly parsedDate))
                {
                    data[i][j] = localization == "en"
                                ? parsedDate.ToString("yyyy/MM/dd")
                                : parsedDate.ToString("dd.MM.yyyy"); 
                }
                else if (decimal.TryParse(data[i][j], out decimal parsedDecimal))
                {
                    data[i][j] = localization == "en"
                                ? parsedDecimal.ToString("n2")
                                : parsedDecimal.ToString("n2").Replace(",", "");
                }
                else
                {
                    continue;
                }
            }
        }
        return data;
    }

    public void TablePrint(List<IProducts> products, string localization)
    {
        var data = Parser(products, localization);
        ConsoleTable table = new ConsoleTable(ColumnNames.ToArray());
        foreach (var item in data)
        {
            table.AddRow(item.ToArray());
        }
        table.Write();
    }

}

class Program
{
    public static string InputLocalization()
    {
        bool incorrect = true;
        Console.WriteLine("Enter localization(en/ua):");
        string input = "a";
        while (incorrect)
        {
            input = Console.ReadLine();
            if (input == "en" || input == "ua")
            {
                incorrect = false;
            }
            else
            {
                Console.WriteLine("Wrong input. Try again:");
                continue;
            }
        }
        return input;
    }
    static void Main(string[] args)
    {
        List<string> list = new List<string> { "Name", "Price", "ExpirationDate", "WarrantyPeriodInMonth" };

        var products = new List<IProducts>();
        products.Add(new Food("Apple", 15.50m, new System.DateOnly(2023, 01, 01)));
        products.Add(new Food("Banana", 17.69m, new System.DateOnly(2023, 01, 03)));
        products.Add(new HouseholdAppliance("Washing Machine", 1234.23m, 15));
        products.Add(new HouseholdAppliance("Fridge", 3245.34m, 24));
        string localization = InputLocalization();
        SmartOutput tableprint = new SmartOutput(list);
        tableprint.TablePrint(products, localization);
    }
}