using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }

        [Required]
        [Display(Name = "List Price")]
        public double ListPrice { get; set; }
       
        [Display(Name = "Price For 1 - 50")]
        public double Price { get; set; }
      
        [Display(Name = "Price For 51 - 100")]
        public double Price50 { get; set; }
        
        [Display(Name = "Price For 101 +")]
        public double Price100 { get; set; }

        public int CategoryId { get; set; }
        [ValidateNever]
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public string ImageUrl { get; set; }



    }



}
