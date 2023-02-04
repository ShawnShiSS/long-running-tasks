using CoffeeShop.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CoffeeShop.Services
{
    /// <summary>
    ///     A coffee maker service that runs in the background.
    /// </summary>
    public class CoffeeMakerService : BackgroundService
    {
        public IOrdersQueue _ordersQueue { get; }
        private readonly ILogger<CoffeeMakerService> _logger;

        public CoffeeMakerService(IOrdersQueue ordersQueue, 
                                  ILogger<CoffeeMakerService> logger)
        {
            _ordersQueue = ordersQueue;
            _logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{DateTime.UtcNow}: Coffee Maker Service is running.");

            await BackgroundProcessing(stoppingToken);
        }

        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await _ordersQueue.DequeueAsync(stoppingToken);

                try
                {
                    // Make coffee
                    string customer = workItem.Customer;
                    await workItem.HowToMakeCoffee(customer, stoppingToken);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing {WorkItem}.", nameof(workItem));
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{DateTime.UtcNow}: Coffee Maker Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
