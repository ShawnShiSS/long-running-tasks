namespace CoffeeShop.Services
{
    /// <summary>
    ///     Order item that can be put into a queue
    /// </summary>
    public class OrderItem
    { 
        /// <summary>
        ///     A func that defines how the coffee should be made
        /// </summary>
        public Func<string, CancellationToken, ValueTask> HowToMakeCoffee { get; set; }

        /// <summary>
        ///     For whom the coffee should be made
        /// </summary>
        public string Customer { get; set; }
    }
}
