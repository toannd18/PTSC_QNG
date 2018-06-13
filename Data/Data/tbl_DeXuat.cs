using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace Data.Data
{
    public partial class tbl_DeXuat
    {
        [Key]
        public int Id { get; set; }
        public string Ma { get; set; }
        public string Tieu_De { get; set; }
        public bool Kieu { get; set; }
        public string Ten_Dx { get; set; }
        public string Ten_Dx1 { get; set; }
        public string Ten_Dx2 { get; set; }
        public string Ten_Dx3 { get; set; }

        public string Ten_Dx4 { get; set; }
        public string Ten_Dx5 { get; set; }
        public string Ten_Dg { get; set; }
        public string Ten_Dg1 { get; set; }
        public string Ten_Dg2 { get; set; }
        public string Ten_Dg3 { get; set; }
        public string Ten_Dg4 { get; set; }
        public DateTime? Ngay_Tao { get; set; }
        public DateTime? Ngay_Exp { get; set; }
        public DateTime? Ngay_Eval { get; set; }
        public DateTime? Ngay_Gui { get; set; }
        public DateTime? Ngay_PD { get; set; }
        public DateTime? Ngay_HD { get; set; }
        public DateTime? Ngay_PHD { get; set; }
        public DateTime? Ngay_Ky { get; set; }
        public DateTime? Ngay_TH { get; set; }
        public DateTime? Ngay_THTT { get; set; }
        public DateTime? Ngay_NT { get; set; }
        public DateTime? Ngay_NT_QC { get; set; }
        public bool Status { get; set; }
        public string Ghi_Chu { get; set; }

    }
}
