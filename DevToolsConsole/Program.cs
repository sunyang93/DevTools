using CronExpressionDescriptor;
using DevTools.Core;
using DevTools.Data;
using Hardware.Info;
using HashidsNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using RulesEngine.Models;
using System.Dynamic;
using System.Net.NetworkInformation;
using Z.Expressions;

using var host = Host.CreateDefaultBuilder(args)
    .UseDefaultServiceProvider((context, options) => {
        //options.ValidateScopes = true;
    })
    .ConfigureServices((context, services) =>
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<IWorkflowsEngine, WorkflowsEngine>();
    })
    .Build();

Console.WriteLine("Input Schema:");
var inputSchemaData = new JsonSchemas().GenerateJsonSchema<WorkflowDto>(JsonSchemaOutputFormatter.JSON);
Console.WriteLine(inputSchemaData);

var workflowEngine = host.Services.GetRequiredService<IWorkflowsEngine>();
var workflowDtos = new List<WorkflowDto>()
{
    new WorkflowDto()
    {
        WorkflowName="Test",
        Description="Test",
        Inputs=new List<Input>()
        { 
            new Input()
            {
                Name=nameof(Book),
                Description="Test",
                Value=new Book()
                {
                    Title="Test",  
                    TotalPages=365,
                    Rating=9,
                    Price=200.6M,
                    Author=new Author()
                    {
                        Name="Test",
                        Age=22,
                        Gender=Gender.Female
                    },
                    Chapters=new List<Chapter>()
                    {
                        new Chapter()
                        {
                            Title="Test"
                        },
                        new Chapter()
                        {
                            Title="Test"
                        }
                    }
                } 
            }
        },
        Rules=new List<RuleDto>()
        {
            new RuleDto()
            {
                RuleName="10%折扣",
                Description="9折",
                ErrorMessage="oops",
                Expression=@"Book.Price>10 AND Book.Rating>=6 AND Book.Author.Gender=""Female"" AND Book.Chapters.Count()>1",
                SuccessEvent="10.2"
            },
            new RuleDto()
            {
                RuleName="20%折扣",
                Description="8折",
                Enabled=false,
                ErrorMessage="oops",
                Expression=@"Book.Price>50 AND Book.Rating>=8 AND Book.Author.Gender=""Female"" AND Book.Chapters.Count()>1",
                SuccessEvent="20"
            },
            new RuleDto()
            {
                RuleName="30%折扣",
                Description="7折",
                ErrorMessage="oops",
                Expression=@"Book.Price>150 AND Book.TotalPages>300 AND Book.Rating>=9 AND Book.Author.Gender=""Female""",
                SuccessEvent="30"
            },
        }
    }
};
var results = await workflowEngine.Validate(workflowDtos);

Console.WriteLine("Input Data:");
Console.WriteLine(JsonConvert.SerializeObject(workflowDtos, Formatting.Indented));

Console.WriteLine("Output Schema:");
var outputSchemaData = new JsonSchemas().GenerateJsonSchema<WorkflowRulesValidateResult>(JsonSchemaOutputFormatter.JSON);
Console.WriteLine(outputSchemaData);

Console.WriteLine("Output Data:");
foreach (var result in results)
{
    Console.WriteLine($"Workflow:{result.WorkflowName}");
    Console.WriteLine($"IsSuccess:{result.IsSuccess}");
    foreach (var _result in result.RulesValidateResult)
    {
        Console.WriteLine($"-Rule:{_result.RuleName}");
        Console.WriteLine($"--Expression:{_result.Expression}");
        Console.WriteLine($"--IsSuccess:{_result.IsSuccess}");
        Console.WriteLine($"--ExceptionMessage:{_result.ExceptionMessage}");
        Console.WriteLine($"--SuccessEvent:{_result.SuccessEvent}");
        Console.WriteLine();
    }
}

Console.ReadLine();

//string output = ExpressionDescriptor.GetDescription("0-10 15 * * *", new Options()
//{
//    DayOfWeekStartIndexZero = false,
//    Use24HourTimeFormat = true,
//    Locale = "zh-Hans"
//});
//Console.WriteLine(output);
//Console.ReadLine();


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
//List<Book> books = new List<Book>()
//{
//    new Book()
//    {
//        Title = "test",
//        Rating = 5,
//        Price = 12m,
//    },
//    new Book()
//    {
//        Title = "demo",
//        Rating = 8,
//        Price = 34.5m,
//    }
//};
//var data = books.ShapeData<List<Book>>("title,price");
//Console.WriteLine(JsonConvert.SerializeObject(data));
//Console.ReadLine(); 




