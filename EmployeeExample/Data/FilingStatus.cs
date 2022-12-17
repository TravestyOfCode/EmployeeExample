using System;

namespace EmployeeExample.Data
{
    [Flags]
    public enum FilingStatus
    {
        Single = 1,
        MarriedFilingSeparately = Single,
        MarriedFilingJointly = 2,
        HeadOfHousehold = 4
    };
}
