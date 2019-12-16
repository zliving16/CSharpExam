using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ExamC_.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) {}
        public DbSet<Users> users {get;set;}
        public DbSet<Guests> guests{get;set;}
        public DbSet<Acitivites> acitivites{get;set;}

        
    }
}