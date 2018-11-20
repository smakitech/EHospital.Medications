using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHospital.Medications.Model
{
    public class Image
    {
        public Image()
        {
            PatientInfo = new HashSet<PatientInfo>();
        }

        public int Id { get; set; }
        [StringLength(100)]
        public string ImageName { get; set; }
        public byte[] PatientImage { get; set; }

        [InverseProperty("Image")]
        public ICollection<PatientInfo> PatientInfo { get; set; }
    }
}
