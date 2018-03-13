namespace Data.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_DailyDetail
    {
        public long Id { get; set; }

        public TimeSpan FormTime { get; set; }

        public TimeSpan ToTime { get; set; }

        [Column(TypeName = "ntext")]
        public string Comment1 { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string Content_Job { get; set; }

        [Column(TypeName = "ntext")]
        public string Method { get; set; }

        [StringLength(250)]
        public string Result { get; set; }

        public int DailyId { get; set; }

        public int JobId { get; set; }

        public int? Level_1 { get; set; }

        public int? Level_2 { get; set; }

        [Column(TypeName = "ntext")]
        public string Comment2 { get; set; }

        public int? Level_3 { get; set; }

        [Column(TypeName = "ntext")]
        public string Comment3 { get; set; }

        public virtual tbl_Daily tbl_Daily { get; set; }
    }
}
