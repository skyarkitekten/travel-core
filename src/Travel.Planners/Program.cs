// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = loggerFactory.CreateLogger<Program>();


