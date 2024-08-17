using CaloryCalculatiom.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace CaloryCalculation.Application.DTOs.User
{
    public class RegisterUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public double Height { get; set; }
        public double Weight { get; set; }

        public int Age { get; set; }
        public Gender Gender { get; set; }
    }
}
