
using System.ComponentModel.DataAnnotations;

namespace MafiaScenarios.GodFather.V1._00.Models
{
    public class GetPlayerNamesVM
    {
        [Required(ErrorMessage = "وارد کردن این فیلد الزامی است")]
        [Display(Name = "نام")]
        public string Name { get; set; } = string.Empty;
    }
}
