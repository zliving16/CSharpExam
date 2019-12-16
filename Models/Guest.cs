using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

public class Guests{
    [Key]
    public int GuestId{get;set;}
    [Required]
    public int ActivityId{get;set;}
    [Required]
    public int UserId{get;set;}
    public Users User{get;set;}
    public Acitivites NotActivity{get;set;}

}