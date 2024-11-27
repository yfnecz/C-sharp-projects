using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
     public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get;set;}

        //[Column(TypeName = "nvarchar(100)")]
        public string Username {get; set;}

        //[Column(TypeName = "nvarchar(100)")]
        public string Password  {get;set;}

        public User()
        {
            Username = string.Empty; 
            Password = string.Empty;
        }

    }
    
}