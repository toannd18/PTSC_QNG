namespace Data.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Daily
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Daily()
        {
            tbl_DailyDetail = new HashSet<tbl_DailyDetail>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Column(TypeName = "ntext")]
        public string Total_Job { get; set; }

        [Column(TypeName = "ntext")]
        public string Comment1 { get; set; }

        [StringLength(50)]
        public string User_Autho1 { get; set; }

        [StringLength(50)]
        public string User_Autho2 { get; set; }

        public bool Status_Autho1 { get; set; }

        public bool Status_Autho2 { get; set; }

        [StringLength(50)]
        public string User_Autho3 { get; set; }

        public bool Status_Autho3 { get; set; }

        [Column(TypeName = "ntext")]
        public string Comment2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_DailyDetail> tbl_DailyDetail { get; set; }
    }
}
