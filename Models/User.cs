using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
    

public class Users
{
    [Key]
    public int UserId {get;set;}
    [Required]
    [MinLength(2)]
    public string Name {get;set;}

    [Required]
    [EmailAddress]
    public string Email {get;set;}
    [DataType(DataType.Password)]
    [Required]
    [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
    public string Password {get;set;}
    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
    public List<Guests> Rsvps{get;set;}
    [NotMapped]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string Confirm {get;set;}
}   

public class LoginUsers
{
    [Required]
    [EmailAddress]
    public string Email {get; set;}
    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; }
}

public class UsersWrapper{
    public Users NewUser{get;set;}
    public LoginUsers LoginUser{get;set;}
}