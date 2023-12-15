using System.ComponentModel.DataAnnotations;

namespace projectModels
{
    internal class State
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "State")]
        [Required(ErrorMessage ="Name of State is Required")]
        [StringLength(ContactManagerConstants.MAX_STATE_NAME_LENGTH)]
        public String Name { get; set; }
        [Required(ErrorMessage ="State Abbrivation is Required")]
        [StringLength(ContactManagerConstants.MAX_STATE_ABBR_LENGTH)]
        public String Abbreviation { get; set; }
        
    }
}
