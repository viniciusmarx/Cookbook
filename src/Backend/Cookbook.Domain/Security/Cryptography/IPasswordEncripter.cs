﻿namespace Cookbook.Domain.Security.Cryptography;

public interface IPasswordEncripter
{
    public string Encrypt(string password);
}