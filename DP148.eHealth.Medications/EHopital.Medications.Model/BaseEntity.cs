using System.ComponentModel.DataAnnotations;

namespace EHospital.Medications.Model
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }
    }
}