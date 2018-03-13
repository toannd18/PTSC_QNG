namespace Data.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Notifications
    {
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string SendId { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceiveId { get; set; }

        [StringLength(100)]
        public string Url { get; set; }

        [StringLength(100)]
        public string Notification { get; set; }

        public DateTime Date { get; set; }

        public bool Status { get; set; }
    }
}
