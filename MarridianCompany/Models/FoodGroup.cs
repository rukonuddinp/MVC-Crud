using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MarridianCompany.Models
{
    [Table("FoodGroup")]
    public class FoodGroup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Required, Display(Name = "FoodGroup")]
        public string Name { get; set; }

        public virtual IList<FoodDetail> FoodDetails { get; set; }
    }
}