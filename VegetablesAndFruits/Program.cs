using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Color { get; set; }
    public int Calories { get; set; }
}

public class VegetablesAndFruitsApp
{
    private string connectionString;

    public VegetablesAndFruitsApp(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public Product FindProductByName(string name)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var command = new SqlCommand("SELECT * FROM VegetablesAndFruits WHERE Name = @name", connection);
            command.Parameters.AddWithValue("@name", name);

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Product
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Type = (string)reader["Type"],
                    Color = (string)reader["Color"],
                    Calories = (int)reader["Calories"],
                };
            }

            return null;
        }
    }

    public List<Product> GetAllProducts()
    {
        var products = new List<Product>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var command = new SqlCommand("SELECT * FROM VegetablesAndFruits", connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                products.Add(new Product
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Type = (string)reader["Type"],
                    Color = (string)reader["Color"],
                    Calories = (int)reader["Calories"],
                });
            }
        }

        return products;
    }

    public void UpdateProduct(Product product)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var command = new SqlCommand("UPDATE VegetablesAndFruits SET Name = @name, Type = @type, Color = @color, Calories = @calories WHERE Id = @id", connection);
            command.Parameters.AddWithValue("@id", product.Id);
            command.Parameters.AddWithValue("@name", product.Name);
            command.Parameters.AddWithValue("@type", product.Type);
            command.Parameters.AddWithValue("@color", product.Color);
            command.Parameters.AddWithValue("@calories", product.Calories);

            command.ExecuteNonQuery();
        }
    }

    public void DeleteProduct(int id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var command = new SqlCommand("DELETE FROM VegetablesAndFruits WHERE Id = @id", connection);
            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
        }
    }

    public double GetAverageCalories()
    {
        double averageCalories = 0;
        int productCount = 0;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var command = new SqlCommand("SELECT AVG(Calories) FROM VegetablesAndFruits", connection);

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                averageCalories = (double)reader[0];
            }

            command = new SqlCommand("SELECT COUNT(*) FROM VegetablesAndFruits", connection);

            reader = command.ExecuteReader();

            if (reader.Read())
            {
                productCount = (int)reader[0];
            }
        }

        return averageCalories / productCount;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        string connectionString = "Data Source=LAPTOP-8HJ85GUS\\ODESI;Initial Catalog=VegetablesAndFruits;Integrated Security=True;";
        var vegetablesAndFruitsApp = new VegetablesAndFruitsApp(connectionString);

        Product apple = vegetablesAndFruitsApp.FindProductByName("Яблоко");
        if (apple != null)
        {
            Console.WriteLine($"Знайдено продукт: {apple.Name} ({apple.Type}), Колір: {apple.Color}, Калорійність: {apple.Calories}");
        }
        else
        {
            Console.WriteLine("Продукт не знайдено");
        }

        List<Product> allProducts = vegetablesAndFruitsApp.GetAllProducts();
        foreach (var product in allProducts)
        {
            Console.WriteLine($"{product.Id} - {product.Name} ({product.Type}), Колір: {product.Color}, Калорійність: {product.Calories}");
        }

        apple.Calories = 55;
        vegetablesAndFruitsApp.UpdateProduct(apple);

        vegetablesAndFruitsApp.DeleteProduct(3);

        double averageCalories = vegetablesAndFruitsApp.GetAverageCalories();
        Console.WriteLine($"Середня калорійність: {averageCalories}");
    }
}

