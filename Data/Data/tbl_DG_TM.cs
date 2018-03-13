using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace Data.Data
{
    public class tbl_DG_TM
    {
        [Key]
        public int Id { get; set; }
        public int DG_NCC_Id { get; set; }
        public int DG_KT_Id { get; set; }
        public int DeXuatId { get; set; }
        public int Don_Gia { get; set; }
        public string Hieu_Luc { get; set; }
        public string Thoi_Gian { get; set; }
        public string Dia_Diem { get; set; }
        public string Dieu_Kien { get; set; }
        public string BH { get; set; }
        public string Che_Do { get; set; }
        public string Ghi_Chu
        {
            get; set;
        }
        }
}
