using System;
using Domain.Constants;
using Domain.Entities;

namespace Domain.Builders;

public class UserBuilder
{
    private readonly User _user = new();

    public UserBuilder SetEmail(string email)
    {
        _user.Email = email;
        return this;
    }

    public UserBuilder SetPassword(string? password)
    {
        _user.Password = password ?? DefaultValuesDomainConstants.DefaultPassword;

        return this;
    }
    public UserBuilder SetName(string name)
    {
        _user.Name = name;
        return this;
    }
    public UserBuilder SetDocument(string document)
    {
        _user.Document = document;
        return this;
    }

    public User Build()
    {
        _user.ValidateAll();

        return _user;
    }
}
