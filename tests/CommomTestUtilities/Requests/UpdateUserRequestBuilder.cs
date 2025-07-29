using Bogus;
using Cookbook.Communication.Requests;

namespace CommomTestUtilities.Requests;

public class UpdateUserRequestBuilder
{
    public static UpdateUserRequest Build()
    {
        return new Faker<UpdateUserRequest>()
            .RuleFor(user => user.Name, (f) => f.Person.FirstName)
            .RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Name));
    }
}