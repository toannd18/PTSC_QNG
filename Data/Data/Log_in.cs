namespace Data.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Log_in
    {
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        public string Event { get; set; }

        public DateTime Date { get; set; }

        public long IdName { get; set; }

        [Required]
        [StringLength(50)]
        public string tbl_Name { get; set; }
    }
}
