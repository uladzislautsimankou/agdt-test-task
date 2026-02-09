using Bogus;
using TestTask.Core.Dto;
using TestTask.Core.Enums;

namespace TestTask.ConsoleApp.Services;

internal sealed class TestDataGenerator
{
    private readonly Faker<CreateOrUpdatePatientDto> _faker = CreateFaker();

    private static Faker<CreateOrUpdatePatientDto> CreateFaker()
    {
        var genders = Enum.GetValues<Gender>();
        var nameUses = Enum.GetValues<NameUse>();

        return new Faker<CreateOrUpdatePatientDto>("en")
            .CustomInstantiator(f => new CreateOrUpdatePatientDto(
                Gender: f.PickRandom<Gender>(),
                BirthDate: f.Date.Past(60, DateTime.Now.AddYears(-18)),
                Active: f.Random.Bool(),
                Name: new List<CreateOrUpdateHumanNameDto>
                {
                    new CreateOrUpdateHumanNameDto(
                        Use: f.PickRandom<NameUse>(),
                        Family: f.Name.LastName(),
                        Given: new List<string> { f.Name.FirstName(), f.Name.Suffix() }
                    )
                }
            ));
    }

    public List<CreateOrUpdatePatientDto> Generate(int count) => _faker.Generate(count);
}
