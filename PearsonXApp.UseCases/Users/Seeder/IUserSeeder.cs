using PearsonXApp.Entities;
namespace PearsonXApp.UseCases.Users.Seeder
{
    public interface IUserSeeder
    {
        List<User>? SeedUsers();
    }
}
