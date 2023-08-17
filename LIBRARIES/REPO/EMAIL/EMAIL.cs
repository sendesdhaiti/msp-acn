using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ACTIONS.STATICFILES;

using Npgsql;
//using MailKit;
//using MailKit.Net.Smtp;
//using MimeKit;
using ACTIONS.STYLE;
using System.Net.Mail;
using static System.Net.Mail.MailMessage;
using System.Net;
using System.ComponentModel;
using System.Text;

namespace REPO.EMAIL
{
    public class EMAIL : BASEREPOCLASS, IEMAIL
    {
        public EMAIL(actions.Imsactions a, converts.IFormConversions c, Microsoft.Extensions.Configuration.IConfiguration config) : base(a, c, config)
        {
            dbconn = connStr ?? "";
            this.actions = a;
            creds = new string[] { config["ConnectionStrings:SMTPServer"], config["ConnectionStrings:SMTPUsername"],  config["ConnectionStrings:SMTPPassword"] };
            api_fe = APIURL;

        }

        private static string? api_fe;

        private static System.Text.StringBuilder msg = new System.Text.StringBuilder();

        private string acnbody =
            $@"
                <html>
                <head>
                    <style>
                        /* Your CSS styles here */

                        
                        
                        
                        

                    </style>
                </head>
                <body>
                    <table class='background1_1' style='{styles.display_flex_justify_n_align_center_direction_column_inline} background-color:rgb(69, 195, 60);'>
                        <tr >
                            {msg.ToString()}
                        </tr>


                        <tr >
                            <p>{Zoom_Times}</p>
                        </tr>

                        <tr >
                            {Zoom_ACN_Business_AnchorTag}
                        </tr>

                        <tr >
                            <p>{MINTSOUP_MailFooterTag}</p>
                        </tr>
                    </table>
                </body>
                </html>
            ";
        private static string? note = null;
        private static string receiverName = "";
        private readonly static string tr =
            "{border:.5px solid black;border-radius: .5rem;padding:5px;margin: 5px auto 5px auto;background-color:white;}";
        private readonly string[] creds;
        private string dbconn;
        private static readonly string Zoom_ACN_Business_Link = "https://zoom.us/j/740835041";
        private static string Zoom_ACN_Business_AnchorTag =
            $"<p>Step 1: Confirm you are going by visiting: <button href=\"{api_fe}\" style='{styles?.btn1Styles_inline}'>Confirm Attendance <u>\"{api_fe}confirm\"</u>!</button></p>"
            + $"<p>Step 2: <button href=\"{Zoom_ACN_Business_Link}\" style='{styles?.btn1Styles_inline}'>Go to <u>{Zoom_ACN_Business_Link}</u></button></p>"
            + "<p>Step 3: Add 'Sendes' next to your name like this: 'Sendes - Your_Name - Your_Phone_#'</p>"
            + "<p>Step 4: Mute your microphone</p>"
            + "<p>Step 5: Get a pen and paper (something to write with) ready.</p>";
        private static readonly string Zoom_Times =
            @"
            If you are available this evening at either 7pm EST or 9:30pm EST, we will be holding two 20 minute presentations on how to earn income on essential services that ties in with real estate.
            If you can make one of these times the zoom code is <span><u>864 398 2855</u></span>.
            The overview is only 20 minutes long so don't miss out on this special opportunity. I hope to see you there!
        ";
        private static readonly actions.STYLE.STYLES styles = new actions.STYLE.STYLES();
        private static readonly string MINTSOUP_MailFooterTag =
            "Mint Soup LLC © 2023, 1‌95 Maplewood Avenue STE 1, Maplewood, NJ 07040, PO Box 66. Mint Soup, Mint Soup Services and the Mint Soup logo are registered trademarks of Mint Soup.";
        actions.Imsactions actions;


        static bool mailSent = false;

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
            mailSent = true;
        }

        private void ReLoadAcnMsg()
        {
            acnbody =
                $@"
                <html>
                <head>
                    <style>
                        /* Your CSS styles here */

                        
                        
                        
                        

                    </style>
                </head>
                <body>
                    <table class='background1_1' style='{styles.display_flex_justify_n_align_center_direction_column_inline} background-color:rgb(69, 195, 60);'>
                        <tr >
                            {msg.ToString()}
                        </tr>


                        <tr >
                            <p>{Zoom_Times}</p>
                        </tr>

                        <tr >
                            {Zoom_ACN_Business_AnchorTag}
                        </tr>

                        <tr >
                            <p>{MINTSOUP_MailFooterTag}</p>
                        </tr>
                    </table>
                </body>
                </html>
            ";
        }

