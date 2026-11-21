using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models ;
public class Category{
    public int CategoryID {get; set;} 
    [Required]
    [StringLength(40, 
    ErrorMessage ="Name must have at least two characters and at most 40 ones",
     MinimumLength =2)]
    public required  string Name {get; set;}
    public List<Product> Products{get;} = new();
     public async Task<CategoryResponseDto> ToResponseAsync()
    {
        return new CategoryResponseDto
        {
          CategoryID = CategoryID,
          Name = Name,
          Products = Products.Select( e => e.ToResponse()).ToList()
        };
    }
 
}
public class CategoryResponseDto{
    public int CategoryID {get; set;} 
    [Required]
    [StringLength(40, 
    ErrorMessage ="Name must have at least two characters and at most 40 ones",
     MinimumLength =2)]
    public required  string Name {get; set;}
    public List<ProductResponseDto> Products{get; set;} = new();
    
 
}
public class CategoryRequestDto{
    public int CategoryID {get;} = 1;
    [Required]
    [StringLength(40, 
    ErrorMessage ="Name must have at least two characters and at most 40 ones",
     MinimumLength =2)]
    public required  string Name {get; set;}
    public List<ProductResponseDto> Products{get;} = new();
     public async Task<Category> ToCategoryAsync()
    {
        return new Category
        {
          Name = Name
        };
    }
 
}