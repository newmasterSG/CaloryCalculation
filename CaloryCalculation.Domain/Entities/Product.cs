namespace CaloryCalculatiom.Domain.Entities
{
    public class Product
    {
        public Product() 
        {
            FoodConsumptions = new List<FoodConsumption>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Calories { get; set; } = 0;
        public double Fat { get; set; } = 0;
        public double Сarbohydrate { get; set; } = 0;

        public double Protein { get; set; } = 0;

        public int CreatedUserId { get; set; }

        public virtual User CreatedUser { get; set; }

        public virtual ICollection<FoodConsumption> FoodConsumptions { get; set; }
    }
}
