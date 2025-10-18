class Program
{
    static void Main()
    {
        // explicit declaration
        int quantity = 3;
        string message = "Hello world!";
        decimal price = 19.99m;

        WriteLine($"Quantity: {quantity}, Message: {message}, Price: {price:C}");

        // var declaration: it'll infer the type
        var greeting = "hey";
        var percentage = 20.00m;

        WriteLine($"Greeting: {greeting}, Percentage: {percentage}");
    }
}