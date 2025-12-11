using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Data;

    public static class DependenceInjection
    {
        public static void AddDataBase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<UpContext>(options =>
                options.UseNpgsql(connectionString));
        }
    }
