using System.ComponentModel.DataAnnotations;

namespace EHospital.Medications.Model
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }
    }
}