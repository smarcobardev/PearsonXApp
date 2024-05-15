using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PearsonXApp.Entities;

namespace PearsonXApp.UseCases.Users.Seeder
{
    public class UserSeeder(IConfigurationRoot _config) : IUserSeeder
    {        

        public List<User>? SeedUsers()
        {
            return _config.GetSection("Users").Get<List<User>>();            
        }
    }
}
