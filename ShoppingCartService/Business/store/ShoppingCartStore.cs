using ShoppingCartService.Business.domain;

namespace ShoppingCartService.Business.store;

public interface IShoppingCartStore
{
    ShoppingCart Get(int userId);
    void Save(ShoppingCart shoppingCart);
    void Init();
}

