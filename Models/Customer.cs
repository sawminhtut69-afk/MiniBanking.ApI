using System.ComponentModel.DataAnnotations;

namespace MiniBanking.API.Models;

public class Customer
{
    public int CustomerId { get; set; }

    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = "";

    [Required]
    [Phone]
    public string Phone { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [StringLength(200)]
    public string Address { get; set; } = "";
}