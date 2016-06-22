namespace _OkrDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Passage")]
    public partial class Passage
    {
        [Key]
        public long Passage_Id { get; set; }

        public DateTime Time { get; set; }

        public int CarOwner_Id { get; set; }

        public int Highway_Id { get; set; }

        public int Speed { get; set; }

        public virtual CarOwner CarOwner { get; set; }

        public virtual Highway Highway { get; set; }
    }
}
