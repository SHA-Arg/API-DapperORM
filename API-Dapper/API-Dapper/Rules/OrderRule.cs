using API_Dapper.Data;
using API_Dapper.Models;

namespace API_Dapper.Rules
{
    public class OrderRule
    {
        public List<Order> GetAllOrders()
        {
            var data = new NorthwindData();
            return data.GetAllOrders();
        }
        public RespuestaDelete DeleteOrderById(int orderId)
        {
            try
            {
                var data = new NorthwindData();
                var cant = data.DeleteOrderById(orderId);
                return new RespuestaDelete()
                {
                    Cantidad = cant,
                    Resultado = true,
                    Mensaje = cant + " Registros eliminados con exito"
                };
            }
            catch (Exception ex)
            {
                return new RespuestaDelete()
                {
                    Cantidad = 0,
                    Resultado = false,
                    Mensaje = ex.Message

                };
            }
        }
    }
}
