namespace Data.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_TB
    {
        [Key]
        [StringLength(10)]
        public string Ma_TB { get; set; }

        [Required]
        [StringLength(10)]
        public string Ma_NCC { get; set; }

        [Required]
        [StringLength(70)]
        public string Ten_TB { get; set; }

        [StringLength(50)]
        public string Cong_Suat { get; set; }
    }
}
