using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Core;

internal class TravelPlaylist
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Reading config...");
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>()
            .Build();

        var model = configuration["AzureOpenAI:Model"] ?? "gpt-3.5-turbo";
        var endpoint = configuration["AzureOpenAI:ApiEndpoint"] ?? throw new ArgumentNullException("AzureOpenAI:ApiEndpoint");
        var key = configuration["AzureOpenAI:ApiKey"] ?? throw new ArgumentNullException("AzureOpenAI:ApiKey");


        Console.WriteLine("Building kernel...");
        var builder = Kernel.CreateBuilder();
        builder.Services.AddAzureOpenAIChatCompletion(model, endpoint, key);

        var kernel = builder.Build();
        Console.WriteLine("Kernel built...");
        kernel.ImportPluginFromType<MusicLibraryPlugin>();

        var result = await kernel.InvokeAsync(
            "MusicLibraryPlugin",
            "AddToRecentlyPlayed",
            new()
            {
                ["artist"] = "Tiara",
                ["song"] = "Danse",
                ["genre"] = "French pop, electropop, pop"
            }
        );

        Console.WriteLine(result);
    }
}