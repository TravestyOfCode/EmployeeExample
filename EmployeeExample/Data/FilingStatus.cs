using System.ComponentModel.DataAnnotations;

namespace EmployeeExample.Data
{
    public enum FilingStatus
    {
        [Display(Name = "Single")]
        Single = 1,

        [Display(Name = "Maried Filing Separately")]
        MarriedFilingSeparately = Single,

        [Display(Name = "Married Filing Jointly")]
        MarriedFilingJointly = 2,

        [Display(Name = "Head of Household")]
        HeadOfHousehold = 3
    };
}
