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
            creds = new string[] { config["ConnectionStrings:SMTPServer"], config["ConnectionStrings:SMTPUsername"], config["ConnectionStrings:SMTPPassword"] };
            api_fe = APIURL;

        }

        private static string? api_fe;

        private static System.Text.StringBuilder msg = new System.Text.StringBuilder();

        private string acnbody = "s";
        private static string? note = null;
        private static string receiverName = "";
        private static string receiverEmail = "";
        private static int code;
        private readonly static string tr =
            "{border:.5px solid black;border-radius: .5rem;padding:5px;margin: 5px auto 5px auto;background-color:white;}";
        private readonly string[] creds;
        private string dbconn;
        private static readonly string Zoom_ACN_Business_Link = "https://zoom.us/j/740835041";
        private static string Zoom_ACN_Business_AnchorTag =
            $"<p>Step 1: Confirm your attendance: <button style='padding:1rem;border-radius:1rem;background-color:white;color:black;'>Confirm Attendance <u>{api_fe}/confirm/:{receiverEmail}</u>!</button></p>"
            + $"<p>Step 1.2: Enter this code to verify your email:<span style='{styles?.background1_style}'><u>{code}</u></span></p>"
            + $"<p>Step 2: <button style='{styles?.btn1Styles_inline}'>Go to <u>{Zoom_ACN_Business_Link}</u></button></p>"
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
        public int GetCode()
        {
            return Random.Shared.Next(100000, 999999);
        }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String? token = e.UserState as string;

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

        public async Task<bool> SendEmailMessage(
            string _receiverEmail,
            EMAILMESSAGE _msgs
        )
        {
            Console.WriteLine(_receiverEmail, ACTIONS.all.msactions._ToString(_msgs));
            bool sent = false;
            try
            {
                SmtpClient mySmtpClient = new SmtpClient(creds[0])
                {
                    Host = creds[0],
                    UseDefaultCredentials = false,
                    Port = 587,
                    Credentials = new NetworkCredential(creds[1], creds[2]),
                    EnableSsl = true,
                };

                // add from,to MailboxAddresses
                MailAddress from = new MailAddress(creds[1], _msgs.Subject);
                MailAddress to = new MailAddress(_receiverEmail ?? "sendes12@gmail.com", _receiverEmail);
                MailMessage myMail = new MailMessage(from, to);

                // add ReplyTo
                MailAddress replyTo = new MailAddress(creds[1]);
                myMail.ReplyToList.Add((MailAddress)replyTo);

                // set subject and encoding
                myMail.Subject = _msgs.Subject;
                myMail.SubjectEncoding = System.Text.Encoding.UTF8;

                // set body-message and encoding
                System.Text.StringBuilder _msg = new System.Text.StringBuilder();

                if (_msgs.msg != null)
                {
                    foreach (string s in _msgs.msg)
                    {


                        Console.WriteLine(s);
                        _msg.AppendLine(s);
                    }
                }

                msg = _msg;
                acnbody =
                    $@"
                    <html>
                    <head>
                        <style>
                            /* Your CSS styles here */

                            {styles?.p}
                            {styles?.background1_style}
                            tr{tr}
                        </style>
                    </head>
                    <body>
                        <table class='background1_1' style='{styles?.display_flex_justify_n_align_center_direction_column_inline} background-color:rgb(69, 195, 60);'>
                            <tr>
                                {msg.ToString()}
                            </tr>

                            <tr >
                                <p>{MINTSOUP_MailFooterTag}</p>
                            </tr>
                        </table>
                    </body>
                    </html>
                ";

                //ACTIONS.Logging.Log(_msgs, msg.ToString(), null, acnbody);

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



        public async Task<bool> SendACNBusinessInterestMessage(
            string _receiverEmail,
            string _receiverFirstName,
            string _receiverLastName,
            string[] _msgs,
            string? _note,
            int _code
        )
        {
            bool sent = false;
            try
            {
                string e = actions.v2_Encrypt(_receiverEmail);
                //Console.WriteLine(e);
                receiverEmail = Uri.EscapeDataString(e);
                //Console.WriteLine(receiverEmail);
                receiverName = _receiverFirstName + " " + _receiverLastName;
                code = _code;
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
                    $"Welcome to the Market Alert GANG {receiverName.ToUpper()}! "
                );
                if (note != null)
                {
                    _msg.AppendLine(
                        $@"It appears that you were invited through '{note.ToUpper()}'. "
                    );
                }

                for (int i = 0; i < _msgs.Length - 1; i++)
                {
                    _msg.AppendLine(_msgs[i]);
                }

                _msg.AppendLine("To log into your account for the first time and change your password and other details, use your email as the user and password when signing in. ");

                msg = _msg;
                Zoom_ACN_Business_AnchorTag =
                $"<p>Step 1: Confirm your attendance: <button  style='{styles?.btn1Styles_inline}'>Confirm Attendance <u>{api_fe}/confirm/:{receiverEmail}</u>!</button></p>"
                + $"<p>Step 1.2: Enter this code to verify your email:<span style='{styles?.background1_style}'><u>{code}</u></span></p>"
                + $"<p>Step 2: <button style='{styles?.btn1Styles_inline}'>Go to <u>Link Available After Confirmation.</u></button></p>"
                + $"<p>Step 3: Add who invited you next to your name like this: '{note} - {receiverName} - Your_Phone_#'</p>"
                + "<p>Step 4: Mute your microphone</p>"
                + "<p>Step 5: Get a pen and paper (something to write with) ready.</p>";
                acnbody =
                    $@"
                    <html>
                    <head>
                        <style>
                            /* Your CSS styles here */

                            {styles?.p}
                            {styles?.background1_style}
                            tr{tr}
                        
                        
                        

                        </style>
                    </head>
                    <body>
                        <table class='background1_1' style='{styles?.display_flex_justify_n_align_center_direction_column_inline} background-color:rgb(69, 195, 60);'>
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

                //ACTIONS.Logging.Log(_msgs, msg.ToString(), null, acnbody);

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

        public async Task<bool> CreateConfirmationMeetingTimeRelation(Guid confirmation, Guid meetingTime)
        {
            bool check = false;

            string cmd =
                "INSERT into meetingtimeconfirmation (confirmation, time) VALUES(@confirmation, @meetingTime);";

            if (creds.Length < 3)
            {
                return false;
            }
            using (NpgsqlConnection connection = new NpgsqlConnection(dbconn))
            {
                var command = new NpgsqlCommand(cmdText: cmd, connection: connection);
                command.Parameters.AddWithValue("@confirmation", confirmation);
                command.Parameters.AddWithValue("@meetingTime", meetingTime);
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
            ACTIONS.Logging.Log("SAVING", "Meeting Time Confirmation Relation", confirmation + " and " + meetingTime, check.ToString());
            return check;
        }





        public async Task<bool> CreateConfirmation(object[] creds)
        {
            bool check = false;

            string cmd =
                "INSERT into confirm (email, code, confirmed) VALUES((select email from account where email = @email), @code, @confirmed) ON CONFLICT (email) DO Update SET code = EXCLUDED.code ,confirmed = EXCLUDED.confirmed;";

            if (creds.Length < 3)
            {
                return false;
            }
            using (NpgsqlConnection connection = new NpgsqlConnection(dbconn))
            {
                var command = new NpgsqlCommand(cmdText: cmd, connection: connection);
                command.Parameters.AddWithValue("@email", creds[0]);
                command.Parameters.AddWithValue("@confirmed", creds[1]);
                command.Parameters.AddWithValue("@code", creds[2]);
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
            ACTIONS.Logging.Log("SAVING", "BUSINESS INTEREST EMAIL CONFIRMATION", creds[0], check.ToString());
            return check;
        }

        public async Task<bool> UpdateConfirmation(object[] creds)
        {
            bool check = false;

            string cmd =
                "UPDATE confirm SET (confirmed, date, updated) = ( @confirmed, @date, @updated) where email = (select email from account where email = @email) AND code = @code;";

            if (creds.Length < 3)
            {
                return false;
            }
            using (NpgsqlConnection connection = new NpgsqlConnection(dbconn))
            {
                var command = new NpgsqlCommand(cmdText: cmd, connection: connection);
                command.Parameters.AddWithValue("@email", creds[0]);
                command.Parameters.AddWithValue("@confirmed", creds[1]);
                command.Parameters.AddWithValue("@code", creds[2]);
                command.Parameters.AddWithValue("@date", creds[3]);
                command.Parameters.AddWithValue("@updated", DateTime.UtcNow.AddHours(-4));
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
            ACTIONS.Logging.Log("UPDATING", "BUSINESS INTEREST EMAIL CONFIRMATION", creds[0], check.ToString());
            return check;
        }

        public async Task<bool> NewConfirmationCode(string email, int code)
        {
            bool check = false;

            string cmd =
                "Insert into confirm (code, confirmed, updated) = VALUES( @code, @confirmed, @updated) where email = (select email from account where email = @email);";

            using (NpgsqlConnection connection = new NpgsqlConnection(dbconn))
            {
                var command = new NpgsqlCommand(cmdText: cmd, connection: connection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@confirmed", false);
                command.Parameters.AddWithValue("@code", code);
                command.Parameters.AddWithValue("@updated", DateTime.UtcNow.AddHours(-4));
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
            ACTIONS.Logging.Log("NEW Code", "BUSINESS INTEREST EMAIL CONFIRMATION", email, check.ToString());
            return check;
        }

        public async Task<models.MeetingConfirmation[]?> GetConfirmations(object[] creds, bool All_or_Personal)
        {
            List< models.MeetingConfirmation> check = new List<models.MeetingConfirmation> ();

            string cmd;

            if (creds.Length < 1)
            {
                return null;
            }
            if (All_or_Personal == false)
            {
                //we'll only get personal confirmations you've made
                cmd = "select * from confirm where email = @email order by updated;";
            }
            else
            {
                cmd = "select * from confirm order by updated;";
            }



            using (NpgsqlConnection connection = new NpgsqlConnection(dbconn))
            {
                var command = new NpgsqlCommand(cmdText: cmd, connection: connection);
                //If getting all confirmations, reserve for admin only
                if (All_or_Personal == false)
                {
                    //Else we'll only get personal confirmations you've made
                    command.Parameters.AddWithValue("@email", creds[0]);
                }

                connection.Open();

                try
                {
                    var ret = await command.ExecuteReaderAsync();
                    while (ret.Read())
                    {
                        var o = new models.MeetingConfirmation();
                        o.id = actions.v2_Encrypt(ret.GetGuid(0).ToString());
                        o.code = 000000;
                        o.confirmation = ConvConfirmation(ret.GetInt32(2));
                        o.date = ret.GetString(3);
                        o.added = ret.GetDateTime(4);
                        o.updated = ret.GetDateTime(5);
                        o.email = actions.v2_Encrypt(ret.GetString(6));
                        check.Add(o);
                    }
                }
                catch (Exception msg)
                {
                    Console.WriteLine(msg);
                }
                connection.Close();
            }
            ACTIONS.Logging.Log("GETTING", "BUSINESS INTEREST EMAIL CONFIRMATIONS", creds[0], check.Count.ToString());
            return check.ToArray();
        }

        public async Task<models.MeetingConfirmation[]?> CheckConfirmation(string email)
        {
            List<models.MeetingConfirmation> check = new List<models.MeetingConfirmation>();

            string cmd =
                "select * from confirm where email = @email Order by updated LIMIT 1;";
            using (NpgsqlConnection connection = new NpgsqlConnection(dbconn))
            {
                var command = new NpgsqlCommand(cmdText: cmd, connection: connection);
                command.Parameters.AddWithValue("@email", email);
                connection.Open();

                try
                {
                    var ret = await command.ExecuteReaderAsync();
                    while (ret.Read())
                    {
                        var o = new models.MeetingConfirmation();
                        o.id = actions.v2_Encrypt(ret.GetGuid(0).ToString());
                        o.code = 000000;
                        o.confirmation = ConvConfirmation(ret.GetInt32(2));
                        o.date = ret.GetString(3);
                        o.added = ret.GetDateTime(4);
                        o.updated = ret.GetDateTime(5);
                        o.email = actions.v2_Encrypt(ret.GetString(6));
                        check.Add(o);
                    }
                }
                catch (Exception msg)
                {
                    Console.WriteLine(msg);
                }
                connection.Close();
            }
            ACTIONS.Logging.Log("GETTING", "BUSINESS INTEREST EMAIL CONFIRMATIONS", creds[0], check.Count.ToString());
            return check.ToArray();
        }

        public async Task<models.MeetingTime[]> GetMeetingTimes()
        {
            List<models.MeetingTime> check = new List<models.MeetingTime>();

            string cmd =
                "select * from meetingtime;";
            using (NpgsqlConnection connection = new NpgsqlConnection(dbconn))
            {
                var command = new NpgsqlCommand(cmdText: cmd, connection: connection);
                connection.Open();

                try
                {
                    var ret = await command.ExecuteReaderAsync();
                    while (ret.Read())
                    {
                        var o = new models.MeetingTime();
                        o.id = actions.v2_Encrypt(ret.GetGuid(0).ToString());
                        o.host = ret.GetString(1);
                        o.frequency = conversions.GetConvertTypes().ConvertEnum<models.MeetingFrequency>(ret.GetInt32(2));
                        o.day = ret.GetString(3);
                        o.time = ret.GetString(4);
                        o.timezone = ret.GetString(5);
                        o.url = ret.GetString(6);
                        o.added = ret.GetDateTime(7);
                        o.updated = ret.GetDateTime(8);
                        o.creator = actions.v2_Encrypt(ret.GetString(9));
                        o.hostemail = ret.GetString(10);
                        check.Add(o);
                    }
                }
                catch (Exception msg)
                {
                    Console.WriteLine(msg);
                }
                connection.Close();
            }
            ACTIONS.Logging.Log("GETTING", "BUSINESS INTEREST MEETING TIMES", null, check.Count.ToString());
            return check.ToArray();
        }

        public async Task<models.MeetingTime[]> GetMeetingTimes(string email)
        {
            List<models.MeetingTime> check = new List<models.MeetingTime>();

            string cmd =
                "select * from meetingtime where creator = @email;";
            using (NpgsqlConnection connection = new NpgsqlConnection(dbconn))
            {
                var command = new NpgsqlCommand(cmdText: cmd, connection: connection);
                command.Parameters.AddWithValue("@email", email);
                connection.Open();

                try
                {
                    var ret = await command.ExecuteReaderAsync();
                    while (ret.Read())
                    {
                        var o = new models.MeetingTime();
                        o.id = actions.v2_Encrypt(ret.GetGuid(0).ToString());
                        o.host = ret.GetString(1);
                        o.frequency = conversions.GetConvertTypes().ConvertEnum<models.MeetingFrequency>(ret.GetInt32(2));
                        o.day = ret.GetString(3);
                        o.time = ret.GetString(4);
                        o.timezone = ret.GetString(5);
                        o.url = ret.GetString(6);
                        o.added = ret.GetDateTime(7);
                        o.updated = ret.GetDateTime(8);
                        o.creator = actions.v2_Encrypt(ret.GetString(9));
                        o.hostemail = ret.GetString(10);
                        check.Add(o);
                    }
                }
                catch (Exception msg)
                {
                    Console.WriteLine(msg);
                }
                connection.Close();
            }
            ACTIONS.Logging.Log("GETTING", "BUSINESS INTEREST MEETING TIMES", null, check.Count.ToString());
            return check.ToArray();
        }

        public async Task<models.MeetingTime> GetConfirmationMeetingTimes(Guid confirmationId)
        {
            models.MeetingTime o = new models.MeetingTime();

            string cmd =
                "select * from MeetingTime where id = (select time from MeetingTimeConfirmation where confirmation = @confirmationId);";
            using (NpgsqlConnection connection = new NpgsqlConnection(dbconn))
            {
                var command = new NpgsqlCommand(cmdText: cmd, connection: connection);
                command.Parameters.AddWithValue("@confirmationId", confirmationId);
                connection.Open();

                try
                {
                    var ret = await command.ExecuteReaderAsync();
                    while (ret.Read())
                    {
                        //var o = new models.MeetingTime();
                        o.id = actions.v2_Encrypt(ret.GetGuid(0).ToString());
                        o.host = ret.GetString(1);
                        o.frequency = conversions.GetConvertTypes().ConvertEnum<models.MeetingFrequency>(ret.GetInt32(2));
                        o.day = ret.GetString(3);
                        o.time = ret.GetString(4);
                        o.timezone = ret.GetString(5);
                        o.url = ret.GetString(6);
                        o.added = ret.GetDateTime(7);
                        o.updated = ret.GetDateTime(8);
                        o.creator = actions.v2_Encrypt(ret.GetString(9));
                        o.hostemail = ret.GetString(10);
                        //check.Add(o);
                    }
                }
                catch (Exception msg)
                {
                    Console.WriteLine(msg);
                }
                connection.Close();
            }
            ACTIONS.Logging.Log("GETTING", "CONFIRMATION MEETING TIMES", null, o.id);
            return o;
        }

        public async Task<int> CreateMeetingTime(string email, models.MeetingTime[] times)
        {
            int check = 0;

            string cmd =
                "INSERT into meetingtime (creator, host, frequency, day, time, timezone, url, hostemail) VALUES((select email from account where email = @email), @host,@frequency,@day, @time,@timezone, @url, @hostemail);";

            if (times.Length < 1)
            {
                return 0;
            }
            Console.WriteLine($"Times available to upload {times.Length}");
            for (int i = 0; i < times.Length; i++)
            {
                Console.WriteLine(check);
                using (NpgsqlConnection connection = new NpgsqlConnection(dbconn))
                {
                    Console.WriteLine(ACTIONS.all.msactions._ToString(times[i]));
                    var command = new NpgsqlCommand(cmdText: cmd, connection: connection);
                    var mf = times[i].frequency;
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@host", times[i].host ?? "Sendes");
                    command.Parameters.AddWithValue("@hostemail", times[i].hostemail ?? email);
                    command.Parameters.AddWithValue("@frequency", (int)mf );
                    command.Parameters.AddWithValue("@day", times[i].day ?? "");
                    command.Parameters.AddWithValue("@time", times[i].time ?? "");
                    command.Parameters.AddWithValue("@timezone", times[i].timezone ?? "");
                    command.Parameters.AddWithValue("@url", times[i].url ?? "");

                    connection.Open();

                    try
                    {
                        var ret = await command.ExecuteNonQueryAsync();
                        if (ret > 0)
                        {
                            check++;
                        }
                    }
                    catch (Exception msg)
                    {
                        Console.WriteLine(msg);
                    }
                    connection.Close();
                }

            }
            ACTIONS.Logging.Log("SAVING", "BUSINESS INTEREST MEETING TIMES", email, $"{check.ToString()}/{times.Length.ToString()}");
            return check;
        }

        public async Task<int> UpdateMeetingTime(string email, models.MeetingTime[] times)
        {
            int check = 0;

            string cmd =
                "Update meetingtime SET ( host, frequency, day, time, timezone, url, updated, hostemail) = ( @host,@frequency,@day, @time,@timezone, @url, @updated, @hostemail) where creator = (select email from account where email = @email);";

            if (times.Length < 1)
            {
                return 0;
            }
            Console.WriteLine($"Times available to upload {times.Length}");
            for (int i = 0; i < times.Length; i++)
            {
                Console.WriteLine(check);
                using (NpgsqlConnection connection = new NpgsqlConnection(dbconn))
                {
                    Console.WriteLine(ACTIONS.all.msactions._ToString(times[i]));
                    var command = new NpgsqlCommand(cmdText: cmd, connection: connection);
                    var mf = times[i].frequency;
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@host", times[i].host ?? "Sendes");
                    command.Parameters.AddWithValue("@hostemail", times[i].hostemail ?? email);
                    command.Parameters.AddWithValue("@frequency", (int)mf);
                    command.Parameters.AddWithValue("@day", times[i].day ?? "");
                    command.Parameters.AddWithValue("@time", times[i].time ?? "");
                    command.Parameters.AddWithValue("@timezone", times[i].timezone ?? "");
                    command.Parameters.AddWithValue("@url", times[i].url ?? "");
                    command.Parameters.AddWithValue("@updated", DateTime.UtcNow.AddHours(-4));

                    connection.Open();

                    try
                    {
                        var ret = await command.ExecuteNonQueryAsync();
                        if (ret > 0)
                        {
                            check++;
                        }
                    }
                    catch (Exception msg)
                    {
                        Console.WriteLine(msg);
                    }
                    connection.Close();
                }

            }
            ACTIONS.Logging.Log("UPDATING", "BUSINESS INTEREST MEETING TIMES", email, $"{check.ToString()}/{times.Length.ToString()}");
            return check;
        }

        bool ConvConfirmation(int o)
        {
            var x = conversions.GetConvertTypes().ConvertEnum<models.confirmation>(o);
            if (models.confirmation.Not_Going == x)
            {
                return false;
            }
            else if (models.confirmation.Confirmed == x)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
