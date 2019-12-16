using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

public class Acitivites{
    [Key]
    public int ActivityId{get;set;}

    [Required]
    public string Title{get;set;}
    public DateTime Time{get;set;}
    [Required]
    public DateTime Date{get;set;}
    [Range(0,220)]
    public int Duration{get;set;}
    public string UnitOfTime{get;set;}
    [Required]
    public string Desc{get;set;}
    [Required]
    public int CreatorId{get;set;}
    public Users Creator{get;set;}
    public List<Guests> UsersAtEvent{get;set;}



}