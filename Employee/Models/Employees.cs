using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.Models
{
    public class Employees
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee Must Has a First Name") , Column(TypeName = "NVARCHAR(30)")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Employee Must Has a Last Name"), Column(TypeName = "NVARCHAR(30)")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Employee Must Has an Email"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Employee Must Has an Email"), DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Employee Must Has an Email"), Range(18 , 60)]
        public int Age { get; set; }

        #region The Relation Between Employee and Department
        public int DepartmentIDFK { get; set; }

        [ForeignKey(nameof(DepartmentIDFK))]
        public Department DepartmentFK { get; set; }
        #endregion
    }
}
