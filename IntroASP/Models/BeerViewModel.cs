using System.ComponentModel.DataAnnotations;

namespace IntroASP.Models;

public class BeerViewModel
{
    [Required]
    [Display(Name="Nombre")]
    public string name { get; set; }
    [Required]
    [Display(Name="Marca")]
    public int brandId {  get; set; }
}
