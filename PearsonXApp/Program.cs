
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PearsonXApp.UseCases;
using PearsonXApp.UseCases.CommandExecutor;
using PearsonXApp.UseCases.Follow;
using PearsonXApp.UseCases.Post;
using PearsonXApp.UseCases.Users;
using PearsonXApp.UseCases.Users.Seeder;
using PearsonXApp.UseCases.Wall;

namespace PearsonXApp
{
    public static class Program
    {
        private const string Prompt = @"
Please select an option:
Post. Write a message
Follow. Connect with another user
Wall. Display all messages from specified user
Exit. Close the application

Enter the key of your choice: ";

        private const string CloseAppCommand = "exit";

        public static void Main(string[] args)
        {
            IConfigurationRoot config = LoadAppSettings();

            IServiceProvider serviceProvider = CreateServices(config);

            StartConsoleApp(serviceProvider);
        }

        public static void StartConsoleApp(IServiceProvider serviceProvider)
        {
            ICommandExecutor commandExecutor = serviceProvider.GetRequiredService<ICommandExecutor>();

            bool canExecute = true;

            while (canExecute)
            {
                Console.WriteLine(Prompt);

                string? command = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(command))
                {
                    continue;
                }

                string[]? commandParts = command.Split(" ");

                //Validate exit option
                if (ExitCommandReceived(commandParts))
                {
                    canExecute = false;
                    continue;
                }

                commandExecutor.ExecuteCommand(commandParts);
            }
        }

        /// <summary>
        /// Checks if the command is to close the app
        /// </summary>
        /// <param name="commandParts"></param>
        /// <returns></returns>
        public static bool ExitCommandReceived(string[] commandParts)
        {
            return commandParts[(int)MessageHelper.ActionIndex].Trim().Equals(CloseAppCommand, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Configure dependency injection
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static IServiceProvider CreateServices(IConfigurationRoot config)
        {
            var serviceProvider = new ServiceCollection()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IPostService, PostService>()
                .AddScoped<IFollowService, FollowService>()
                .AddScoped<IWallService, WallService>()
                .AddScoped<ICommandExecutor, CommandExecutor>()
                .AddSingleton(config)
                .AddSingleton<IUserSeeder, UserSeeder>()
                .BuildServiceProvider();

            return serviceProvider;
        }

        /// <summary>
        /// load app settings from appsettings.json
        /// </summary>
        /// <returns></returns>
        private static IConfigurationRoot LoadAppSettings()
        {
            return new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json")
                            .Build();
        }
    }
}