using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models ;
public class Product{
    public int ProductID {get; set;}
    [Required]
    public required int CategoryID {get; set;}
    [Required(ErrorMessage = "name is required")]
    public required  string Name {get; set;}
    public required Guid Sku {get; set;} 
    public string? Description {get; set;}
    [Required(ErrorMessage = "price is required")]
    public required decimal Price {get; set;}
    [Required(ErrorMessage = "quantity is required")]
    public required int Quantity {get; set;}
    public Category Category{get;} = null!;
    public ProductResponseDto ToResponse()
    {
        return new ProductResponseDto
        {
            ProductID = ProductID,
            CategoryID = CategoryID,
            Name = Name,
            Description = Description,
            Price = Price,
            Quantity = Quantity,
            CategoryName = Category.Name,
            Sku = Sku
        };
    }
}
public class ProductResponseDto{
    public int ProductID {get; set;}
    [Required(ErrorMessage = "name is required")]
    public required  string Name {get; set;}
    public required int CategoryID {get; set;}
    public required string CategoryName { get; set;}
    public required Guid Sku {get;set;}
    public string? Description {get; set;}
    [Required(ErrorMessage = "price is required")]
    public required decimal Price {get; set;}
    [Required(ErrorMessage = "quantity is required")]
    public required int Quantity {get; set;}
    public ProductRequestDto ToRequest()
    {
        return new ProductRequestDto
        {
            Name = Name,
            Description = Description,
            Price = Price,
            Quantity = Quantity,
            CategoryID = CategoryID
        };
    }
}
public class ProductRequestDto{
    public int ProductID {get;} = 1;
    [Required]
    [Range(1,int.MaxValue)]
    public required int CategoryID {get; set;}

    [Required(ErrorMessage = "name is required")]
     [StringLength(40, MinimumLength = 2, ErrorMessage = "name must have at least 2 characters and at most 40 characters")]
    public required  string Name {get; set;}
    [StringLength(100, MinimumLength = 2, ErrorMessage = "description must have at least 2 characters and at most 100 characters")]
    public string? Description {get; set;}
    [Required(ErrorMessage = "price is required")]
    [Range(1.0, double.MaxValue, ErrorMessage ="price must be greater or equal to 1.0")]
    public required decimal Price {get; set;}
    [Required(ErrorMessage = "quantity is required")]
    [Range(1, int.MaxValue, ErrorMessage ="quantity must be greater or equal to 1")]
    public required int Quantity {get; set;}

    public Product ToProduct()
    {
        return new Product
        {   
            Name = Name,
            CategoryID = CategoryID,
            Description = Description,
            Price = Price,
            Quantity = Quantity,
            Sku = Guid.NewGuid()

        };
    }
}