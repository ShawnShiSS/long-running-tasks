using CoffeeShop.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CoffeeShop.Services
{
    /// <summary>
    ///     Front desk service, which is a SignalR hub to handle realtime communication
    /// </summary>
    public class FrontdeskService : Hub<ICoffeeClient>
    {
        public IOrdersQueue _ordersQueue { get; }
        private readonly ILogger<FrontdeskService> _logger;

        public FrontdeskService(IOrdersQueue ordersQueue,
            ILogger<FrontdeskService> logger)
        {
            _ordersQueue = ordersQueue;
            _logger = logger;
        }

        /// <summary>
        ///     Hub method to accept requests to make coffee
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task MakeCoffee(string user)
        {
            // Announce that the order is received
            await Clients.All.ReceiveMessage(user, $"Can I get a coffee please? {DateTime.UtcNow}");

            _logger.LogInformation($"{DateTime.UtcNow}: Make coffee request received from {user}.");

            // Add order to the queue
            await _ordersQueue.EnqueueAsync(new OrderItem()
                {
                    HowToMakeCoffee = BuildMakeCoffeeTask,
                    Customer = user
                } 
            );
            
            // Announce that the order is received
            await Clients.All.ReceiveMessage("Front desk", $"Hi {user}, your order is received. {DateTime.UtcNow}");

        }


        #region Helpers
        private async ValueTask BuildMakeCoffeeTask(string user, CancellationToken cancellationToken)
        {
            // Simulate a 3 seconds tasks to make coffee
            _logger.LogInformation($"Coffee for user {user} is being brewed...");

            // Announce order status
            await Clients.All.ReceiveMessage("Front desk", $"Hi {user}, we are making your coffee now. {DateTime.UtcNow}");

            await Task.Delay(TimeSpan.FromSeconds(3));

            _logger.LogInformation($"Coffee for user {user} is ready...");

            await Clients.All.ReceiveMessage("Front desk", $"Hi {user}, your coffee is ready. {DateTime.UtcNow}");

        }
        #endregion
    }
}
