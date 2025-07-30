﻿using Bogus;
using Cookbook.Communication.Requests;

namespace CommomTestUtilities.Requests;

public class ChangePasswordRequestBuilder
{
    public static ChangePasswordRequest Build(int passwordLength = 10)
    {
        return new Faker<ChangePasswordRequest>()
            .RuleFor(user => user.Password, f => f.Internet.Password())
            .RuleFor(user => user.NewPassword, f => f.Internet.Password(passwordLength));
    }
}