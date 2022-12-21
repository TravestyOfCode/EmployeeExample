using System.ComponentModel.DataAnnotations;

namespace EmployeeExample.Models.ViewModels.Employee
{
    public class IndexViewModel
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(1024)]
        [Display(Name = "Work Email")]
        public string WorkEmail { get; set; }

        [MaxLength(32)]
        [Display(Name = "Work Phone")]
        public string WorkPhone { get; set; }
        
        public bool CanDelete { get; set; }

        public bool CanEdit { get; set; }
    }
}
