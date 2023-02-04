using CoffeeShop.Services;

namespace CoffeeShop.Interfaces
{
    public interface IOrdersQueue
    {
        /// <summary>
        ///     Add a background work item to the queue.
        /// </summary>
        /// <param name="workItem"></param>
        /// <returns></returns>
        ValueTask EnqueueAsync(OrderItem workItem);

        /// <summary>
        ///     Remove a background work item from the queue.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask<OrderItem> DequeueAsync(CancellationToken cancellationToken);
    }
}
