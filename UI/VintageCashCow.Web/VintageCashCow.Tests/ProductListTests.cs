using NUnit.Framework;
using System.Collections.Generic;
using VintageCashCow.Client.Models;

namespace VintageCashCow.Tests;

[TestFixture]
public class ProductListTests
{
    [Test]
    public void ProductList_CanBeInstantiated()
    {
        // Act
        var comp = new Client.Shared.ProductList();

        // Assert
        Assert.That(comp, Is.Not.Null);
        Assert.That(comp.Products, Is.Not.Null);
    }

    [Test]
    public void ProductList_ProductsDefaultsToEmptyList()
    {
        // Act
        var comp = new Client.Shared.ProductList();

        // Assert
        Assert.That(comp.Products.Count, Is.EqualTo(0));
    }

    [Test]
    public void ProductList_CanSetProducts()
    {
        // Arrange
        var comp = new Client.Shared.ProductList();
        var products = new List<ProductViewModel>
        {
            new() { Id = 1, Name = "Product1", Price = 10m },
            new() { Id = 2, Name = "Product2", Price = 20m }
        };

        // Act
        comp.Products = products;

        // Assert
        Assert.That(comp.Products.Count, Is.EqualTo(2));
        Assert.That(comp.Products[0].Name, Is.EqualTo("Product1"));
    }
}
