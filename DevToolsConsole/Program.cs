using CronExpressionDescriptor;
using CsvHelper.Configuration.Attributes;
using DevTools.Core;
using DevTools.Data;
using Hardware.Info;
using HashidsNet;
using Microsoft.Diagnostics.Tracing.Parsers.AspNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using RulesEngine.Models;
using System;
using System.Dynamic;
using System.Globalization;
using System.Linq.Dynamic.Core;
using System.Net.NetworkInformation;
using Z.Expressions;

using IHost host = Host.CreateDefaultBuilder(args)
    .UseDefaultServiceProvider((context, options) => {
        //options.ValidateScopes = true;
    })
    .ConfigureServices((context, services) =>
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<IWorkflowsEngine, WorkflowsEngine>();
    })
    .Build();

var time = DateTime.Now.Month == 8 || DateTime.Now.Month == 9 || DateTime.Now.Month == 10;
var bd = @" DateTime.Now>= DateTime.Parse(""10:23:23"") && DateTime.Now<=DateTime.Parse(""12:34:56"")".Execute<bool>();
var dfs=CultureInfo.GetCultureInfo("en-US").DateTimeFormat.MonthNames;
//string.Concat(prefix, suffix);
while (Console.ReadKey().KeyChar=='R')
{
    Console.WriteLine();
    var ex = "string.Concat(\"abc-\",DateTime.Now.ToString(\"yyyyMMddHHmmss\"),\"-\",Random.Shared.Next(100, 1000), \"-xyz\")".Execute<dynamic>();
    Console.WriteLine(ex);
}

var courtesyCardRule = new CourtesyCardRule("test", 
    CourtesyCardType.FullCouponReduction, 
    TargetObject.Product, 
    new TermOfValidity(false,3,LimitType.Day), 
    new TimeInterval(false,"13:12:00","22:22:23"), 
    "", 
    "", 
    "");
Console.WriteLine(JsonConvert.SerializeObject(courtesyCardRule, Formatting.Indented));
var order = new Order(120.45M);
order.AddProducts(new List<Product>()
{
    new Product("a",12.56M,2),
    new Product("a",11.21M,3),
    new Product("a",22.34M,5),
});
Console.WriteLine(JsonConvert.SerializeObject(order, Formatting.Indented));
#region Eval Expression
var _result = Enum.GetValues<DayOfWeek>().Where(d => d == DayOfWeek.Friday || d == DayOfWeek.Sunday).Contains(DateTime.Now.DayOfWeek);
var result = "Enum.GetValues<DayOfWeek>().Where(d=>d==DayOfWeek.Friday||d==DayOfWeek.Sunday).Contains(DateTime.Now.DayOfWeek)".Execute<bool>();
Console.WriteLine(result);
var result1 = "DayOfWeeks.Where(d=>d==DayOfWeek.Friday||d==DayOfWeek.Sunday).Contains(DateTime.Now.DayOfWeek)"
    .Execute<bool>(new { DayOfWeeks = Enum.GetValues<DayOfWeek>() });
Console.WriteLine(result1);
Console.ReadKey();
#endregion

#region WorkflowEngine
JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
{
    ContractResolver = new CamelCasePropertyNamesContractResolver()
};

Console.WriteLine("Input Schema:");
string inputSchemaData = new JsonSchemas().GenerateJsonSchema<WorkflowDto>(JsonSchemaOutputFormatter.JSON, jsonSerializerSettings);
Console.WriteLine(inputSchemaData);

IWorkflowsEngine workflowEngine = host.Services.GetRequiredService<IWorkflowsEngine>();
List<WorkflowDto> workflowDtos = new List<WorkflowDto>()
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
                    Price=100M,
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
                            Title="Test",
                            Money=11.3M,
                        },
                        new Chapter()
                        {
                            Title="Test1",
                            Money=23.5M,
                        },
                        new Chapter()
                        {
                            Title="Test2",
                            Money=36.1M,
                        }
                    }
                }
            },
            new Input()
            {
                Name="DayOfWeeks",
                Value=Enum.GetValues<DayOfWeek>()
            }
        },
        Rules=new List<RuleDto>()
        {
            new RuleDto()
            {
                RuleName="10%折扣",
                Description="当价格大于等于40且评分大于等于8时提供10%的折扣",
                ErrorMessage="oops",
                Expression=@"Book.Price>40 AND Book.Rating>=8 AND Book.Chapters.Where(d=>d.Title.Contains(""Test"")).Sum(Money)>70",
                SuccessEvent="10"
            },
            new RuleDto()
            {
                RuleName="15%折扣",
                Description="当价格大于等于60且评分大于等于9且作者为女性且章节数大于等于3时提供15%的折扣",
                ErrorMessage="oops",
                Expression=@"Book.Price>60 AND Book.Rating>=9 AND Book.Author.Gender=""Female"" AND Book.Chapters.Count()>=3",
                SuccessEvent="15"
            },
            new RuleDto()
            {
                RuleName="20%折扣",
                Description="当价格大于等于80且评分大于等于9且作者为男性时提供15%的折扣",
                Enabled=true,
                ErrorMessage="oops",
                Expression=@"Book.Price>80 AND Book.Rating>=9 AND Book.Author.Gender=""Male""",
                SuccessEvent="20"
            },
            new RuleDto()
            {
                RuleName="Test",
                Description="当价格大于等于80且评分大于等于9且作者为男性时提供15%的折扣",
                ErrorMessage="oops",
                Expression="DayOfWeeks.Where(d=>d==DayOfWeek.Friday||d==DayOfWeek.Sunday).Contains(DateTime.Now.DayOfWeek)",
                SuccessEvent="Test"
            },
        }
    }
};
Console.WriteLine("Input Data:");
Console.WriteLine(JsonConvert.SerializeObject(workflowDtos, Formatting.Indented, jsonSerializerSettings));

List<WorkflowRulesValidateResult> results = await workflowEngine.Validate(workflowDtos);

Console.WriteLine("Output Schema:");
string outputSchemaData = new JsonSchemas().GenerateJsonSchema<WorkflowRulesValidateResult>(JsonSchemaOutputFormatter.JSON, jsonSerializerSettings);
Console.WriteLine(outputSchemaData);

Console.WriteLine("Output Data:");
Console.WriteLine(JsonConvert.SerializeObject(results, Formatting.Indented, jsonSerializerSettings));

Console.ReadLine();
#endregion

#region Cron
//string output = ExpressionDescriptor.GetDescription("0-10 15 * * *", new Options()
//{
//    DayOfWeekStartIndexZero = false,
//    Use24HourTimeFormat = true,
//    Locale = "zh-Hans"
//});
//Console.WriteLine(output);

//Console.ReadLine();
#endregion

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

#region hashids
//string salt = "";
//int minLength = 12;
//long id = 1;
//var hashids = new Hashids(salt, minLength);
//var hash = hashids.EncodeLong(id);
//Console.WriteLine(hash);
//long _id = hashids.DecodeSingleLong(hash);
//Console.WriteLine(_id);
#endregion




