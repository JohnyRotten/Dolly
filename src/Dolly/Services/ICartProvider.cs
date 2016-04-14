using Dolly.Models;

namespace Dolly.Services
{
    public interface ICartProvider
    {
        
        Cart Cart { get; }

        bool AddItem(Item item);

        bool RemoveItem(Item item);

    }
}