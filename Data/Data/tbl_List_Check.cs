namespace Data.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_List_Check
    {
        public long Id { get; set; }

        public int RequestId { get; set; }

        [Required]
        [StringLength(10)]
        public string Ma_NCC { get; set; }

        [Required]
        [StringLength(50)]
        public string Ma_TB { get; set; }

      
        public string YC_KT { get; set; }

       
        public string TT_KT { get; set; }

        public int YC_SL { get; set; }

        public int TT_SL { get; set; }

        [Required]
        [StringLength(20)]
        public string DonVi { get; set; }

        public bool? CO { get; set; }

        public bool? CQ { get; set; }

        public bool? MTR { get; set; }

        public bool? SN { get; set; }

        public bool? PN { get; set; }

        public bool? Other { get; set; }

        [StringLength(200)]
        public string Note_Other { get; set; }

        public bool? Result { get; set; }

        public string Reason { get; set; }

        [StringLength(50)]
        public string User_Nhap { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_Nhap { get; set; }

        [StringLength(50)]
        public string User_Edit { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_Edit { get; set; }
    }
}
