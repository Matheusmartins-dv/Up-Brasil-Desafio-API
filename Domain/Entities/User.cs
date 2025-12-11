namespace Domain.Entities;

public class User : EntityBase
{
   public string Name { get; private set; } = string.Empty;
   public string Email { get; private set; } = string.Empty;
   public string Password { get; private set; } = string.Empty;
   public ICollection<TenantUser>? TenantUsers { get; private set; }
   public class Builder
    {
        private readonly User _user = new();

        public Builder SetName(string name)
        {
            _user.Name = name;
            return this;
        }

        public Builder SetEmail(string email)
        {
            _user.Email = email;
            return this;
        }

        public Builder SetPassword(string password)
        {
            _user.Password = password;
            return this;
        }

        public User Build()
        {
            return _user;
        }
    }
}