using Clinic.Data.Store;
using Clinic.Data.Store.Contracts;

namespace Clinic.DataAccess.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
