using CronExpressionDescriptor;
using DevTools.Core;
using DevTools.Data;
using Hardware.Info;
using HashidsNet;
using Newtonsoft.Json;
using NJsonSchema;
using System.Net.NetworkInformation;

//Rules rules = new Rules();
//var inputs = new Dictionary<string, object>();
//inputs.Add(nameof(Book), new Book()
//{
//    Title = "Test",
//    Url = "",
//    Author = new Author()
//    {
//        Name = "Test",
//        Age = 22,
//        Gender = Gender.Female
//    },
//    Content = "",
//    Rating = 9,
//    Price = 190.6M,
//    PublishDate = new DateOnly(2022, 10, 10)
//});
//var results = await rules.ValidateRules(@"[{
//    'WorkflowName': 'Discount',
//    'Rules': [
//      {
//        'RuleName': 'TestRule',
//        'Expression': 'Book.Rating >=6 AND Book.Author.Age>=20 AND Book.Author.Gender=2'
//      },
//{
//        'RuleName': 'TestRule2',
//        'Expression': 'Book.Price<200'
//      }
//    ]
//  }]", inputs);
//foreach(var result in results)
//{
//    Console.WriteLine($"Workflow:{result.WorkflowName}");
//    foreach(var _result in result.RulesValidateResult)
//    {
//        Console.WriteLine($"-Rule:{_result.RuleName}");
//        Console.WriteLine($"-Expression:{_result.Expression}");
//        Console.WriteLine($"-Result:{_result.Result}");
//        Console.WriteLine($"-Message:{_result.Message}");
//        Console.WriteLine();
//    }
//}
//Console.ReadLine();

//var schemaData = new JsonSchemas().GenerateJsonSchema<Book>(JsonSchemaOutputFormatter.YAML);
//Console.WriteLine(schemaData);

//string output = ExpressionDescriptor.GetDescription("0-10 15 * * *", new Options()
//{
//    DayOfWeekStartIndexZero = false,
//    Use24HourTimeFormat = true,
//    Locale = "zh-Hans"
//});
//Console.WriteLine(output);


//IHardwareInfo hardwareInfo = new HardwareInfo();

//hardwareInfo.RefreshAll();

//Console.WriteLine(hardwareInfo.OperatingSystem);

//Console.WriteLine(hardwareInfo.MemoryStatus);

//foreach (var hardware in hardwareInfo.BatteryList)
//    Console.WriteLine(hardware);

//foreach (var hardware in hardwareInfo.BiosList)
//    Console.WriteLine(hardware);

//foreach (var cpu in hardwareInfo.CpuList)
//{
//    Console.WriteLine(cpu);

//    foreach (var cpuCore in cpu.CpuCoreList)
//        Console.WriteLine(cpuCore);
//}

//foreach (var drive in hardwareInfo.DriveList)
//{
//    Console.WriteLine(drive);

//    foreach (var partition in drive.PartitionList)
//    {
//        Console.WriteLine(partition);

//        foreach (var volume in partition.VolumeList)
//            Console.WriteLine(volume);
//    }
//}

//foreach (var hardware in hardwareInfo.KeyboardList)
//    Console.WriteLine(hardware);

//foreach (var hardware in hardwareInfo.MemoryList)
//    Console.WriteLine(hardware);

//foreach (var hardware in hardwareInfo.MonitorList)
//    Console.WriteLine(hardware);

//foreach (var hardware in hardwareInfo.MotherboardList)
//    Console.WriteLine(hardware);

//foreach (var hardware in hardwareInfo.MouseList)
//    Console.WriteLine(hardware);

//foreach (var hardware in hardwareInfo.NetworkAdapterList)
//    Console.WriteLine(hardware);

//foreach (var hardware in hardwareInfo.PrinterList)
//    Console.WriteLine(hardware);

//foreach (var hardware in hardwareInfo.SoundDeviceList)
//    Console.WriteLine(hardware);

//foreach (var hardware in hardwareInfo.VideoControllerList)
//    Console.WriteLine(hardware);

//foreach (var address in HardwareInfo.GetLocalIPv4Addresses(NetworkInterfaceType.Ethernet, OperationalStatus.Up))
//    Console.WriteLine(address);

//Console.WriteLine();

//foreach (var address in HardwareInfo.GetLocalIPv4Addresses(NetworkInterfaceType.Wireless80211))
//    Console.WriteLine(address);

//Console.WriteLine();

//foreach (var address in HardwareInfo.GetLocalIPv4Addresses(OperationalStatus.Up))
//    Console.WriteLine(address);

//Console.WriteLine();

//foreach (var address in HardwareInfo.GetLocalIPv4Addresses())
//    Console.WriteLine(address);

//string salt = "";
//int minLength = 12;
//long id = 1;
//var hashids = new Hashids(salt, minLength);
//var hash = hashids.EncodeLong(id);
//Console.WriteLine(hash);
//long _id=hashids.DecodeSingleLong(hash);
//Console.WriteLine(_id);


//Book book = new()
//{
//    Title="test",
//    Rating=5,
//    Price=12m,
//};
//var data = book.ShapeData("title,price");
//Console.WriteLine(JsonConvert.SerializeObject(data));
List<Book> books = new List<Book>()
{
    new Book()
    {
        Title = "test",
        Rating = 5,
        Price = 12m,
    },
    new Book()
    {
        Title = "demo",
        Rating = 8,
        Price = 34.5m,
    }
};
var data = books.ShapeData<List<Book>>("title,price");
Console.WriteLine(JsonConvert.SerializeObject(data));

Console.ReadLine(); 

