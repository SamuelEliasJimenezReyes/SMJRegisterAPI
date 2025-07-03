using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMJRegisterAPI.Entities.Enums;

public enum Gender
{
    [Description("Hombre")] 
    [Display(Name = "Hombre")]
    Male = 1 ,
    
    [Description("Mujer")] 
    [Display(Name = "Mujer")]
    Female
}