using System;

// 1. Shipping strategy interface
public interface IShippingStrategy
{
    decimal CalculateShippingCost(decimal weight, decimal distance);
    string GetStrategyName();
}

// 2. Standard shipping strategy
public class StandardShippingStrategy : IShippingStrategy
{
    public decimal CalculateShippingCost(decimal weight, decimal distance)
    {
        return weight * 0.5m + distance * 0.1m;
    }
    
    public string GetStrategyName()
    {
        return "Standard Shipping";
    }
}

// 3. Express shipping strategy
public class ExpressShippingStrategy : IShippingStrategy
{
    public decimal CalculateShippingCost(decimal weight, decimal distance)
    {
        return (weight * 0.75m + distance * 0.2m) + 10; // Additional fee for speed
    }
    
    public string GetStrategyName()
    {
        return "Express Shipping";
    }
}

// 4. International shipping strategy
public class InternationalShippingStrategy : IShippingStrategy
{
    public decimal CalculateShippingCost(decimal weight, decimal distance)
    {
        return weight * 1.0m + distance * 0.5m + 15; // Additional fees for international shipping
    }
    
    public string GetStrategyName()
    {
        return "International Shipping";
    }
}

// 5. Night shipping strategy (extended functionality)
public class NightShippingStrategy : IShippingStrategy
{
    public decimal CalculateShippingCost(decimal weight, decimal distance)
    {
        return (weight * 0.8m + distance * 0.3m) + 25; // Additional fee for night delivery
    }
    
    public string GetStrategyName()
    {
        return "Night Shipping";
    }
}

// 6. Delivery context class
public class DeliveryContext
{
    private IShippingStrategy _shippingStrategy;

    // Method to set shipping strategy
    public void SetShippingStrategy(IShippingStrategy strategy)
    {
        _shippingStrategy = strategy;
    }

    // Method to calculate shipping cost with input validation
    public decimal CalculateCost(decimal weight, decimal distance)
    {
        if (_shippingStrategy == null)
        {
            throw new InvalidOperationException("Shipping strategy is not set.");
        }

        // Validate input data
        ValidateInput(weight, distance);

        return _shippingStrategy.CalculateShippingCost(weight, distance);
    }

    // Method to get information about current strategy
    public string GetCurrentStrategyInfo()
    {
        return _shippingStrategy?.GetStrategyName() ?? "Strategy not set";
    }

    // Input validation
    private void ValidateInput(decimal weight, decimal distance)
    {
        if (weight <= 0)
        {
            throw new ArgumentException("Weight must be a positive number.");
        }
        
        if (distance <= 0)
        {
            throw new ArgumentException("Distance must be a positive number.");
        }
        
        if (weight > 1000)
        {
            throw new ArgumentException("Weight cannot exceed 1000 kg.");
        }
        
        if (distance > 50000)
        {
            throw new ArgumentException("Distance cannot exceed 50000 km.");
        }
    }
}

// 7. Client code
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Shipping Cost Calculator ===");
        
        DeliveryContext deliveryContext = new DeliveryContext();
        bool continueCalculation = true;

        while (continueCalculation)
        {
            try
            {
                Console.WriteLine("\nSelect delivery type:");
                Console.WriteLine("1 - Standard Shipping");
                Console.WriteLine("2 - Express Shipping");
                Console.WriteLine("3 - International Shipping");
                Console.WriteLine("4 - Night Shipping");
                Console.WriteLine("0 - Exit");
                Console.Write("Your choice: ");

                string choice = Console.ReadLine();

                if (choice == "0")
                {
                    continueCalculation = false;
                    Console.WriteLine("Goodbye!");
                    continue;
                }

                // Set strategy based on user choice
                switch (choice)
                {
                    case "1":
                        deliveryContext.SetShippingStrategy(new StandardShippingStrategy());
                        break;
                    case "2":
                        deliveryContext.SetShippingStrategy(new ExpressShippingStrategy());
                        break;
                    case "3":
                        deliveryContext.SetShippingStrategy(new InternationalShippingStrategy());
                        break;
                    case "4":
                        deliveryContext.SetShippingStrategy(new NightShippingStrategy());
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please select from 0 to 4.");
                        continue;
                }

                Console.WriteLine($"\nSelected: {deliveryContext.GetCurrentStrategyInfo()}");

                // Input weight
                decimal weight = GetValidatedDecimalInput("Enter package weight (kg): ", 0.1m, 1000m);

                // Input distance
                decimal distance = GetValidatedDecimalInput("Enter delivery distance (km): ", 1m, 50000m);

                // Calculate cost
                decimal cost = deliveryContext.CalculateCost(weight, distance);
                Console.WriteLine($"\nCalculation Result:");
                Console.WriteLine($"Delivery Type: {deliveryContext.GetCurrentStrategyInfo()}");
                Console.WriteLine($"Weight: {weight} kg");
                Console.WriteLine($"Distance: {distance} km");
                Console.WriteLine($"Shipping Cost: {cost:C}");

                // Ask to continue
                Console.Write("\nWould you like to perform another calculation? (y/n): ");
                string continueChoice = Console.ReadLine().ToLower();
                if (continueChoice != "y" && continueChoice != "yes")
                {
                    continueCalculation = false;
                    Console.WriteLine("Goodbye!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("Please try again.");
            }
        }
    }

    // Method to get validated numeric input
    private static decimal GetValidatedDecimalInput(string prompt, decimal minValue, decimal maxValue)
    {
        while (true)
        {
            try
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                
                // Check for empty input
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty. Please try again.");
                    continue;
                }

                decimal value = Convert.ToDecimal(input);
                
                if (value < minValue)
                {
                    Console.WriteLine($"Value cannot be less than {minValue}. Please try again.");
                    continue;
                }
                
                if (value > maxValue)
                {
                    Console.WriteLine($"Value cannot be greater than {maxValue}. Please try again.");
                    continue;
                }
                
                return value;
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid number format. Please enter a numeric value.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("The number entered is too large. Please enter a smaller value.");
            }
        }
    }
}
