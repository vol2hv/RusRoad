namespace RusRoadLib
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    
    public partial class PassageSrc
    {
        public string Govnumber { get; set; }
        public string Highway_Id { get; set; }
        public string Time { get; set; }
        public string Speed { get; set; }

    }
}
