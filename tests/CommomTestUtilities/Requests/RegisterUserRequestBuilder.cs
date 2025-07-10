using Bogus;
using Cookbook.Communication.Requests;

namespace CommomTestUtilities.Requests;

public class RegisterUserRequestBuilder
{
    public static RegisterUserRequest Build(int passwordLength = 10)
    {
        return new Faker<RegisterUserRequest>()
            .RuleFor(user => user.Name, fake => fake.Person.FirstName)
            .RuleFor(user => user.Email, (fake, user) => fake.Internet.Email(user.Name))
            .RuleFor(user => user.Password, fake => fake.Internet.Password(passwordLength));
    }
}
