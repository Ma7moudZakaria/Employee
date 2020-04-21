using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Department Must Has a Name"), Column(TypeName = "NVARCHAR(30)")]
        public string Name { get; set; }

        public ICollection<Employees> Employees { get; set; }
    }
}
