using API_Dapper.Models;
using Dapper;
using System.Data.SqlClient;
using System.Diagnostics;

namespace API_Dapper.Data
{
    public class NorthwindData
    {
        public string _cnxStr = "Server=SHA-DEV;Database=Northwind;Trusted_Connection=True;";
        public int DeleteOrderById(int orderId)
        {
            using var cnx = new SqlConnection(_cnxStr);
            cnx.Open();
            var tran = cnx.BeginTransaction();
            try
            {                
                var sql = "DELETE FROM [Order Details] WHERE OrderID = @orderId";
                var cant = cnx.Execute(sql, new { orderId }, tran);

                sql = "DELETE FROM Orders WHERE OrderId = @orderId";
                cant += cnx.Execute(sql, new { orderId }, tran);

                tran.Commit();
                return cant;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                cnx.Close();
            }         
        }
        public List<Product> GetAllProducts()
        {
            using var cnx = new SqlConnection();
            cnx.Open();
            var query = "SELECT * FROM Products";
            var listProduct = cnx.Query<Product>(query).ToList();
            return listProduct;
// Siempre recordar poner el cierre de conexion NombreDeLaConexion.Close(); Siempre y cuando no usemos using ya que este cierra solo.
        }
        public Product GetProductById(int id)
        {
            using var cnx = new SqlConnection(_cnxStr);
            cnx.Open();
            var q = "SELECT * FROM Product WHERE ProductId = @id";
            var product = cnx.QueryFirstOrDefault<Product>(q, new { id } );
            return product;     
        }
        //Otra manera de llamar haciendo sobrecarga.
        public Product GetProductById(Product p)
        {
            using var cnx = new SqlConnection(_cnxStr);
            cnx.Open();
            var q = "SELECT * FROM Product WHERE ProductId = @ProductId";
            // Aca recibo un objeto del tipo Producto.
            var product = cnx.QueryFirstOrDefault<Product>(q, p);
            return product;
        }

        public List<Order> GetAllOrders2()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            using var cnx = new SqlConnection(_cnxStr);
            cnx.Open();
            var q = "SELECT OrderId, CustomerID FROM Orders";

            var ordenes = cnx.Query<Order>(q).ToList();

            if (ordenes != null)
            {
                foreach (var o in ordenes)
                {
                    var query = "SELECT * FORM [Order Details] WHERE OrderId = @OrderId";
                    o.Details = new List<OrderDetail>();
                    o.Details.AddRange(cnx.Query<OrderDetail>(query, o).ToList());
                }
            }
            stopwatch.Stop();
            var tiempo = stopwatch.ElapsedMilliseconds;
            return ordenes;
        }
        public List<Order> GetAllOrders()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            using var cnx = new SqlConnection(_cnxStr);
                cnx.Open();
                var q = "SELECT * FROM Orders o INNER JOIN [Order Details]" +
                    "od ON o.OrderID = od.OrderID";

                var dicc = new Dictionary<int, Order>();

                cnx.Query<Order, OrderDetail, Order>(q, (o, d) =>
                {
                    if (!dicc.TryGetValue(o.OrderID, out Order order))
                        dicc.Add(o.OrderID, order = o);
                    if (order.Details == null)
                        order.Details = new List<OrderDetail>();
                    order.Details.Add(d);
                    return order;
                },
                splitOn: "OrderId").AsQueryable();

                var orders = dicc.Values.ToList();

            stopwatch.Stop();
            var tiempo = stopwatch.ElapsedMilliseconds;
            return orders;
        }
    }
}
