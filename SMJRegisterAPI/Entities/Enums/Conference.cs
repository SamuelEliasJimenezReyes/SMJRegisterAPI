using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMJRegisterAPI.Entities.Enums;

public enum Conference
{
    [Description("Noreste")] 
    [Display(Name = "Noreste")]
    Northeast=1,
    
    [Description("Sureste")] 
    [Display(Name = "Sureste")]
    Southeast,
    
    [Description("Central")] 
    [Display(Name = "Central")]
    Central
}