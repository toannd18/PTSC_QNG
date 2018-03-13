namespace Data.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Job
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Ten_Job { get; set; }

        [Required]
        [StringLength(10)]
        public string Ma_BP { get; set; }

        [StringLength(10)]
        public string Ma_TO { get; set; }

        public virtual tbl_BP tbl_BP { get; set; }
    }
}
