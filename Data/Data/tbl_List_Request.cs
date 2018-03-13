namespace Data.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_List_Request
    {
        public int Id { get; set; }

        public int FirstId { get; set; }

        [StringLength(70)]
        public string LateId { get; set; }

        [Required]
        [StringLength(10)]
        public string Ma_BP { get; set; }

        [StringLength(70)]
        public string Dia_Diem { get; set; }

        [StringLength(50)]
        public string Hang_Muc { get; set; }

        [StringLength(50)]
        public string HopDong { get; set; }

        [Required]
        [StringLength(50)]
        public string DeXuat { get; set; }

        public bool CO { get; set; }

        public bool CQ { get; set; }

        public bool Other { get; set; }

        [StringLength(50)]
        public string Note { get; set; }

        [Required]
        [StringLength(50)]
        public string User_Nhap { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date_Nhap { get; set; }

        [StringLength(50)]
        public string User_Edit { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_Edit { get; set; }

        [StringLength(50)]
        public string User_Autho { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_Autho { get; set; }

        [StringLength(1)]
        public string Status_Autho { get; set; }

        [StringLength(50)]
        public string Note_Autho { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }
    }
}
