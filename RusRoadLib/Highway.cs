namespace RusRoadLib
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Highway")]
    public partial class Highway
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Highway()
        {
            Passage = new HashSet<Passage>();
        }

        [Key]
        public int Highway_Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        public int Speed { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Passage> Passage { get; set; }
    }
}
