using System;
using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Models.Dtos;

public class CreateCategoryDto
{
    [Required(ErrorMessage ="The name is mandatory.")]
    [MaxLength(50, ErrorMessage ="Name cannot contain more than 50 characters.")]
    [MinLength(3, ErrorMessage ="Name cannot contain less than 3 characters.")]
    public string Name { get; set; } = string.Empty;
}
