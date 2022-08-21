using MicroRabbit.MVC.Models;
using MicroRabbit.MVC.Models.DTO;
using MicroRabbit.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MicroRabbit.MVC.Controllers
{
    public class BankingController : Controller
    {
        private readonly ITransferService transferService;

        public BankingController(ITransferService transferService)
        {
            this.transferService = transferService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(TransferViewModel model)
        {
            var transferDto = new TransferDTO
            {
                FromAccount = model.FromAccount,
                ToAccount = model.ToAccount,
                TransferAmount = model.TransferAmount
            };

            await transferService.Transfer(transferDto);

            return View(nameof(Index));
        }

    }
}