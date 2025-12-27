using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    public string DisplayName { get; set; } = ""; // Default value of string is null. If we are not give it initial value is ""
    //So you will not have a standard error, the error is Json serialization (because null) instead of is required
    //The user won't able to send up an empty string and pass validation beacause Required attribute are not allowed
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
    //Required attribute in here is a bit pointless 
    //because asp identity is enforce a complex password for the password
    public string Password { get; set; } = "";
}
