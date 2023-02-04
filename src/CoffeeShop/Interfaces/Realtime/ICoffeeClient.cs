namespace CoffeeShop.Interfaces
{
    public interface ICoffeeClient
    {
        Task ReceiveMessage(string fromUser, string message);
    }
}
