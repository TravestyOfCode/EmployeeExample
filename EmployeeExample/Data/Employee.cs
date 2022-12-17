using System.ComponentModel.DataAnnotations;

namespace EmployeeExample.Data
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(96)]
        public string FirstName { get; set; }

        [MaxLength(64)]
        public string MiddleName { get; set; }

        [Required]
        [MaxLength(96)]
        public string LastName { get; set; }

        [MaxLength(256)]
        public string Address { get; set; }

        [MaxLength(64)]
        public string City { get; set; }

        [MaxLength(16)]
        public string State { get; set; }

        [MaxLength(12)]
        public string ZipCode { get; set; }

        [MaxLength(1024)]
        public string WorkEmail { get; set; }

        [MaxLength(32)]
        public string WorkPhone { get; set; }

        [MaxLength(1024)]
        public string PersonalEmail { get; set; }

        [MaxLength(32)]
        public string PersonalPhone { get; set; }

        [MaxLength(16)]
        public string SocialSecurityNumber { get; set; }

        public FilingStatus FilingStatus { get; set; }

        public bool HasTwoJobs { get; set; }

        [Range(0, int.MaxValue)]
        public int ClaimDependantAndOtherCreditsAmount { get; set; }

        [Range(0, int.MaxValue)]
        public int OtherIncomeAmount { get; set; }

        [Range(0, int.MaxValue)]
        public int DeductionAmount { get; set; }

        [Range(0, int.MaxValue)]
        public int ExtraWitholdingAmount { get; set; }
    }
}