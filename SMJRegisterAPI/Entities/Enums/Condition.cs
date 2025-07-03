using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMJRegisterAPI.Entities.Enums;

public enum Condition
{
    [Description("Campista")] 
    [Display(Name = "Campista")]
    RegularCamper=1,
    
    [Description("Staff")] 
    [Display(Name = "Staff")]
    Staff,
    
    [Description("Directivo")] 
    [Display(Name = "Directivo")]
    Executive,
}