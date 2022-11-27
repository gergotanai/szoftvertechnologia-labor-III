namespace ProductsElasticsearch.NET.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Descreption { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
