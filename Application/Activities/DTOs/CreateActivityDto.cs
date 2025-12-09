using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Activities.DTOs;

public class CreateActivityDto
{
    //Using DataAnnotations instead of required modifier keyword because the respond error is better formatted
    [Required]
    public string Title { get; set; } = "";
    //DateTime is a reference type but don't get warning 
    //beacause c# give a default value of DateTime is 1/ January 0001 00:00:00
    public DateTime Date { get; set; }
    //string is a reference type so it can warning if it is null
    //we can use nullable reference type to avoid this warning
    [Required]
    public string Description { get; set; } = "";
    [Required]
    public string Category { get; set; } = "";
    //location props
    [Required]
    public string City { get; set; } = "";
    //địa điểm 
    [Required]
    public string Venue { get; set; } = "";
    //vĩ độ
    //double | number is a value type and default value is 0
    public double Latitude { get; set; }
    //kinh độ
    public double Longitude { get; set; }
}
