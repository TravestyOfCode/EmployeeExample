using System.ComponentModel.DataAnnotations;

namespace EmployeeExample.Data
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(96)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MaxLength(64)]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [MaxLength(96)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(256)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [MaxLength(64)]
        [Display(Name = "City")]
        public string City { get; set; }

        [MaxLength(16)]
        [Display(Name = "State")]
        public string State { get; set; }

        [MaxLength(12)]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [MaxLength(1024)]
        [Display(Name = "Work Email")]
        public string WorkEmail { get; set; }

        [MaxLength(32)]
        [Display(Name = "Work Phone")]
        public string WorkPhone { get; set; }

        [MaxLength(1024)]
        [Display(Name = "Personal Email")]
        public string PersonalEmail { get; set; }

        [MaxLength(32)]
        [Display(Name = "Personal Phone")]
        public string PersonalPhone { get; set; }

        [MaxLength(16)]
        [Display(Name = "SSN")]
        public string SocialSecurityNumber { get; set; }

        [Display(Name = "Filing Status")]
        public FilingStatus FilingStatus { get; set; }

        [Display(Name = "Has Two Jobs?")]
        public bool HasTwoJobs { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Claim Dependant / Other Credits")]
        public int ClaimDependantAndOtherCreditsAmount { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Other Income")]
        public int OtherIncomeAmount { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Deduction")]
        public int DeductionAmount { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Extra Witholding")]
        public int ExtraWitholdingAmount { get; set; }
    }
}