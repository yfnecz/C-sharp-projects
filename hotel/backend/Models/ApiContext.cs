using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
        : base(options){}

        public DbSet<User> Users { get; set; } = null!;   

        public DbSet<Room> Rooms { get; set; } = null!;   

        public DbSet<Reservation> Reservations { get; set; } = null!;  

    }

}