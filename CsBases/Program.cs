using System.Threading.Tasks;
using CsBases.Fundamentals;

class Program
{
    static async Task Main()
    {
        // ---------------------
        var laptop = new Product("Laptop", 1200);
        WriteLine(laptop.GetDescription());

        var support = new ServiceProduct("Technical support", 300, 30);
        WriteLine(support.GetDescription());

        // adapter pattern
        WriteLine("\n\tAdapter pattern");
        var product = new Product("Mouse Gamer", 300);
        var productDto = ProductAdapter.ToDto(product);

        WriteLine($"{productDto.Name} - {productDto.Price:C} - Code: {productDto.Code}");

        // dependency injection
        WriteLine("\n\tDependency injection");
        ILabelService labelService = new LabelService();
        var manager = new ProductManager(labelService);

        var monitor = new Product("Monitor", 100);
        var installation = new ServiceProduct("Monitor installation", 20, 30);

        manager.PrintLabel(monitor);
        manager.PrintLabel(installation);

        // asynchronous methods
        WriteLine("\n\tAsynchronous methods");
        var firstProduct = await new ProductRepository().GetProduct(1);
        firstProduct.Description = "This is the description of the first product";

        // this line applies the uppercase transformation for the object according to the annotations that were made on the class
        AttributeProcessor.ApplyUpperCase(firstProduct);
        WriteLine($"{firstProduct.Name} - {firstProduct.Price} - {firstProduct.Description}");
    }
}