        private string MSG(System.Text.StringBuilder _msg, string[] msgs)
        {
            _msg.AppendLine(
                $"Welcome to the Market Madness GANG {receiverName.ToUpper()}! "
            );
            if (note != null)
            {
                _msg.AppendLine(
                    $@"It appears that you have joined through {note.ToLower()}. "
                );
            }

            for (int i = 0; i < msgs.Length - 1; i++)
            {
                _msg.AppendLine(msgs[i]);
            }

            msg = _msg;
            ReLoadAcnMsg();

            ACTIONS.Logging.Log(msgs, msg.ToString(), null, acnbody);
            return acnbody;
        }

        public async Task<bool> SendACNBusinessInterestMessage(
            string _receiverEmail,
            string _receiverFirstName,
            string _receiverLastName,
            string[] _msgs,
            string? _note
        )
        {
            bool sent = false;
            try
            {
                receiverName = _receiverFirstName + " " + _receiverLastName;
                note = _note;
                SmtpClient mySmtpClient = new SmtpClient(creds[0])
                {
                    Host = creds[0],
                    UseDefaultCredentials = false,
                    Port = 587,
                    Credentials = new NetworkCredential(creds[1], creds[2]),
                    EnableSsl = true,
                };

                // add from,to MailboxAddresses
                MailAddress from = new MailAddress(creds[1], "The Market Alert Business Interest");
                MailAddress to = new MailAddress(_receiverEmail, receiverName);
                MailMessage myMail = new MailMessage(from, to);

                // add ReplyTo
                MailAddress replyTo = new MailAddress(creds[1]);
                myMail.ReplyToList.Add((MailAddress)replyTo);

                // set subject and encoding
                myMail.Subject = "Business Partner and Market Alert Team Interest";
                myMail.SubjectEncoding = System.Text.Encoding.UTF8;

                // set body-message and encoding
                System.Text.StringBuilder _msg = new System.Text.StringBuilder();

                _msg.AppendLine(
                    $"Welcome to the Market Madness GANG {receiverName.ToUpper()}! "
                );
                if (note != null)
                {
                    _msg.AppendLine(
                        $@"It appears that you have joined through {note.ToLower()}. "
                    );
                }

                for (int i = 0; i < _msgs.Length - 1; i++)
                {
                    _msg.AppendLine(_msgs[i]);
                }

                msg = _msg;
                acnbody =
                    $@"
                    <html>
                    <head>
                        <style>
                            /* Your CSS styles here */

                            {styles.p}
                            {styles.background1_style}
                            tr{tr}
                        
                        
                        

                        </style>
                    </head>
                    <body>
                        <table class='background1_1' style='{styles.display_flex_justify_n_align_center_direction_column_inline} background-color:rgb(69, 195, 60);'>
                            <tr >
                                {msg.ToString()}
                            </tr>


                            <tr >
                                <p>{Zoom_Times}</p>
                            </tr>

                            <tr >
                                {Zoom_ACN_Business_AnchorTag}
                            </tr>

                            <tr >
                                <p>{MINTSOUP_MailFooterTag}</p>
                            </tr>
                        </table>
                    </body>
                    </html>
                ";

                ACTIONS.Logging.Log(_msgs, msg.ToString(), null, acnbody);

                //byte[] htmlBytes = Encoding.UTF8.GetBytes(htmlBody);
                //string encodedHtml = Convert.ToBase64String(htmlBytes);
                myMail.Body = acnbody;
                myMail.BodyEncoding = System.Text.Encoding.UTF32;
                // text or html
                myMail.IsBodyHtml = true;

                await mySmtpClient.SendMailAsync(myMail);
                mySmtpClient.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

                sent = true;
            }
            catch (SmtpException ex)
            {
                sent = false;
                throw new ApplicationException("SmtpException has occured: " + ex.Message);
            }

            return sent;
            //return false;
        }

        public async Task<bool> RegisterUser(string[] creds)
        {
            bool check = false;

            string cmd =
                "INSERT into account (email, firstname, lastname) VALUES(@email, @fn, @ln) ON CONFLICT (email) DO Update SET firstname = EXCLUDED.firstname ,lastname = EXCLUDED.lastname;";

            if (creds.Length < 3)
            {
                return false;
            }
            using (NpgsqlConnection connection = new NpgsqlConnection(dbconn))
            {
                var command = new NpgsqlCommand(cmdText: cmd, connection: connection);
                command.Parameters.AddWithValue("@email", creds[0]);
                command.Parameters.AddWithValue("@fn", creds[1]);
                command.Parameters.AddWithValue("@ln", creds[2]);
                connection.Open();

                try
                {
                    var ret = await command.ExecuteNonQueryAsync();
                    if (ret > 0)
                    {
                        check = true;
                    }
                }
                catch (Exception msg)
                {
                    Console.WriteLine(msg);
                }
                connection.Close();
            }
            ACTIONS.Logging.Log("SAVING", "BUSINESS INTEREST EMAIL", creds[0], check.ToString());
            return check;
        }
    }
}
