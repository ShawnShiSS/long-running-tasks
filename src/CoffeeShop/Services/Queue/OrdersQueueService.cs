using CoffeeShop.Interfaces;
using System.Threading.Channels;

namespace CoffeeShop.Services
{
    /// <summary>
    ///     Orders queue implementation using a in-process channel.
    /// </summary>
    public class OrdersQueueService : IOrdersQueue
    {
        private readonly Channel<OrderItem> _queue;

        public OrdersQueueService()
        {
            // Note: capacity can also be set using IOptions using DI
            var options = new BoundedChannelOptions(1000)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _queue = Channel.CreateBounded<OrderItem>(options);
        }

        public async ValueTask EnqueueAsync(OrderItem workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            await _queue.Writer.WriteAsync(workItem);
        }

        public async ValueTask<OrderItem> DequeueAsync(CancellationToken cancellationToken)
        {
            var workItem = await _queue.Reader.ReadAsync(cancellationToken);

            return workItem;
        }
    }
}
