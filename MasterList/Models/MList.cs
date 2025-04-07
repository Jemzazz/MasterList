using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterList.Models
{
    public class MList
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name= "First Name")]
        public string FirstName     { get; set; }


        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name ="Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name ="Category")]
        public string Category { get; set; }

        [Required]
        [Display(Name ="Location Code")]
        public string LocationCode { get; set; }

        [Required]
        [Display(Name ="Position")]
        public string? Position { get; set; }

        [Display(Name = "School")]
        public string? School { get; set; }
    }
}
