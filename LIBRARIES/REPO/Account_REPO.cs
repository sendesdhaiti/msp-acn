using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace REPO
{
    public interface IAccount_REPO
    {
        Task<models.Account?> Login(string email, string password);
        Task<bool> DeleteAccount(string email);
        Task<bool> CreateAccount(
            string email,
            string password,
            string firstname,
            string lastname,
            string username
        );
        Task<bool> CheckEmail(string email);
        Task<bool> CheckUsername(string username);
        Task<bool> UpdateAccount(
            string email,
            string? new_email,
            string? password,
            string? username,
            bool? verified
        );
        Task<models.Account?> GetAccount(string email);
        Task<bool> VerifyAccount(
            string email,
            int code
        );
    }

    public class Account_REPO : BASEREPOCLASS, IAccount_REPO
    {
        public Account_REPO(
            actions.Imsactions a,
            converts.IFormConversions c,
            Microsoft.Extensions.Configuration.IConfiguration config_
        ) : base(a, c, config_) { }

        // private readonly string conn;
        // private readonly IWebEnvironments conn;
        public async Task<models.Account?> Login(string email, string password)
        {
            models.Account? o = null;
            string cmd =
                "select * from public.Account where email = @email AND password = crypt( @password, password);";
            using (NpgsqlConnection connection = new NpgsqlConnection(connStr))
            {
                var command = new NpgsqlCommand(cmdText: cmd, connection: connection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);
                connection.Open();

                try
                {
                    var ret = await command.ExecuteReaderAsync();
                    if (ret.Read())
                    {
                        o = new MODELS.Account();
                        o.id = this.action.v2_Encrypt(ret.GetGuid(0).ToString());
                        o.email = ret.GetString(1);
                        o.firstname = ret.GetString(2);
                        o.lastname = ret.GetString(3);
                        o.username = ret.GetString(4);
                        o.password = "";
                        o.verified = ret.GetBoolean(6);
                        o.added = ret.GetDateTime(7);
                        o.updated = ret.GetDateTime(8);
                    }
                }
                catch (Exception msg)
                {
                    Console.WriteLine(msg);
                }
                connection.Close();
            }
            actions.Logging.Log("Logging in", "Account", email ?? "NULL", o?.username ?? "NULL");
            return o;
        }

        public async Task<models.Account?> GetAccount(string email)
        {
            models.Account? o = null;
            string cmd =
                "select * from public.Account where email = @email;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connStr))
            {
                var command = new NpgsqlCommand(cmdText: cmd, connection: connection);
                command.Parameters.AddWithValue("@email", email);
                connection.Open();

                try
                {
                    var ret = await command.ExecuteReaderAsync();
                    if (ret.Read())
                    {
                        o = new MODELS.Account();
                        o.id = this.action.v2_Encrypt(ret.GetGuid(0).ToString());
                        o.email = ret.GetString(1);
                        o.firstname = ret.GetString(2);
                        o.lastname = ret.GetString(3);
                        o.username = ret.GetString(4);
                        o.password = "";
                        o.verified = ret.GetBoolean(6);
                        o.added = ret.GetDateTime(7);
                        o.updated = ret.GetDateTime(8);
                    }
                }
                catch (Exception msg)
                {
                    Console.WriteLine(msg);
                }
                connection.Close();
            }
            actions.Logging.Log("Getting", "Account", email ?? "NULL", o?.username ?? "NULL");
            return o;
        }

        public async Task<bool> CreateAccount(
            string email,
            string password,
            string firstname,
            string lastname,
            string username
        )
        {
            bool o = false;
            string cmd =
                "insert into public.Account (email, password, firstname, lastname, username) VALUES(@email, crypt(@password, gen_salt('md5')), @firstname, @lastname, @username) On Conflict (email) Do Nothing;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connStr))
            {
                var command = new NpgsqlCommand(cmdText: cmd, connection: connection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@firstname", firstname);
                command.Parameters.AddWithValue("@lastname", lastname);
                command.Parameters.AddWithValue("@username", username);
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
            actions.Logging.Log("Signing up", "Account", email, o.ToString());
            return o;
        }

        public async Task<bool> UpdateAccount(
            string email,
            string? new_email,
            string? password,
            string? username,
            bool? verified
        )
        {
            bool o = false;
            string cmd;

            if (username != null && password != null && verified != null )
            {
                cmd =
                "update Account SET (email, username, verified, updated, password) VALUES(@new_email, @username, @verified, @updated, @password) where email = @email;";
            }
            else if (username != null && password != null && verified == null)
            {
                cmd =
                "update Account SET (email, username, updated, password) VALUES(@new_email, @username, @updated, crypt(@password, gen_salt('md5'))) where email = @email;";
            }
            else if (username != null && password == null && verified == null)
            {
                cmd =
                "update Account SET (email, username, updated) VALUES(@new_email, @username, @updated) where email = @email;";
            }
            else {
                return false;
            }
            using (NpgsqlConnection connection = new NpgsqlConnection(connStr))
            {
                var command = new NpgsqlCommand(cmdText: cmd, connection: connection);

                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@new_email", new_email ?? email);

                command.Parameters.AddWithValue("@updated", DateTime.UtcNow.AddHours(-4));
                if (username != null && password != null && verified != null)
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@verified", verified);
                    command.Parameters.AddWithValue("@password", password);
                }
                else if (username != null && password != null && verified == null)
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                }
                else if (username != null && password == null && verified == null)
                {
                    command.Parameters.AddWithValue("@username", username);
                }
                else
                {
                    return false;
                }

                
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
            actions.Logging.Log("Updating", "Account", email, o.ToString());
            return o;
        }

        public async Task<bool> VerifyAccount(
            string email,
            int code
        )
        {
            bool o = false;
            string cmd = "update Account SET (verified, updated) = ( 'true', @updated) where email = (select email from confirm where email = @email and code = @code);";
            using (NpgsqlConnection connection = new NpgsqlConnection(connStr))
            {
                var command = new NpgsqlCommand(cmdText: cmd, connection: connection);

                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@code", code);
                command.Parameters.AddWithValue("@updated", DateTime.UtcNow.AddHours(-4));


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
            actions.Logging.Log("Updating", "Account", email, o.ToString());
            return o;
        }

        public async Task<bool> DeleteAccount(string email)
        {
            bool o = false;
            string cmd = "delete from public.Account where email = @email;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connStr))
            {
                var command = new NpgsqlCommand(cmdText: cmd, connection: connection);
                command.Parameters.AddWithValue("@email", email);
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
            actions.Logging.Log("Deleting", "Account", email, o.ToString());
            return o;
        }

        public async Task<bool> CheckEmail(string email)
        {
            bool o = false;
            string cmd = "select * from public.Account where email = @email;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connStr))
            {
                var command = new NpgsqlCommand(cmdText: cmd, connection: connection);
                command.Parameters.AddWithValue("@email", email);
                connection.Open();

                try
                {
                    var ret = await command.ExecuteReaderAsync();
                    if (ret.Read())
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
            actions.Logging.Log("checking email", "Account", email, o.ToString());
            return o;
        }

        public async Task<bool> CheckUsername(string username)
        {
            bool o = false;
            string cmd = "select * from public.Account where username = @username;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connStr))
            {
                var command = new NpgsqlCommand(cmdText: cmd, connection: connection);
                command.Parameters.AddWithValue("@username", username);
                connection.Open();

                try
                {
                    var ret = await command.ExecuteReaderAsync();
                    if (ret.Read())
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
            actions.Logging.Log("checking username", "Account", username, o.ToString());
            return o;
        }
    }
}
