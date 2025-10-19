using CsBases.Fundamentals;

public class ProductRepository
{
    // public Product GetProduct(int id)
    // {
    //     // Synchronous method
    //     return new Product("Simulated product", 200);
    // }

    public async Task<Product> GetProduct(int id)
    {
        // we use asynchronous calls when we get data from a DB, from an external API, or maybe from a file

        // simulating a wait
        WriteLine("Looking for the product...");
        // the use of "await" is because we need to wait for the result and not continue with the execution of the method
        await Task.Delay(2000);
        
        return new Product("Simulated product", 200);
    }
}