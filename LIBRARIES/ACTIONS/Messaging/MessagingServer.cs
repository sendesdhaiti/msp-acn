using System;// For String, Int32, Console, ArgumentException
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using ACTIONS;
using System.IO;// For IO Exception
using System.Net.Sockets;// For TcpClient, NetworkStream, SocketException
using Microsoft.AspNetCore.SignalR;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.SignalR.Hubs;
namespace ACTIONS
{

    public class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        public void PostConfigure(string? name, JwtBearerOptions options)
        {
            var originalOnMessageReceived = options.Events.OnMessageReceived;
            options.Events.OnMessageReceived = async context =>
            {
                await originalOnMessageReceived(context);

                if (string.IsNullOrEmpty(context.Token))
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;

                    if (!string.IsNullOrEmpty(accessToken) &&
                        path.StartsWithSegments("/hubs"))
                    {
                        context.Token = accessToken;
                    }
                }
            };
        }
    }
    public class Messaging{
        public class Message{
            public  Message(){}
            public Message(string msg, string taskid, string email ){
                Msg = ACTIONS.all.msactions.v2_Encrypt_ToString(msg);
                Taskid = ACTIONS.all.msactions.v2_Encrypt_ToString(taskid);
                Email = ACTIONS.all.msactions.v2_Encrypt_ToString(email);
            }

            public string Msg = "";
            public string Taskid = "";
            public string Email = "";
        }

        public struct WebSocketActions
        {
            public static readonly string MESSAGE_RECEIVED = "messageReceived";
            public static readonly string USER_LEFT = "userLeft";
            public static readonly string USER_JOINED = "userJoined";
        }
        public const string tokenScheme =  nameof(tokenScheme);
        public const string cookieScheme =  nameof(cookieScheme);
        public interface IMessagingClient{
            Task Register(string username);
            Task Leave(string username);
            Task Notification(string email, Message message);
            object CookieProtected();
            object TokenProtected();
        }

        [Microsoft.AspNetCore.Cors.EnableCors("CookieOriginsPolicy")]
        [Authorize]
        // [HubName("MyHub")]
        // [Authorize(AuthenticationSchemes = cookieScheme)]
        // [Microsoft.AspNetCore.Authorization.Authorize(Policy =  "MyMessagingAuthorizationPolicy")]
        public class MessagingClientHub:Hub, IMessagingClient
        {
            static readonly Dictionary<string, string> Users = new Dictionary<string, string>();
            public async Task Register(string email)
            {
                if (Users.ContainsKey(email))
                {
                    Users.Add(email, this.Context.ConnectionId);
                }

                await Clients.All.SendAsync(WebSocketActions.USER_JOINED, email);
            }

            public async Task Leave(string email)
            {
                Users.Remove(email);
                await Clients.All.SendAsync(WebSocketActions.USER_LEFT, email);
            }


            [Authorize("Cookie")]
            public async Task Notification(string email, Message message)
            {
                Console.WriteLine("Sending Message");
                var c = Context.GetHttpContext()?.Connection;
                var con = Context.GetHttpContext();
                var Protocol = Context.GetHttpContext()?.Request.Protocol;
                var cookies = Context.GetHttpContext()?.Request.Cookies;
                Console.WriteLine($"{c}, {con}, {Protocol}, {cookies}");
                await Clients.All.SendAsync("notification", email, message);
            }

            [Authorize("Cookie")]
            public object CookieProtected(){
                return CompileResult();
            }

            [Authorize("Token")]
            public object TokenProtected(){
                return CompileResult();
            }

            private object CompileResult() => new {
                UserId = Context.UserIdentifier,
                Claims = Context.User?.Claims.Select(x => new {x.Type, x.Value})
            };
        // }

            // // Clients
            // // [Microsoft.AspNetCore.Mvc.HttpPost("SendAMessage")]
            // // [Authorize(AuthenticationSchemes = cookieScheme)]
            // public async void SendAMessage(Message msg){
            //     //Get the clients credentials such as email, personid, and username from the front-end

            //     //We are gonna use this info to make a jwt token to send with the clients request to any front-end client
            //     //, even your own notification messages

            //     //We are gonna do that by Configuring the SendAsnc with a jwt 
            //     //token to make the info safe for transport and make each request identifyable
            //     await this.Clients.Clients(this.Context.ConnectionId).SendAsync("requestServerResponse", msg);
            //     Console.WriteLine(ACTIONS.all.msactions._break + ACTIONS.all.msactions._ToString(msg));
            //     ACTIONS.all.msactions._ToString(ACTIONS.all.msactions._break + this.Context);
            //     ACTIONS.all.msactions._ToString(ACTIONS.all.msactions._break + this.Clients);
            // }



            
        }

    }
}