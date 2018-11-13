using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DP148.eHealth.API.Medications.Domain.Models
{
    public partial class Images
    {
        public Images()
        {
            PatientInfo = new HashSet<PatientInfo>();
        }

        [Key]
        public int ImageId { get; set; }
        [StringLength(100)]
        public string ImageName { get; set; }
        public byte[] Image { get; set; }

        [InverseProperty("Image")]
        public ICollection<PatientInfo> PatientInfo { get; set; }
    }
}
