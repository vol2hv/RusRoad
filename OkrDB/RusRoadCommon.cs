namespace _OkrDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RusRoadCommon")]
    public partial class RusRoadCommon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RusRoadCommon_Id { get; set; }

        public DateTime? LastReport { get; set; }

        public DateTime? Test { get; set; }
    }
}
