using Bogus;
using Cookbook.Communication.Requests;

namespace CommomTestUtilities.Requests;

public class LoginRequestBuilder
{
    public static LoginRequest Build()
    {
        return new Faker<LoginRequest>()
            .RuleFor(u => u.Email, (f) => f.Internet.Email())
            .RuleFor(u => u.Password, (f) => f.Internet.Password());
    }
}
