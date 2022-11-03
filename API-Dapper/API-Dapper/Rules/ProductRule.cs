using API_Dapper.Models;
using API_Dapper.Data;

namespace API_Dapper.Rules
{
    public class ProductRule
    {
        public List<Product> GetAllProducts()
        {
            var data = new Data.NorthwindData();
            return data.GetAllProducts();
        }
        public Product GetProductById(int id)
        {
            var data = new Data.NorthwindData();
            var p = new Product()
            {
                ProductID = id
            };

            return data.GetProductById(id);        
        }

    }
}
