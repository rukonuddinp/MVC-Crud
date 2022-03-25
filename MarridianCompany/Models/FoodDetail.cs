using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using MarridianCompany.Attirbutes.ValidationAttributes;

namespace MarridianCompany.Models
{
    [Table("FoodDetail")]
    public class FoodDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Required, Display(Name = "FoodDetail Name")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        
        [Column(TypeName = "image")]
        public byte[] Image { get; set; }

        [Required, DataType(DataType.Date), Column(TypeName = "date"), Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }

        [Required]
        public long Quantity { get; set; }
        

        [ForeignKey("FoodGroup")]
        public long FoodGroupID { get; set; }
        [NotMapped]
        public bool? ToBeDeleted { get; set; }

        public virtual FoodGroup FoodGroup { get; set; }
    }
}