namespace CaloryCalculation.Application.DTOs.Products
{
    public class CreateProductDTO
    {
        public string Name { get; set; }

        public double Protein { get; set; }

        public double Fat {  get; set; }

        public double Carb { get; set; }

        public int? UserId { get; set; }
    }
}
