using System;
using ApiEcommerce.Models;

namespace ApiEcommerce.Repository.IRepository;

 interface IProductRepository
{
    ICollection<Product> GetProducts();
    ICollection<Product> GetProductsByCategory(int categoryId);
    ICollection<Product> SearchProduct(string name);
    Product? GetProduct(int id);
    bool BuyProduct(string name, int quantity);
    bool ProductExists(int id);
    bool ProductExists(string name);
    bool CreateProduct(Product product);
    bool UpdateProduct(Product product);
    bool DeleteProduct(Product product);
    bool Save(); 
}