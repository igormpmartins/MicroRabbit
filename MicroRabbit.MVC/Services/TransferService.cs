using MicroRabbit.MVC.Models.DTO;
using Newtonsoft.Json;
using System.Text;

namespace MicroRabbit.MVC.Services
{
    public class TransferService : ITransferService
    {
        private readonly HttpClient httpClient;

        public TransferService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task Transfer(TransferDTO transferDTO)
        {
            var uri = "https://localhost:7037/banking";
            var content = new StringContent(
                JsonConvert.SerializeObject(transferDTO),
                Encoding.UTF8,
                "application/json");

            var response = await httpClient.PostAsync(uri, content);
            response.EnsureSuccessStatusCode();
        }
    }
}
