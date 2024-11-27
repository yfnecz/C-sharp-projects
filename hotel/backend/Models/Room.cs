using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
     public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get;set;}

        public string Type {get; set;}

        public int Capacity  {get;set;}

        public int RatePerNight {get;set;} 

        public Room()
        {
            Type = string.Empty;
        }
    }

}