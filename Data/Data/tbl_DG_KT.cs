using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Data.Data
{
    public class tbl_DG_KT
    {

        [Key]
        public int Id { get; set; }
        public int DG_NCC_Id { get; set; }
        public int DG_KT_Id { get; set; }
        public int DeXuatId { get; set; }
        public string Ten { get; set; }
        public string Mo_Ta { get; set; }
        public string Ghi_Chu { get; set; }
        public bool DG { get; set; }
    }
}
