using AutoMapper;
using Clinic.Caching.Interfaces;
using Clinic.Common.Models;
using Clinic.Common.Options;
using Clinic.Common.Validator;
using Clinic.Data.Models;
using Clinic.DTO.Models;
using Clinic.Invoice.Contracts;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Clinic.Invoice.Services
{
    public class InvoiceService : IInvoiceService
    {
        private const string ContentType = "application/json";
        private readonly IOptions<ApplicationSettings> applicationSettings;
        private readonly HttpClient httpClient;
        private readonly IMapper autoMapper;
        private readonly IDistributedCacheService cacheService;
        public InvoiceService(IHttpClientFactory httpClientFactory, IOptions<ApplicationSettings> applicationSettings, IMapper autoMapper, IDistributedCacheService cacheService)
        {
            ArgumentValidation.ThrowIfNull(applicationSettings, nameof(applicationSettings));
            httpClient = httpClientFactory.CreateClient();
            this.applicationSettings = applicationSettings;
            this.autoMapper = autoMapper;
            this.cacheService = cacheService;
        }
        public async Task<InvoiceDetailsViewModel> AddInvoiceAsync(InvoiceDetailsViewModel invoice)
        {
            ArgumentValidation.ThrowIfNull(invoice, nameof(invoice));
            invoice.Id = Guid.NewGuid().ToString();
            invoice.SoldBy = new SoldByViewModel()
            {
                Email = "",
                Phone = "",
                ManagerName = "Derek"
            };
            var invoiceEntity = autoMapper.Map<Data.Models.Invoice>(invoice);
            using var invoiceRequest = new StringContent(JsonSerializer.Serialize(invoiceEntity),Encoding.UTF8,ContentType);
            var invoiceResponse = await httpClient.PostAsync(new Uri($"{applicationSettings.Value.DataStoreEndpoint}getinvoice"), invoiceRequest).ConfigureAwait(false);

            if(!invoiceResponse.IsSuccessStatusCode)
            {
                await ThrowServiceToServiceErrors(invoiceResponse).ConfigureAwait(false);
            }
            var createdInvoiceDAO = await invoiceResponse.Content.ReadFromJsonAsync<Clinic.Data.Models.Invoice>().ConfigureAwait(false);
            var createdInvoice = autoMapper.Map<InvoiceDetailsViewModel>(createdInvoiceDAO);
            return createdInvoice;
        }

        public async Task<InvoiceDetailsViewModel> GetInvoiceByIdAsync(string invoiceId)
        {
            using var invoiceRequest = new HttpRequestMessage(HttpMethod.Get, $"{applicationSettings.Value.DataStoreEndpoint}getinvoice/{invoiceId}");
            var invoiceResponse = await httpClient.SendAsync(invoiceRequest).ConfigureAwait(false);
            if(!invoiceResponse.IsSuccessStatusCode)
            {
                await ThrowServiceToServiceErrors(invoiceResponse).ConfigureAwait(false);
            }
            if(invoiceResponse.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                var invoiceDAO = await invoiceResponse.Content.ReadFromJsonAsync<Clinic.Data.Models.Invoice>().ConfigureAwait(false);

                var invoice = autoMapper.Map<InvoiceDetailsViewModel>(invoiceDAO);
                return invoice;
            }
            else
            {
                return null;
            }
        }

        private async Task ThrowServiceToServiceErrors(HttpResponseMessage response)
        {
            var exceptionReponse = await response.Content.ReadFromJsonAsync<ExceptionResponse>().ConfigureAwait(false);
            throw new Exception(exceptionReponse.InnerException);
        }
    }
}
