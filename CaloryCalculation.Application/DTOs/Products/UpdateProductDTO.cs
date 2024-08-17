namespace CaloryCalculation.Application.DTOs.Products
{
    public class UpdateProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public double Protein { get; set; }

        public double Fat { get; set; }

        public double Carb { get; set; }
    }
}
