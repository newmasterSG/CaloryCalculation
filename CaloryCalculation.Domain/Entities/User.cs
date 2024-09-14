using CaloryCalculatiom.Domain.Entities.Enums;
using CaloryCalculation.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CaloryCalculatiom.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public User() 
        {
            Goals = new List<Goal>();
            DailyLogs = new List<DailyLog>();
            CreatedProducts = new List<Product>();
            CreatedExerices = new List<Exercise>();
        }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public double Height { get; set; }
        public double Weight { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }

        public virtual ICollection<Goal> Goals { get; set; }
        public virtual ICollection<DailyLog> DailyLogs { get; set; }
        public virtual ICollection<Product> CreatedProducts { get; set; }
        public virtual ICollection<Exercise> CreatedExerices { get; set; }
    }
}
