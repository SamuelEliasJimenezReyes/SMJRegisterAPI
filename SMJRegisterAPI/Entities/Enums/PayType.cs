using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMJRegisterAPI.Entities.Enums;

public enum PayType
{
    [Description("Mediante Directivo")] 
    [Display(Name = "Mediante Directivo")]
    Cash = 1 ,
    
    [Description("Transferencia Bancaria")] 
    [Display(Name = "Transferencia Bancaria")]
    Transfer
}