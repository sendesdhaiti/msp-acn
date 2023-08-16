using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace REPO
{
    public class Product_REPO : BASEREPOCLASS, IProduct_REPO
    {
        public Product_REPO(
            actions.Imsactions a,
            converts.IFormConversions c,
            Microsoft.Extensions.Configuration.IConfiguration config_
        ) : base(a, c, config_) { }

        public async Task<models.Product[]> GetProducts()
        {
            List<models.Product> o = new List<models.Product>();
            string cmd = "select * from public.Product order by updated;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connStr))
            {
                var command = new NpgsqlCommand(cmdText: cmd, connection: connection);
                connection.Open();

                try
                {
                    var ret = await command.ExecuteReaderAsync();
                    while (ret.Read())
                    {
                        // Console.WriteLine(allactions.msactions._ToString(ret));
                        var p = new models.Product();
                        p.id = this.action.v2_Encrypt(ret.GetGuid(0).ToString());
                        p.category = ret.GetString(1);
                        p.name = ret.GetString(2);
                        p.image = ret.GetString(3);
                        p.price = ret.GetDecimal(4);
                        p.description = ret.GetString(5);
                        p.productstatus = this.conversions
                            .GetConvertTypes()
                            .ConvertEnum<models.ProductStatus>(ret.GetInt32(6));
                        p.size = ret.GetInt32(7);
                        p.color = ret.GetString(8);
                        p.productType = this.conversions
                            .GetConvertTypes()
                            .ConvertEnum<models.ProductType>(ret.GetInt32(9));
                        p.deliveryMethod = this.conversions
                            .GetConvertTypes()
                            .ConvertEnum<models.ProductDeliveryMethod>(ret.GetInt32(10));
                        p.productSubscriptionType = this.conversions
                            .GetConvertTypes()
                            .ConvertEnum<models.ProductSubscriptionType>(ret.GetInt32(11));
                        p.inventory = ret.GetInt32(12);
                        p.added = ret.GetDateTime(13);
                        p.updated = ret.GetDateTime(14);
                        p.accountid = this.action.v2_Encrypt(ret.GetGuid(15).ToString());

                        o.Add(p);
                    }
                }
                catch (Exception msg)
                {
                    Console.WriteLine(msg);
                }
                connection.Close();
            }
            actions.Logging.Log("Getting all", "Product", "user", o.Count.ToString());
            return o.ToArray();
        }

        public async Task<bool> CreateProduct(
            string category,
            string name,
            string image,
            decimal price,
            string description,
            models.ProductStatus productstatus,
            int size,
            string color,
            models.ProductType productType,
            models.ProductDeliveryMethod deliveryMethod,
            models.ProductSubscriptionType productSubscriptionType,
            int inventory,
            Guid accountid
        )
        {
            bool o = false;
            string cmd =
                "insert into public.Product "
                + " (category, name, image, price, description, productstatus, size, color, productType, deliveryMethod, productSubscriptionType, inventory, accountid ) "
                + " VALUES(@category, @name, @image, @price, @description, @productstatus, @size, @color, @productType, @deliveryMethod, @productSubscriptionType, @inventory, @accountid);";
            using (NpgsqlConnection connection = new NpgsqlConnection(connStr))
            {
                // Console.WriteLine(productstatus.ToString());
                var command = new NpgsqlCommand(cmdText: cmd, connection: connection);
                command.Parameters.AddWithValue("@category", category);
                command.Parameters.AddWithValue("@name", name);

                command.Parameters.AddWithValue("@image", image);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@productstatus", (int)productstatus);
                command.Parameters.AddWithValue("@size", size);
                command.Parameters.AddWithValue("@color", color);
                command.Parameters.AddWithValue("@productType", (int)productType);
                command.Parameters.AddWithValue("@deliveryMethod", (int)deliveryMethod);
                command.Parameters.AddWithValue(
                    "@productSubscriptionType",
                    (int)productSubscriptionType
                );
                command.Parameters.AddWithValue("@inventory", inventory);

                command.Parameters.AddWithValue("@accountid", accountid);
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
            actions.Logging.Log("Creating", "Product", accountid.ToString(), o.ToString());
            return o;
        }
    }

    public interface IProduct_REPO
    {
        Task<models.Product[]> GetProducts();
        Task<bool> CreateProduct(
            string category,
            string name,
            string image,
            decimal price,
            string description,
            models.ProductStatus productstatus,
            int size,
            string color,
            models.ProductType productType,
            models.ProductDeliveryMethod deliveryMethod,
            models.ProductSubscriptionType productSubscriptionType,
            int inventory,
            Guid accountid
        );
    }
}
