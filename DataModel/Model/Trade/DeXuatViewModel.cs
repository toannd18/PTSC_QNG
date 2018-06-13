using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Model.Trade
{
    public class DeXuatViewModel
    {
        public int Id { get; set; }
        public string Ma { get; set; }
        public string Tieu_De { get; set; }
        public bool Kieu { get; set; }
        public string FullName_Dx { get; set; }
        public string FullName_TH { get; set; }
        public string Sohd { get; set; }
        public string TenNCC { get; set; }
        public DateTime? Ngay_Tao { get; set; }
        public DateTime? Ngay_Ky { get; set; }
        public DateTime? Ngay_TH { get; set; }
        public DateTime? Ngay_THTT { get; set; }
        public DateTime? Ngay_NT { get; set; }
        public DateTime? Ngay_NT_QC { get; set; }
        public bool Status { get; set; }
        public string Ghi_Chu { get; set; }
    }
}
