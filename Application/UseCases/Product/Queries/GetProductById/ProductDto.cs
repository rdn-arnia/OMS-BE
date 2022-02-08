namespace OMS.Application.UseCases.Product.Queries.GetProductById
{
    public class ProductDto
    {
        public string ProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CurrentStock { get; set; }
    }
}
