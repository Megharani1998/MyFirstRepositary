using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectModels
{
    public class Contacts
    {

        [Key]
        public int Id { get; set; }
        [Display(Name ="First Name")]
        [Required(ErrorMessage ="First Name is Required")]
        [StringLength(ContactManagerConstants.MAX_FIRST_NAME_LENGTH)]
        public String FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is Required")]
        [StringLength(ContactManagerConstants.MAX_LAST_NAME_LENGTH)]
        public String LastName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is Required")]
        [StringLength(ContactManagerConstants.MAX_EMAIL_LENGTH)]
        [EmailAddress(ErrorMessage ="Invalid Email Address")]
        public string Email { get; set; }
        [Display(Name = " Mobile Phone")]
        [Required(ErrorMessage = "Phone Number is Required")]
        [StringLength(ContactManagerConstants.MAX_PHONE_LENGTH)]
        public string PhonePrimary { get; set; }

        [Display(Name = " Home Phone")]
        [Required(ErrorMessage = "Phone Number is Required")]
        [StringLength(ContactManagerConstants.MAX_PHONE_LENGTH)]
        public string PhoneSecondary { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [Display(Name ="Street Address Line1")]
        [StringLength(ContactManagerConstants.MAX_STREET_ADDRESS_LENGTH)]
        public string StreetAddress1 { get; set; }

        [Display(Name = "Street Address Line2")]
        [StringLength(ContactManagerConstants.MAX_STREET_ADDRESS_LENGTH)]
        public string StreetAddress2 { get; set; }

        [Required(ErrorMessage ="City is Required")]
        [StringLength(ContactManagerConstants.MAX_CITY_LENGTH)]
        public string City { get; set; }
        [Display(Name ="State")]
        [Required(ErrorMessage ="State is required")]
        public int StateId { get; set; }
        public virtual State State { get; set; }
        [Display(Name="Zip Code")]
        [Required(ErrorMessage ="Zip code is required")]
        [StringLength(ContactManagerConstants.MAX_ZIP_CODE_LENGTH, MinimumLength = ContactManagerConstants.MIN_ZIP_CODE_LENGTH)]
        [RegularExpression("^(?i)(\\d{5}(-\\d{4})?|[A-CEGHJ-NPRSTVXY]\\d[A-CEGHJ-NPRSTV-Z] ?\\d[A-CEGHJ-NPRSTV-Z]\\d)$\r\n",ErrorMessage ="Zip code is Invalid!")]
        public string Zip { get; set; }

        [Required(ErrorMessage ="The UserIs is required in order to map the contact to a user correctly")]
        public string UserId { get; set; }

        [Display(Name ="Full Name")]
        public string FirendlyName => $"{FirstName} {LastName}";
        [Display(Name = "Full Address")]
        public string FriendlyAddress => State is null ? "" : string.IsNullOrWhiteSpace(StreetAddress1) ? $"{City}, {State.Abbreviation}, {Zip}" :
            string.IsNullOrWhiteSpace(StreetAddress2)
            ? $"{StreetAddress1},{City}, {State.Abbreviation},{Zip}"
            : $"{StreetAddress1}-{StreetAddress2},{City},{State.Abbreviation},{Zip}";



    }
}
