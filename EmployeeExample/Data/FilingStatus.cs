using System.ComponentModel.DataAnnotations;

namespace EmployeeExample.Data
{
    public enum FilingStatus
    {
        [Display(Name = "Single or Maried Filing Separately")]
        SingleOrMarriedFilingSeparately = 1,

        [Display(Name = "Married Filing Jointly")]
        MarriedFilingJointly = 2,

        [Display(Name = "Head of Household")]
        HeadOfHousehold = 3
    };
}
