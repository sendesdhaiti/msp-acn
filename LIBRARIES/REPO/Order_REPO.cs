using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
namespace REPO
{
    public class Order_REPO: BASEREPOCLASS, IOrder_REPO
    {
        public Order_REPO(
            actions.Imsactions a,
            converts.IFormConversions c,
            Microsoft.Extensions.Configuration.IConfiguration config_
        ) : base(a, c, config_) { }

        public async Task<bool> CreateOrder(
            models.ProductType type,
            string category,
            decimal amount,
            string desc,
            models.OrderStatus orderstatus,
            string paymentmethod,
            Guid accid,
            Guid prodid
        ){
            bool o = false;
            string cmd =
                "insert into public.Order "
                + " (category, type, amount, description, orderstatus, paymentmethod, accountid, productid) "
                + " VALUES(@category, @type, @amount, @description, @orderstatus, @paymentmethod, @accountid, @productid);";
            using (NpgsqlConnection connection = new NpgsqlConnection(connStr))
            {
                // Console.WriteLine(productstatus.ToString());
                var command = new NpgsqlCommand(cmdText: cmd, connection: connection);
                command.Parameters.AddWithValue("@category", category);
                command.Parameters.AddWithValue("@type", (int)type);
                command.Parameters.AddWithValue("@amount", amount);

                command.Parameters.AddWithValue("@description", desc);
                command.Parameters.AddWithValue("@orderstatus", (int)orderstatus);
                command.Parameters.AddWithValue("@paymentmethod", paymentmethod);
                command.Parameters.AddWithValue("@accountid", accid);
                command.Parameters.AddWithValue("@productid", prodid);
                connection.Open();

                try
                {
                    var ret = await command.ExecuteNonQueryAsync();
                    if (ret > 0)
                    {
                        o = true;
                    }
                }
                catch (Exception msg)
                {
                    Console.WriteLine(msg);
                }
                connection.Close();
            }
            actions.Logging.Log("Creating", "Order", accid.ToString(), o.ToString());
            return o;
        }
    }

    public interface IOrder_REPO{
        Task<bool> CreateOrder(
            models.ProductType type,
            string category,
            decimal amount,
            string desc,
            models.OrderStatus orderstatus,
            string paymentmethod,
            Guid accid,
            Guid prodid
        );
    }
}