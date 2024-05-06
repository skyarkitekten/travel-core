using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Plugins.Core;

using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSimpleConsole(options =>
    {
        options.IncludeScopes = true;
        options.SingleLine = true;
        options.TimestampFormat = "HH:mm:ss ";
    });
    builder.SetMinimumLevel(LogLevel.Debug);
});

ILogger logger = loggerFactory.CreateLogger<Program>();

logger.LogInformation("Reading config...");
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>()
    .Build();

var model = configuration["AzureOpenAI:Model"] ?? "gpt-3.5-turbo";
var endpoint = configuration["AzureOpenAI:ApiEndpoint"] ?? throw new ArgumentNullException("AzureOpenAI:ApiEndpoint");
var key = configuration["AzureOpenAI:ApiKey"] ?? throw new ArgumentNullException("AzureOpenAI:ApiKey");


logger.LogInformation("Building kernel...");
var builder = Kernel.CreateBuilder();
builder.Services.AddAzureOpenAIChatCompletion(model, endpoint, key);
builder.Services.AddSingleton(loggerFactory);

#pragma warning disable SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

builder.Plugins.AddFromType<TimePlugin>();
builder.Plugins.AddFromType<ConversationSummaryPlugin>();

#pragma warning restore SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

var kernel = builder.Build();
logger.LogInformation("Kernel built...");

Console.WriteLine("USE CASE 1: Get the current day of the week using the TimePlugin ************************************************");
var currentDay = await kernel.InvokeAsync("TimePlugin", "DayOfWeek");
Console.WriteLine($"Today is {currentDay}.");

Console.WriteLine("USE CASE 2: Get the conversation topics items using the ConversationSummaryPlugin ********************************");
string input = @"I love BBQ and it pairs especially well with Ohio State Buckeyes football. 
Some of my favorite BBQ spots are Ray Ray's Hog Pit, City Barbeque, and smoking my own brisket.";

var result = await kernel.InvokeAsync(
    "ConversationSummaryPlugin",
    "GetConversationTopics",
    new() { { "input", input } });

Console.WriteLine(result);

Console.WriteLine("USE CASE 3: Summarize the conversation and suggest recipes based on user background ********************************");
string history = @"It's game day and the Buckeyes are heavily favored. The weather is brisk and sunny, 
    and the tailgating will start at 10:30 AM. I ahve a few friends coming by, and I want to make sure
    that I have some delicious food ready for them.

    One of friends must have beer with their BBQ, and another friend has been sober for 5 years.
    Armed with creativity and a passion for wholesome 
    cooking, I've embarked on a flavorful adventure, for things that pair well with a 36-hour smoked beef brisket.";

// NOTE: the {..} it templating language that lets us call functions within the prompt
string functionPrompt = @"User background: 
    {{ConversationSummaryPlugin.SummarizeConversation $history}}
    Given this user's background, provide a list of relevant recipes.";

var suggestRecipes = kernel.CreateFunctionFromPrompt(functionPrompt);
result = await kernel.InvokeAsync(suggestRecipes,
    new KernelArguments() {
        { "history", history }
    });
Console.WriteLine(result);

Console.WriteLine("USE CASE 4: Suggest destinations based on travel plans ********************************");
var prompts = kernel.ImportPluginFromPromptDirectory("Prompts/TravelPlugins");


ChatHistory chatHistory = [];
string travelPlans = @"I'm planning an anniversary trip with my 
    spouse. We like hiking, mountains, and beaches. Our 
    travel budget is $15000";

var chatResult = await kernel.InvokeAsync<string>(prompts["SuggestDestinations"],
    new() {
        { "input", travelPlans },
    }
);

Console.WriteLine(chatResult);
chatHistory.AddUserMessage(input);
chatHistory.AddAssistantMessage(chatResult);

Console.WriteLine("Where would you like to go?");
input = Console.ReadLine();

chatResult = await kernel.InvokeAsync<string>(prompts["SuggestActivities"],
    new() {
        { "history", history },
        { "destination", input },
    }
);
Console.WriteLine(chatResult);

logger.LogInformation("Finished..");