using System.Threading.Tasks;
using ShoppingCartService.Business.domain;

namespace ShoppingCartService.Business.store;

public interface IShoppingCartStore
{
    Task<ShoppingCart> Get(int userId);
    Task Save(ShoppingCart shoppingCart);
    // void Init();
}

