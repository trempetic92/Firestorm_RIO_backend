using System;
using System.Net;
using System.Collections.Specialized;
using HtmlAgilityPack;
using MySql.Data.MySqlClient;
using System.Text;
using System.Text.RegularExpressions;
using Topshelf;

namespace ConsoleApp
{
    
    class Program
    {

        static void Main(string[] args)
        {

           
           

                while (true)
                {

                    string connectionString = "server=localhost;user=root;database=firestorm_mythicplus;port=3306;password=*********";


                    MySqlConnection connection = new MySqlConnection(connectionString);
                    connection.Open();

                    for (int j = 0; j <= 9; j++)
                    {
                        string url = "";
                        switch (j)
                        {
                            case 0:
                                url = "https://firestorm-servers.com/en/challenge/challenge/2284/380";
                                break;

                            case 1:
                                url = "https://firestorm-servers.com/en/challenge/challenge/2287/378";
                                break;

                            case 2:
                                url = "https://firestorm-servers.com/en/challenge/challenge/2290/375";
                                break;

                            case 3:
                                url = "https://firestorm-servers.com/en/challenge/challenge/2285/381";
                                break;

                            case 4:
                                url = "https://firestorm-servers.com/en/challenge/challenge/2289/379";
                                break;

                            case 5:
                                url = "https://firestorm-servers.com/en/challenge/challenge/2286/376";
                                break;

                            case 6:
                                url = "https://firestorm-servers.com/en/challenge/challenge/2291/377";
                                break;

                            case 7:
                                url = "https://firestorm-servers.com/en/challenge/challenge/2293/382";
                                break;

                            case 8:
                                url = "https://firestorm-servers.com/en/challenge/challenge/2441/392";
                                break;

                            case 9:
                                url = "https://firestorm-servers.com/en/challenge/challenge/2441/391";

                                break;
                        }


                        string postdata = "csrf_test_name=04e658905c793fb322e8e6d9f5bee7cf";


                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);


                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";



                        request.Headers.Add("Connection", "keep-alive");
                        request.Headers.Add("authority", "firestorm-servers.com");
                        request.Headers.Add("accept", "application/json, text/javascript, */*; q=0.01");
                        request.Headers.Add("accept-language", "en-US,en;q=0.9");
                        request.Headers.Add("content-type", "application/x-www-form-urlencoded; charset=UTF-8");
                        request.Headers.Add("cookie", "_ga=GA1.2.1506014137.1652033197; csrf_cookie_name=04e658905c793fb322e8e6d9f5bee7cf; user_lang=en; _gid=GA1.2.2132695181.1671471007; teasing-sl-92=1; _gat=1; firestorm_session=egf5sf1pv8kdf1ksrqc4g9c9avsk9g3k");
                        request.Headers.Add("origin", "https://firestorm-servers.com");
                        request.Headers.Add("referer", "https://firestorm-servers.com/en/challenge/index");
                        request.Headers.Add("sec-ch-ua", "\"Not ? A_Brand\";v=\"8\", \"Chromium\";v=\"108\", \"Google Chrome\";v=\"108\"");
                        request.Headers.Add("sec-ch-ua-mobile", "?0");
                        request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
                        request.Headers.Add("sec-fetch-dest", "empty");
                        request.Headers.Add("sec-fetch-mode", "cors");
                        request.Headers.Add("sec-fetch-site", "same-origin");
                        request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36");
                        request.Headers.Add("x-requested-with", "XMLHttpRequest");


                        byte[] dataBytes = Encoding.UTF8.GetBytes(postdata);


                        request.ContentLength = dataBytes.Length;


                        using (Stream requestStream = request.GetRequestStream())
                        {
                            requestStream.Write(dataBytes, 0, dataBytes.Length);
                        }


                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();



                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string html = reader.ReadToEnd();
                            html = html.Substring(90).Replace(@"\/", "/");
                            html = html.Replace(@"\""", "\"");
                            html = html.Replace(@"\n", "");

                            HtmlDocument doc = new HtmlDocument();
                            doc.LoadHtml(html);
                            HtmlNode table = doc.DocumentNode.SelectSingleNode("//table[@id='challenge-results']");




                            foreach (HtmlNode row in table.SelectNodes("tbody"))
                            {

                                HtmlNodeCollection cells = row.SelectNodes("tr");


                                string[] data = new string[cells.Count];

                                for (int i = 0; i < cells.Count; i++)
                                {

                                    data[i] = cells[i].InnerText;



                                    string innerHtml = cells[i].InnerHtml;


                                    Regex regex = new Regex("class='(color_\\S+)'");
                                    MatchCollection matches = regex.Matches(innerHtml);

                                    string[] spanClasses = new string[5];

                                    int g = 0;
                                    foreach (Match match in matches)
                                    {
                                        spanClasses[g] = match.Groups[1].Value;
                                        g++;
                                    }


                                    string dungeon = "";

                                    if (j == 0)
                                    {

                                        string weburl = "https://firestorm-servers.com/en/challenge/index";
                                        HtmlWeb web = new HtmlWeb();
                                        HtmlDocument doc1 = web.Load(weburl);
                                        HtmlNode div = doc1.DocumentNode.SelectSingleNode("//div[@class='mutator' and (contains(., 'Fortified') or contains(., 'Tyrannical'))]");
                                        string innerText = div.InnerText.Trim();


                                        string sql = "";
                                        if (innerText == "Fortified")
                                        {
                                            sql = "REPLACE INTO sd_forti (Rank, MythicLevel, Time, Groups, Completed, Raiting, InsertTime) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                                        }
                                        else
                                        {
                                            sql = "REPLACE INTO sd_tyra (Rank, MythicLevel, Time, Groups, Completed, Raiting, InsertTime) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                                        }
                                        MySqlCommand cmd = new MySqlCommand(sql, connection);

                                        string test = Regex.Unescape(data[i]);



                                        string[] realdata = test.Split(" ");
                                        string[] values = { realdata[53], "00:41:00", "00:32:48", "00:24:36", realdata[35] };
                                        string raiting_multiplier = raitingcalculation(values);
                                        string[] raitingdata = raiting_multiplier.Split(' ');

                                        double raiting = double.Parse(raitingdata[0]);

                                        string plus = raitingdata[1];



                                        realdata[17] = realdata[17].Trim();
                                        realdata[35] = realdata[35].Trim();
                                        realdata[53] = realdata[53].Trim();
                                        string group = realdata[71] + " " + realdata[72] + " " + realdata[73] + " " + realdata[74] + " " + realdata[75];
                                        string vrijeme = realdata[93] + " " + realdata[94] + " " + realdata[95] + realdata[96];

                                        int rank = Int32.Parse(realdata[17]);
                                        cmd.Parameters.AddWithValue("@value1", rank);
                                        cmd.Parameters.AddWithValue("@value2", realdata[35] + "" + plus);
                                        cmd.Parameters.AddWithValue("@value3", realdata[53]);
                                        cmd.Parameters.AddWithValue("@value4", group);
                                        cmd.Parameters.AddWithValue("@value5", vrijeme);




                                        cmd.Parameters.AddWithValue("@value6", raiting);

                                        DateTime dateTimeVariable = DateTime.Now;
                                        cmd.Parameters.AddWithValue("@value7", dateTimeVariable);
                                        cmd.ExecuteNonQuery();


                                        string[] playernames = { realdata[71], realdata[72], realdata[73], realdata[74], realdata[75] };


                                        if (innerText == "Fortified")
                                        {
                                            for (int h = 0; h < playernames.Length; h++)
                                            {

                                                using (MySqlConnection connectionplayer = new MySqlConnection(connectionString))
                                                {
                                                    connectionplayer.Open();

                                                    string query = @"INSERT INTO raiting_total (PlayerName, PlayerRaiting, PlayerClass)
                                                                 SELECT @PlayerName, @PlayerRaiting, @PlayerClass
                                                                 FROM DUAL
                                                                 WHERE NOT EXISTS (SELECT 1 FROM raiting_total WHERE PlayerName = @PlayerName);";

                                                    using (MySqlCommand command = new MySqlCommand(query, connection))
                                                    {
                                                        command.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        command.Parameters.AddWithValue("@PlayerRaiting", 0);
                                                        command.Parameters.AddWithValue("@PlayerClass", spanClasses[h]);

                                                        command.ExecuteNonQuery();
                                                    }

                                                    connectionplayer.Close();
                                                }

                                                using (MySqlConnection connection_sd = new MySqlConnection(connectionString))
                                                {
                                                    connection_sd.Open();
                                                    using (MySqlCommand cmd_sd_raiting = new MySqlCommand("SELECT Raiting FROM playerlist_sd_forti WHERE PlayerName = '" + playernames[h] + "' ORDER BY Raiting DESC LIMIT 1", connection_sd))
                                                    {
                                                        cmd_sd_raiting.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        object result_sd_raiting = cmd_sd_raiting.ExecuteScalar();

                                                        double old_raiting = 0.0;

                                                        if (result_sd_raiting == DBNull.Value || result_sd_raiting == null)
                                                        {
                                                            old_raiting = 0.0;
                                                        }
                                                        else
                                                        {
                                                            old_raiting = (double)result_sd_raiting;
                                                        }


                                                        if (raiting > old_raiting)
                                                        {
                                                            string sql2 = "REPLACE INTO playerlist_sd_forti(PlayerName, PlayerRaiting, PlayerClass ,MythicLevel, Time, Groups, Completed, Raiting) VALUES (@value1, @value2, @value3, @value4, @value5, @value6 ,@value7, @value8)";
                                                            MySqlCommand cmd2 = new MySqlCommand(sql2, connection);
                                                            cmd2.Parameters.AddWithValue("@value1", playernames[h]);
                                                            cmd2.Parameters.AddWithValue("@value2", raiting);
                                                            cmd2.Parameters.AddWithValue("@value3", spanClasses[h]);
                                                            cmd2.Parameters.AddWithValue("@value4", realdata[35] + "" + plus);
                                                            cmd2.Parameters.AddWithValue("@value5", realdata[53]);
                                                            cmd2.Parameters.AddWithValue("@value6", group);
                                                            cmd2.Parameters.AddWithValue("@value7", vrijeme);
                                                            cmd2.Parameters.AddWithValue("@value8", raiting);
                                                            cmd2.ExecuteNonQuery();

                                                        }

                                                    }
                                                    connection_sd.Close();
                                                }

                                            }
                                        }
                                        else
                                        {
                                            for (int h = 0; h < playernames.Length; h++)
                                            {

                                                using (MySqlConnection connectionplayer = new MySqlConnection(connectionString))
                                                {
                                                    connectionplayer.Open();

                                                    string query = @"INSERT INTO raiting_total (PlayerName, PlayerRaiting, PlayerClass)
                                                                 SELECT @PlayerName, @PlayerRaiting, @PlayerClass
                                                                 FROM DUAL
                                                                 WHERE NOT EXISTS (SELECT 1 FROM raiting_total WHERE PlayerName = @PlayerName);";

                                                    using (MySqlCommand command = new MySqlCommand(query, connection))
                                                    {
                                                        command.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        command.Parameters.AddWithValue("@PlayerRaiting", 0);
                                                        command.Parameters.AddWithValue("@PlayerClass", spanClasses[h]);

                                                        command.ExecuteNonQuery();
                                                    }

                                                    connectionplayer.Close();
                                                }


                                                using (MySqlConnection connection_sd = new MySqlConnection(connectionString))
                                                {
                                                    connection_sd.Open();
                                                    using (MySqlCommand cmd_sd_raiting = new MySqlCommand("SELECT Raiting FROM playerlist_sd_tyra WHERE PlayerName = '" + playernames[h] + "' ORDER BY Raiting DESC LIMIT 1", connection_sd))
                                                    {
                                                        cmd_sd_raiting.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        object result_sd_raiting = cmd_sd_raiting.ExecuteScalar();

                                                        double old_raiting = 0.0;

                                                        if (result_sd_raiting == DBNull.Value || result_sd_raiting == null)
                                                        {
                                                            old_raiting = 0.0;
                                                        }
                                                        else
                                                        {
                                                            old_raiting = (double)result_sd_raiting;
                                                        }


                                                        if (raiting > old_raiting)
                                                        {
                                                            string sql2 = "REPLACE INTO playerlist_sd_tyra(PlayerName, PlayerRaiting, PlayerClass ,MythicLevel, Time, Groups, Completed, Raiting) VALUES (@value1, @value2, @value3, @value4, @value5, @value6 ,@value7, @value8)";
                                                            MySqlCommand cmd2 = new MySqlCommand(sql2, connection);
                                                            cmd2.Parameters.AddWithValue("@value1", playernames[h]);
                                                            cmd2.Parameters.AddWithValue("@value2", raiting);
                                                            cmd2.Parameters.AddWithValue("@value3", spanClasses[h]);
                                                            cmd2.Parameters.AddWithValue("@value4", realdata[35] + "" + plus);
                                                            cmd2.Parameters.AddWithValue("@value5", realdata[53]);
                                                            cmd2.Parameters.AddWithValue("@value6", group);
                                                            cmd2.Parameters.AddWithValue("@value7", vrijeme);
                                                            cmd2.Parameters.AddWithValue("@value8", raiting);
                                                            cmd2.ExecuteNonQuery();

                                                        }
                                                        connection_sd.Close();
                                                    }
                                                }

                                            }
                                        }


                                        dungeon = "SD";


                                    }
                                    else if (j == 1)
                                    {
                                        string weburl = "https://firestorm-servers.com/en/challenge/index";
                                        HtmlWeb web = new HtmlWeb();
                                        HtmlDocument doc1 = web.Load(weburl);
                                        HtmlNode div = doc1.DocumentNode.SelectSingleNode("//div[@class='mutator' and (contains(., 'Fortified') or contains(., 'Tyrannical'))]");
                                        string innerText = div.InnerText.Trim();


                                        string sql = "";
                                        if (innerText == "Fortified")
                                        {
                                            sql = "REPLACE INTO hoa_forti (Rank, MythicLevel, Time, Groups, Completed, Raiting, InsertTime) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                                        }
                                        else
                                        {
                                            sql = "REPLACE INTO hoa_tyra (Rank, MythicLevel, Time, Groups, Completed, Raiting, InsertTime) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                                        }
                                        MySqlCommand cmd = new MySqlCommand(sql, connection);

                                        string test = Regex.Unescape(data[i]);



                                        string[] realdata = test.Split(" ");


                                        realdata[17] = realdata[17].Trim();
                                        realdata[35] = realdata[35].Trim();
                                        realdata[53] = realdata[53].Trim();
                                        string group = realdata[71] + " " + realdata[72] + " " + realdata[73] + " " + realdata[74] + " " + realdata[75];
                                        string vrijeme = realdata[93] + " " + realdata[94] + " " + realdata[95] + realdata[96];
                                        string[] values = { realdata[53], "00:32:00", "00:25:36", "00:19:12", realdata[35] };
                                        string raiting_multiplier = raitingcalculation(values);
                                        string[] raitingdata = raiting_multiplier.Split(' ');

                                        double raiting = double.Parse(raitingdata[0]);

                                        string plus = raitingdata[1];
                                        int rank = Int32.Parse(realdata[17]);
                                        cmd.Parameters.AddWithValue("@value1", rank);
                                        cmd.Parameters.AddWithValue("@value2", realdata[35] + "" + plus);
                                        cmd.Parameters.AddWithValue("@value3", realdata[53]);
                                        cmd.Parameters.AddWithValue("@value4", group);
                                        cmd.Parameters.AddWithValue("@value5", vrijeme);



                                        cmd.Parameters.AddWithValue("@value6", raiting);

                                        DateTime dateTimeVariable = DateTime.Now;
                                        cmd.Parameters.AddWithValue("@value7", dateTimeVariable);
                                        cmd.ExecuteNonQuery();



                                        string[] playernames = { realdata[71], realdata[72], realdata[73], realdata[74], realdata[75] };


                                        if (innerText == "Fortified")
                                        {
                                            for (int h = 0; h < playernames.Length; h++)
                                            {

                                                using (MySqlConnection connectionplayer = new MySqlConnection(connectionString))
                                                {
                                                    connectionplayer.Open();

                                                    string query = @"INSERT INTO raiting_total (PlayerName, PlayerRaiting, PlayerClass)
                                                                 SELECT @PlayerName, @PlayerRaiting, @PlayerClass
                                                                 FROM DUAL
                                                                 WHERE NOT EXISTS (SELECT 1 FROM raiting_total WHERE PlayerName = @PlayerName);";

                                                    using (MySqlCommand command = new MySqlCommand(query, connection))
                                                    {
                                                        command.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        command.Parameters.AddWithValue("@PlayerRaiting", 0);
                                                        command.Parameters.AddWithValue("@PlayerClass", spanClasses[h]);

                                                        command.ExecuteNonQuery();
                                                    }

                                                    connectionplayer.Close();
                                                }


                                                using (MySqlConnection connection_sd = new MySqlConnection(connectionString))
                                                {
                                                    connection_sd.Open();
                                                    using (MySqlCommand cmd_sd_raiting = new MySqlCommand("SELECT Raiting FROM playerlist_hoa_forti WHERE PlayerName = '" + playernames[h] + "' ORDER BY Raiting DESC LIMIT 1", connection_sd))
                                                    {
                                                        cmd_sd_raiting.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        object result_sd_raiting = cmd_sd_raiting.ExecuteScalar();

                                                        double old_raiting = 0.0;

                                                        if (result_sd_raiting == DBNull.Value || result_sd_raiting == null)
                                                        {
                                                            old_raiting = 0.0;
                                                        }
                                                        else
                                                        {
                                                            old_raiting = (double)result_sd_raiting;
                                                        }


                                                        if (raiting > old_raiting)
                                                        {
                                                            string sql2 = "REPLACE INTO playerlist_hoa_forti(PlayerName, PlayerRaiting, PlayerClass ,MythicLevel, Time, Groups, Completed, Raiting) VALUES (@value1, @value2, @value3, @value4, @value5, @value6 ,@value7, @value8)";
                                                            MySqlCommand cmd2 = new MySqlCommand(sql2, connection);
                                                            cmd2.Parameters.AddWithValue("@value1", playernames[h]);
                                                            cmd2.Parameters.AddWithValue("@value2", raiting);
                                                            cmd2.Parameters.AddWithValue("@value3", spanClasses[h]);
                                                            cmd2.Parameters.AddWithValue("@value4", realdata[35] + "" + plus);
                                                            cmd2.Parameters.AddWithValue("@value5", realdata[53]);
                                                            cmd2.Parameters.AddWithValue("@value6", group);
                                                            cmd2.Parameters.AddWithValue("@value7", vrijeme);
                                                            cmd2.Parameters.AddWithValue("@value8", raiting);
                                                            cmd2.ExecuteNonQuery();

                                                        }

                                                    }
                                                    connection_sd.Close();
                                                }

                                            }
                                        }
                                        else
                                        {
                                            for (int h = 0; h < playernames.Length; h++)
                                            {


                                                using (MySqlConnection connectionplayer = new MySqlConnection(connectionString))
                                                {
                                                    connectionplayer.Open();

                                                    string query = @"INSERT INTO raiting_total (PlayerName, PlayerRaiting, PlayerClass)
                                                                 SELECT @PlayerName, @PlayerRaiting, @PlayerClass
                                                                 FROM DUAL
                                                                 WHERE NOT EXISTS (SELECT 1 FROM raiting_total WHERE PlayerName = @PlayerName);";

                                                    using (MySqlCommand command = new MySqlCommand(query, connection))
                                                    {
                                                        command.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        command.Parameters.AddWithValue("@PlayerRaiting", 0);
                                                        command.Parameters.AddWithValue("@PlayerClass", spanClasses[h]);

                                                        command.ExecuteNonQuery();
                                                    }

                                                    connectionplayer.Close();
                                                }

                                                using (MySqlConnection connection_sd = new MySqlConnection(connectionString))
                                                {
                                                    connection_sd.Open();
                                                    using (MySqlCommand cmd_sd_raiting = new MySqlCommand("SELECT Raiting FROM playerlist_hoa_tyra WHERE PlayerName = '" + playernames[h] + "' ORDER BY Raiting DESC LIMIT 1", connection_sd))
                                                    {
                                                        cmd_sd_raiting.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        object result_sd_raiting = cmd_sd_raiting.ExecuteScalar();

                                                        double old_raiting = 0.0;

                                                        if (result_sd_raiting == DBNull.Value || result_sd_raiting == null)
                                                        {
                                                            old_raiting = 0.0;
                                                        }
                                                        else
                                                        {
                                                            old_raiting = (double)result_sd_raiting;
                                                        }


                                                        if (raiting > old_raiting)
                                                        {
                                                            string sql2 = "REPLACE INTO playerlist_hoa_tyra(PlayerName, PlayerRaiting, PlayerClass ,MythicLevel, Time, Groups, Completed, Raiting) VALUES (@value1, @value2, @value3, @value4, @value5, @value6 ,@value7, @value8)";
                                                            MySqlCommand cmd2 = new MySqlCommand(sql2, connection);
                                                            cmd2.Parameters.AddWithValue("@value1", playernames[h]);
                                                            cmd2.Parameters.AddWithValue("@value2", raiting);
                                                            cmd2.Parameters.AddWithValue("@value3", spanClasses[h]);
                                                            cmd2.Parameters.AddWithValue("@value4", realdata[35] + "" + plus);
                                                            cmd2.Parameters.AddWithValue("@value5", realdata[53]);
                                                            cmd2.Parameters.AddWithValue("@value6", group);
                                                            cmd2.Parameters.AddWithValue("@value7", vrijeme);
                                                            cmd2.Parameters.AddWithValue("@value8", raiting);
                                                            cmd2.ExecuteNonQuery();

                                                        }

                                                    }
                                                    connection_sd.Close();
                                                }

                                            }
                                        }
                                        dungeon = "HOA";
                                    }
                                    else if (j == 2)
                                    {
                                        string weburl = "https://firestorm-servers.com/en/challenge/index";
                                        HtmlWeb web = new HtmlWeb();
                                        HtmlDocument doc1 = web.Load(weburl);
                                        HtmlNode div = doc1.DocumentNode.SelectSingleNode("//div[@class='mutator' and (contains(., 'Fortified') or contains(., 'Tyrannical'))]");
                                        string innerText = div.InnerText.Trim();


                                        string sql = "";
                                        if (innerText == "Fortified")
                                        {
                                            sql = "REPLACE INTO mots_forti (Rank, MythicLevel, Time, Groups, Completed, Raiting, InsertTime) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                                        }
                                        else
                                        {
                                            sql = "REPLACE INTO mots_tyra (Rank, MythicLevel, Time, Groups, Completed, Raiting, InsertTime) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                                        }
                                        MySqlCommand cmd = new MySqlCommand(sql, connection);

                                        string test = Regex.Unescape(data[i]);



                                        string[] realdata = test.Split(" ");


                                        realdata[17] = realdata[17].Trim();
                                        realdata[35] = realdata[35].Trim();
                                        realdata[53] = realdata[53].Trim();
                                        string group = realdata[71] + " " + realdata[72] + " " + realdata[73] + " " + realdata[74] + " " + realdata[75];
                                        string vrijeme = realdata[93] + " " + realdata[94] + " " + realdata[95] + realdata[96];
                                        string[] values = { realdata[53], "00:30:00", "00:24:00", "00:18:00", realdata[35] };
                                        string raiting_multiplier = raitingcalculation(values);
                                        string[] raitingdata = raiting_multiplier.Split(' ');

                                        double raiting = double.Parse(raitingdata[0]);

                                        string plus = raitingdata[1];
                                        int rank = Int32.Parse(realdata[17]);
                                        cmd.Parameters.AddWithValue("@value1", rank);
                                        cmd.Parameters.AddWithValue("@value2", realdata[35] + "" + plus);
                                        cmd.Parameters.AddWithValue("@value3", realdata[53]);
                                        cmd.Parameters.AddWithValue("@value4", group);
                                        cmd.Parameters.AddWithValue("@value5", vrijeme);


                                        cmd.Parameters.AddWithValue("@value6", raiting);

                                        DateTime dateTimeVariable = DateTime.Now;
                                        cmd.Parameters.AddWithValue("@value7", dateTimeVariable);
                                        cmd.ExecuteNonQuery();



                                        string[] playernames = { realdata[71], realdata[72], realdata[73], realdata[74], realdata[75] };


                                        if (innerText == "Fortified")
                                        {
                                            for (int h = 0; h < playernames.Length; h++)
                                            {

                                                using (MySqlConnection connectionplayer = new MySqlConnection(connectionString))
                                                {
                                                    connectionplayer.Open();

                                                    string query = @"INSERT INTO raiting_total (PlayerName, PlayerRaiting, PlayerClass)
                                                                 SELECT @PlayerName, @PlayerRaiting, @PlayerClass
                                                                 FROM DUAL
                                                                 WHERE NOT EXISTS (SELECT 1 FROM raiting_total WHERE PlayerName = @PlayerName);";

                                                    using (MySqlCommand command = new MySqlCommand(query, connection))
                                                    {
                                                        command.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        command.Parameters.AddWithValue("@PlayerRaiting", 0);
                                                        command.Parameters.AddWithValue("@PlayerClass", spanClasses[h]);

                                                        command.ExecuteNonQuery();
                                                    }

                                                    connectionplayer.Close();
                                                }

                                                using (MySqlConnection connection_sd = new MySqlConnection(connectionString))
                                                {
                                                    connection_sd.Open();
                                                    using (MySqlCommand cmd_sd_raiting = new MySqlCommand("SELECT Raiting FROM playerlist_mots_forti WHERE PlayerName = '" + playernames[h] + "' ORDER BY Raiting DESC LIMIT 1", connection_sd))
                                                    {
                                                        cmd_sd_raiting.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        object result_sd_raiting = cmd_sd_raiting.ExecuteScalar();

                                                        double old_raiting = 0.0;

                                                        if (result_sd_raiting == DBNull.Value || result_sd_raiting == null)
                                                        {
                                                            old_raiting = 0.0;
                                                        }
                                                        else
                                                        {
                                                            old_raiting = (double)result_sd_raiting;
                                                        }


                                                        if (raiting > old_raiting)
                                                        {
                                                            string sql2 = "REPLACE INTO playerlist_mots_forti(PlayerName, PlayerRaiting, PlayerClass ,MythicLevel, Time, Groups, Completed, Raiting) VALUES (@value1, @value2, @value3, @value4, @value5, @value6 ,@value7, @value8)";
                                                            MySqlCommand cmd2 = new MySqlCommand(sql2, connection);
                                                            cmd2.Parameters.AddWithValue("@value1", playernames[h]);
                                                            cmd2.Parameters.AddWithValue("@value2", raiting);
                                                            cmd2.Parameters.AddWithValue("@value3", spanClasses[h]);
                                                            cmd2.Parameters.AddWithValue("@value4", realdata[35] + "" + plus);
                                                            cmd2.Parameters.AddWithValue("@value5", realdata[53]);
                                                            cmd2.Parameters.AddWithValue("@value6", group);
                                                            cmd2.Parameters.AddWithValue("@value7", vrijeme);
                                                            cmd2.Parameters.AddWithValue("@value8", raiting);
                                                            cmd2.ExecuteNonQuery();

                                                        }

                                                    }
                                                    connection_sd.Close();
                                                }

                                            }
                                        }
                                        else
                                        {
                                            for (int h = 0; h < playernames.Length; h++)
                                            {

                                                using (MySqlConnection connectionplayer = new MySqlConnection(connectionString))
                                                {
                                                    connectionplayer.Open();

                                                    string query = @"INSERT INTO raiting_total (PlayerName, PlayerRaiting, PlayerClass)
                                                                 SELECT @PlayerName, @PlayerRaiting, @PlayerClass
                                                                 FROM DUAL
                                                                 WHERE NOT EXISTS (SELECT 1 FROM raiting_total WHERE PlayerName = @PlayerName);";

                                                    using (MySqlCommand command = new MySqlCommand(query, connection))
                                                    {
                                                        command.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        command.Parameters.AddWithValue("@PlayerRaiting", 0);
                                                        command.Parameters.AddWithValue("@PlayerClass", spanClasses[h]);

                                                        command.ExecuteNonQuery();
                                                    }

                                                    connectionplayer.Close();
                                                }


                                                using (MySqlConnection connection_sd = new MySqlConnection(connectionString))
                                                {
                                                    connection_sd.Open();
                                                    using (MySqlCommand cmd_sd_raiting = new MySqlCommand("SELECT Raiting FROM playerlist_mots_tyra WHERE PlayerName = '" + playernames[h] + "' ORDER BY Raiting DESC LIMIT 1", connection_sd))
                                                    {
                                                        cmd_sd_raiting.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        object result_sd_raiting = cmd_sd_raiting.ExecuteScalar();

                                                        double old_raiting = 0.0;

                                                        if (result_sd_raiting == DBNull.Value || result_sd_raiting == null)
                                                        {
                                                            old_raiting = 0.0;
                                                        }
                                                        else
                                                        {
                                                            old_raiting = (double)result_sd_raiting;
                                                        }


                                                        if (raiting > old_raiting)
                                                        {
                                                            string sql2 = "REPLACE INTO playerlist_mots_tyra(PlayerName, PlayerRaiting, PlayerClass ,MythicLevel, Time, Groups, Completed, Raiting) VALUES (@value1, @value2, @value3, @value4, @value5, @value6 ,@value7, @value8)";
                                                            MySqlCommand cmd2 = new MySqlCommand(sql2, connection);
                                                            cmd2.Parameters.AddWithValue("@value1", playernames[h]);
                                                            cmd2.Parameters.AddWithValue("@value2", raiting);
                                                            cmd2.Parameters.AddWithValue("@value3", spanClasses[h]);
                                                            cmd2.Parameters.AddWithValue("@value4", realdata[35] + "" + plus);
                                                            cmd2.Parameters.AddWithValue("@value5", realdata[53]);
                                                            cmd2.Parameters.AddWithValue("@value6", group);
                                                            cmd2.Parameters.AddWithValue("@value7", vrijeme);
                                                            cmd2.Parameters.AddWithValue("@value8", raiting);
                                                            cmd2.ExecuteNonQuery();

                                                        }

                                                    }
                                                    connection_sd.Close();
                                                }

                                            }
                                        }
                                        dungeon = "MOTS";
                                    }
                                    else if (j == 3)
                                    {
                                        string weburl = "https://firestorm-servers.com/en/challenge/index";
                                        HtmlWeb web = new HtmlWeb();
                                        HtmlDocument doc1 = web.Load(weburl);
                                        HtmlNode div = doc1.DocumentNode.SelectSingleNode("//div[@class='mutator' and (contains(., 'Fortified') or contains(., 'Tyrannical'))]");
                                        string innerText = div.InnerText.Trim();


                                        string sql = "";
                                        if (innerText == "Fortified")
                                        {
                                            sql = "REPLACE INTO soa_forti (Rank, MythicLevel, Time, Groups, Completed, Raiting, InsertTime) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                                        }
                                        else
                                        {
                                            sql = "REPLACE INTO soa_tyra (Rank, MythicLevel, Time, Groups, Completed, Raiting, InsertTime) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                                        }
                                        MySqlCommand cmd = new MySqlCommand(sql, connection);

                                        string test = Regex.Unescape(data[i]);



                                        string[] realdata = test.Split(" ");


                                        realdata[17] = realdata[17].Trim();
                                        realdata[35] = realdata[35].Trim();
                                        realdata[53] = realdata[53].Trim();
                                        string group = realdata[71] + " " + realdata[72] + " " + realdata[73] + " " + realdata[74] + " " + realdata[75];
                                        string vrijeme = realdata[93] + " " + realdata[94] + " " + realdata[95] + realdata[96];
                                        string[] values = { realdata[53], "00:39:00", "00:31:12", "00:23:24", realdata[35] };
                                        string raiting_multiplier = raitingcalculation(values);
                                        string[] raitingdata = raiting_multiplier.Split(' ');

                                        double raiting = double.Parse(raitingdata[0]);

                                        string plus = raitingdata[1];
                                        int rank = Int32.Parse(realdata[17]);
                                        cmd.Parameters.AddWithValue("@value1", rank);
                                        cmd.Parameters.AddWithValue("@value2", realdata[35] + "" + plus);
                                        cmd.Parameters.AddWithValue("@value3", realdata[53]);
                                        cmd.Parameters.AddWithValue("@value4", group);
                                        cmd.Parameters.AddWithValue("@value5", vrijeme);


                                        cmd.Parameters.AddWithValue("@value6", raiting);

                                        DateTime dateTimeVariable = DateTime.Now;
                                        cmd.Parameters.AddWithValue("@value7", dateTimeVariable);
                                        cmd.ExecuteNonQuery();



                                        string[] playernames = { realdata[71], realdata[72], realdata[73], realdata[74], realdata[75] };


                                        if (innerText == "Fortified")
                                        {
                                            for (int h = 0; h < playernames.Length; h++)
                                            {

                                                using (MySqlConnection connectionplayer = new MySqlConnection(connectionString))
                                                {
                                                    connectionplayer.Open();

                                                    string query = @"INSERT INTO raiting_total (PlayerName, PlayerRaiting, PlayerClass)
                                                                 SELECT @PlayerName, @PlayerRaiting, @PlayerClass
                                                                 FROM DUAL
                                                                 WHERE NOT EXISTS (SELECT 1 FROM raiting_total WHERE PlayerName = @PlayerName);";

                                                    using (MySqlCommand command = new MySqlCommand(query, connection))
                                                    {
                                                        command.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        command.Parameters.AddWithValue("@PlayerRaiting", 0);
                                                        command.Parameters.AddWithValue("@PlayerClass", spanClasses[h]);

                                                        command.ExecuteNonQuery();
                                                    }

                                                    connectionplayer.Close();
                                                }


                                                using (MySqlConnection connection_sd = new MySqlConnection(connectionString))
                                                {
                                                    connection_sd.Open();
                                                    using (MySqlCommand cmd_sd_raiting = new MySqlCommand("SELECT Raiting FROM playerlist_soa_forti WHERE PlayerName = '" + playernames[h] + "' ORDER BY Raiting DESC LIMIT 1", connection_sd))
                                                    {
                                                        cmd_sd_raiting.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        object result_sd_raiting = cmd_sd_raiting.ExecuteScalar();

                                                        double old_raiting = 0.0;

                                                        if (result_sd_raiting == DBNull.Value || result_sd_raiting == null)
                                                        {
                                                            old_raiting = 0.0;
                                                        }
                                                        else
                                                        {
                                                            old_raiting = (double)result_sd_raiting;
                                                        }


                                                        if (raiting > old_raiting)
                                                        {
                                                            string sql2 = "REPLACE INTO playerlist_soa_forti(PlayerName, PlayerRaiting, PlayerClass ,MythicLevel, Time, Groups, Completed, Raiting) VALUES (@value1, @value2, @value3, @value4, @value5, @value6 ,@value7, @value8)";
                                                            MySqlCommand cmd2 = new MySqlCommand(sql2, connection);
                                                            cmd2.Parameters.AddWithValue("@value1", playernames[h]);
                                                            cmd2.Parameters.AddWithValue("@value2", raiting);
                                                            cmd2.Parameters.AddWithValue("@value3", spanClasses[h]);
                                                            cmd2.Parameters.AddWithValue("@value4", realdata[35] + "" + plus);
                                                            cmd2.Parameters.AddWithValue("@value5", realdata[53]);
                                                            cmd2.Parameters.AddWithValue("@value6", group);
                                                            cmd2.Parameters.AddWithValue("@value7", vrijeme);
                                                            cmd2.Parameters.AddWithValue("@value8", raiting);
                                                            cmd2.ExecuteNonQuery();

                                                        }

                                                    }
                                                    connection_sd.Close();
                                                }

                                            }
                                        }
                                        else
                                        {
                                            for (int h = 0; h < playernames.Length; h++)
                                            {


                                                using (MySqlConnection connectionplayer = new MySqlConnection(connectionString))
                                                {
                                                    connectionplayer.Open();

                                                    string query = @"INSERT INTO raiting_total (PlayerName, PlayerRaiting, PlayerClass)
                                                                 SELECT @PlayerName, @PlayerRaiting, @PlayerClass
                                                                 FROM DUAL
                                                                 WHERE NOT EXISTS (SELECT 1 FROM raiting_total WHERE PlayerName = @PlayerName);";

                                                    using (MySqlCommand command = new MySqlCommand(query, connection))
                                                    {
                                                        command.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        command.Parameters.AddWithValue("@PlayerRaiting", 0);
                                                        command.Parameters.AddWithValue("@PlayerClass", spanClasses[h]);

                                                        command.ExecuteNonQuery();
                                                    }

                                                    connectionplayer.Close();
                                                }


                                                using (MySqlConnection connection_sd = new MySqlConnection(connectionString))
                                                {
                                                    connection_sd.Open();
                                                    using (MySqlCommand cmd_sd_raiting = new MySqlCommand("SELECT Raiting FROM playerlist_soa_tyra WHERE PlayerName = '" + playernames[h] + "' ORDER BY Raiting DESC LIMIT 1", connection_sd))
                                                    {
                                                        cmd_sd_raiting.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        object result_sd_raiting = cmd_sd_raiting.ExecuteScalar();

                                                        double old_raiting = 0.0;

                                                        if (result_sd_raiting == DBNull.Value || result_sd_raiting == null)
                                                        {
                                                            old_raiting = 0.0;
                                                        }
                                                        else
                                                        {
                                                            old_raiting = (double)result_sd_raiting;
                                                        }


                                                        if (raiting > old_raiting)
                                                        {
                                                            string sql2 = "REPLACE INTO playerlist_soa_tyra(PlayerName, PlayerRaiting, PlayerClass ,MythicLevel, Time, Groups, Completed, Raiting) VALUES (@value1, @value2, @value3, @value4, @value5, @value6 ,@value7, @value8)";
                                                            MySqlCommand cmd2 = new MySqlCommand(sql2, connection);
                                                            cmd2.Parameters.AddWithValue("@value1", playernames[h]);
                                                            cmd2.Parameters.AddWithValue("@value2", raiting);
                                                            cmd2.Parameters.AddWithValue("@value3", spanClasses[h]);
                                                            cmd2.Parameters.AddWithValue("@value4", realdata[35] + "" + plus);
                                                            cmd2.Parameters.AddWithValue("@value5", realdata[53]);
                                                            cmd2.Parameters.AddWithValue("@value6", group);
                                                            cmd2.Parameters.AddWithValue("@value7", vrijeme);
                                                            cmd2.Parameters.AddWithValue("@value8", raiting);
                                                            cmd2.ExecuteNonQuery();

                                                        }

                                                    }
                                                    connection_sd.Close();
                                                }

                                            }
                                        }
                                        dungeon = "SOA";
                                    }
                                    else if (j == 4)
                                    {
                                        string weburl = "https://firestorm-servers.com/en/challenge/index";
                                        HtmlWeb web = new HtmlWeb();
                                        HtmlDocument doc1 = web.Load(weburl);
                                        HtmlNode div = doc1.DocumentNode.SelectSingleNode("//div[@class='mutator' and (contains(., 'Fortified') or contains(., 'Tyrannical'))]");
                                        string innerText = div.InnerText.Trim();


                                        string sql = "";
                                        if (innerText == "Fortified")
                                        {
                                            sql = "REPLACE INTO pf_forti (Rank, MythicLevel, Time, Groups, Completed, Raiting, InsertTime) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                                        }
                                        else
                                        {
                                            sql = "REPLACE INTO pf_tyra (Rank, MythicLevel, Time, Groups, Completed, Raiting, InsertTime) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                                        }
                                        MySqlCommand cmd = new MySqlCommand(sql, connection);

                                        string test = Regex.Unescape(data[i]);



                                        string[] realdata = test.Split(" ");


                                        realdata[17] = realdata[17].Trim();
                                        realdata[35] = realdata[35].Trim();
                                        realdata[53] = realdata[53].Trim();
                                        string group = realdata[71] + " " + realdata[72] + " " + realdata[73] + " " + realdata[74] + " " + realdata[75];
                                        string vrijeme = realdata[93] + " " + realdata[94] + " " + realdata[95] + realdata[96];
                                        string[] values = { realdata[53], "00:38:00", "00:30:24", "00:22:48", realdata[35] };
                                        string raiting_multiplier = raitingcalculation(values);
                                        string[] raitingdata = raiting_multiplier.Split(' ');

                                        double raiting = double.Parse(raitingdata[0]);

                                        string plus = raitingdata[1];
                                        int rank = Int32.Parse(realdata[17]);
                                        cmd.Parameters.AddWithValue("@value1", rank);
                                        cmd.Parameters.AddWithValue("@value2", realdata[35] + "" + plus);
                                        cmd.Parameters.AddWithValue("@value3", realdata[53]);
                                        cmd.Parameters.AddWithValue("@value4", group);
                                        cmd.Parameters.AddWithValue("@value5", vrijeme);


                                        cmd.Parameters.AddWithValue("@value6", raiting);

                                        DateTime dateTimeVariable = DateTime.Now;
                                        cmd.Parameters.AddWithValue("@value7", dateTimeVariable);
                                        cmd.ExecuteNonQuery();



                                        string[] playernames = { realdata[71], realdata[72], realdata[73], realdata[74], realdata[75] };


                                        if (innerText == "Fortified")
                                        {
                                            for (int h = 0; h < playernames.Length; h++)
                                            {

                                                using (MySqlConnection connectionplayer = new MySqlConnection(connectionString))
                                                {
                                                    connectionplayer.Open();

                                                    string query = @"INSERT INTO raiting_total (PlayerName, PlayerRaiting, PlayerClass)
                                                                 SELECT @PlayerName, @PlayerRaiting, @PlayerClass
                                                                 FROM DUAL
                                                                 WHERE NOT EXISTS (SELECT 1 FROM raiting_total WHERE PlayerName = @PlayerName);";

                                                    using (MySqlCommand command = new MySqlCommand(query, connection))
                                                    {
                                                        command.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        command.Parameters.AddWithValue("@PlayerRaiting", 0);
                                                        command.Parameters.AddWithValue("@PlayerClass", spanClasses[h]);

                                                        command.ExecuteNonQuery();
                                                    }

                                                    connectionplayer.Close();
                                                }

                                                using (MySqlConnection connection_sd = new MySqlConnection(connectionString))
                                                {
                                                    connection_sd.Open();
                                                    using (MySqlCommand cmd_sd_raiting = new MySqlCommand("SELECT Raiting FROM playerlist_pf_forti WHERE PlayerName = '" + playernames[h] + "' ORDER BY Raiting DESC LIMIT 1", connection_sd))
                                                    {
                                                        cmd_sd_raiting.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        object result_sd_raiting = cmd_sd_raiting.ExecuteScalar();

                                                        double old_raiting = 0.0;

                                                        if (result_sd_raiting == DBNull.Value || result_sd_raiting == null)
                                                        {
                                                            old_raiting = 0.0;
                                                        }
                                                        else
                                                        {
                                                            old_raiting = (double)result_sd_raiting;
                                                        }


                                                        if (raiting > old_raiting)
                                                        {
                                                            string sql2 = "REPLACE INTO playerlist_pf_forti(PlayerName, PlayerRaiting, PlayerClass ,MythicLevel, Time, Groups, Completed, Raiting) VALUES (@value1, @value2, @value3, @value4, @value5, @value6 ,@value7, @value8)";
                                                            MySqlCommand cmd2 = new MySqlCommand(sql2, connection);
                                                            cmd2.Parameters.AddWithValue("@value1", playernames[h]);
                                                            cmd2.Parameters.AddWithValue("@value2", raiting);
                                                            cmd2.Parameters.AddWithValue("@value3", spanClasses[h]);
                                                            cmd2.Parameters.AddWithValue("@value4", realdata[35] + "" + plus);
                                                            cmd2.Parameters.AddWithValue("@value5", realdata[53]);
                                                            cmd2.Parameters.AddWithValue("@value6", group);
                                                            cmd2.Parameters.AddWithValue("@value7", vrijeme);
                                                            cmd2.Parameters.AddWithValue("@value8", raiting);
                                                            cmd2.ExecuteNonQuery();

                                                        }

                                                    }
                                                    connection_sd.Close();
                                                }

                                            }
                                        }
                                        else
                                        {
                                            for (int h = 0; h < playernames.Length; h++)
                                            {

                                                using (MySqlConnection connectionplayer = new MySqlConnection(connectionString))
                                                {
                                                    connectionplayer.Open();

                                                    string query = @"INSERT INTO raiting_total (PlayerName, PlayerRaiting, PlayerClass)
                                                                 SELECT @PlayerName, @PlayerRaiting, @PlayerClass
                                                                 FROM DUAL
                                                                 WHERE NOT EXISTS (SELECT 1 FROM raiting_total WHERE PlayerName = @PlayerName);";

                                                    using (MySqlCommand command = new MySqlCommand(query, connection))
                                                    {
                                                        command.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        command.Parameters.AddWithValue("@PlayerRaiting", 0);
                                                        command.Parameters.AddWithValue("@PlayerClass", spanClasses[h]);

                                                        command.ExecuteNonQuery();
                                                    }

                                                    connectionplayer.Close();
                                                }


                                                using (MySqlConnection connection_sd = new MySqlConnection(connectionString))
                                                {
                                                    connection_sd.Open();
                                                    using (MySqlCommand cmd_sd_raiting = new MySqlCommand("SELECT Raiting FROM playerlist_pf_tyra WHERE PlayerName = '" + playernames[h] + "' ORDER BY Raiting DESC LIMIT 1", connection_sd))
                                                    {
                                                        cmd_sd_raiting.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        object result_sd_raiting = cmd_sd_raiting.ExecuteScalar();

                                                        double old_raiting = 0.0;

                                                        if (result_sd_raiting == DBNull.Value || result_sd_raiting == null)
                                                        {
                                                            old_raiting = 0.0;
                                                        }
                                                        else
                                                        {
                                                            old_raiting = (double)result_sd_raiting;
                                                        }


                                                        if (raiting > old_raiting)
                                                        {
                                                            string sql2 = "REPLACE INTO playerlist_pf_tyra(PlayerName, PlayerRaiting, PlayerClass ,MythicLevel, Time, Groups, Completed, Raiting) VALUES (@value1, @value2, @value3, @value4, @value5, @value6 ,@value7, @value8)";
                                                            MySqlCommand cmd2 = new MySqlCommand(sql2, connection);
                                                            cmd2.Parameters.AddWithValue("@value1", playernames[h]);
                                                            cmd2.Parameters.AddWithValue("@value2", raiting);
                                                            cmd2.Parameters.AddWithValue("@value3", spanClasses[h]);
                                                            cmd2.Parameters.AddWithValue("@value4", realdata[35] + "" + plus);
                                                            cmd2.Parameters.AddWithValue("@value5", realdata[53]);
                                                            cmd2.Parameters.AddWithValue("@value6", group);
                                                            cmd2.Parameters.AddWithValue("@value7", vrijeme);
                                                            cmd2.Parameters.AddWithValue("@value8", raiting);
                                                            cmd2.ExecuteNonQuery();

                                                        }

                                                    }
                                                    connection_sd.Close();
                                                }

                                            }
                                        }
                                        dungeon = "PF";
                                    }
                                    else if (j == 5)
                                    {
                                        string weburl = "https://firestorm-servers.com/en/challenge/index";
                                        HtmlWeb web = new HtmlWeb();
                                        HtmlDocument doc1 = web.Load(weburl);
                                        HtmlNode div = doc1.DocumentNode.SelectSingleNode("//div[@class='mutator' and (contains(., 'Fortified') or contains(., 'Tyrannical'))]");
                                        string innerText = div.InnerText.Trim();


                                        string sql = "";
                                        if (innerText == "Fortified")
                                        {
                                            sql = "REPLACE INTO nw_forti (Rank, MythicLevel, Time, Groups, Completed, Raiting, InsertTime) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                                        }
                                        else
                                        {
                                            sql = "REPLACE INTO nw_tyra (Rank, MythicLevel, Time, Groups, Completed, Raiting, InsertTime) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                                        }
                                        MySqlCommand cmd = new MySqlCommand(sql, connection);

                                        string test = Regex.Unescape(data[i]);



                                        string[] realdata = test.Split(" ");


                                        realdata[17] = realdata[17].Trim();
                                        realdata[35] = realdata[35].Trim();
                                        realdata[53] = realdata[53].Trim();
                                        string group = realdata[71] + " " + realdata[72] + " " + realdata[73] + " " + realdata[74] + " " + realdata[75];
                                        string vrijeme = realdata[93] + " " + realdata[94] + " " + realdata[95] + realdata[96];
                                        string[] values = { realdata[53], "00:36:00", "00:28:48", "00:21:36", realdata[35] };
                                        string raiting_multiplier = raitingcalculation(values);
                                        string[] raitingdata = raiting_multiplier.Split(' ');

                                        double raiting = double.Parse(raitingdata[0]);

                                        string plus = raitingdata[1];
                                        int rank = Int32.Parse(realdata[17]);
                                        cmd.Parameters.AddWithValue("@value1", rank);
                                        cmd.Parameters.AddWithValue("@value2", realdata[35] + "" + plus);
                                        cmd.Parameters.AddWithValue("@value3", realdata[53]);
                                        cmd.Parameters.AddWithValue("@value4", group);
                                        cmd.Parameters.AddWithValue("@value5", vrijeme);


                                        cmd.Parameters.AddWithValue("@value6", raiting);

                                        DateTime dateTimeVariable = DateTime.Now;
                                        cmd.Parameters.AddWithValue("@value7", dateTimeVariable);
                                        cmd.ExecuteNonQuery();


                                        string[] playernames = { realdata[71], realdata[72], realdata[73], realdata[74], realdata[75] };


                                        if (innerText == "Fortified")
                                        {
                                            for (int h = 0; h < playernames.Length; h++)
                                            {

                                                using (MySqlConnection connectionplayer = new MySqlConnection(connectionString))
                                                {
                                                    connectionplayer.Open();

                                                    string query = @"INSERT INTO raiting_total (PlayerName, PlayerRaiting, PlayerClass)
                                                                 SELECT @PlayerName, @PlayerRaiting, @PlayerClass
                                                                 FROM DUAL
                                                                 WHERE NOT EXISTS (SELECT 1 FROM raiting_total WHERE PlayerName = @PlayerName);";

                                                    using (MySqlCommand command = new MySqlCommand(query, connection))
                                                    {
                                                        command.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        command.Parameters.AddWithValue("@PlayerRaiting", 0);
                                                        command.Parameters.AddWithValue("@PlayerClass", spanClasses[h]);

                                                        command.ExecuteNonQuery();
                                                    }

                                                    connectionplayer.Close();
                                                }

                                                using (MySqlConnection connection_sd = new MySqlConnection(connectionString))
                                                {
                                                    connection_sd.Open();
                                                    using (MySqlCommand cmd_sd_raiting = new MySqlCommand("SELECT Raiting FROM playerlist_nw_forti WHERE PlayerName = '" + playernames[h] + "' ORDER BY Raiting DESC LIMIT 1", connection_sd))
                                                    {
                                                        cmd_sd_raiting.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        object result_sd_raiting = cmd_sd_raiting.ExecuteScalar();

                                                        double old_raiting = 0.0;

                                                        if (result_sd_raiting == DBNull.Value || result_sd_raiting == null)
                                                        {
                                                            old_raiting = 0.0;
                                                        }
                                                        else
                                                        {
                                                            old_raiting = (double)result_sd_raiting;
                                                        }


                                                        if (raiting > old_raiting)
                                                        {
                                                            string sql2 = "REPLACE INTO playerlist_nw_forti(PlayerName, PlayerRaiting, PlayerClass ,MythicLevel, Time, Groups, Completed, Raiting) VALUES (@value1, @value2, @value3, @value4, @value5, @value6 ,@value7, @value8)";
                                                            MySqlCommand cmd2 = new MySqlCommand(sql2, connection);
                                                            cmd2.Parameters.AddWithValue("@value1", playernames[h]);
                                                            cmd2.Parameters.AddWithValue("@value2", raiting);
                                                            cmd2.Parameters.AddWithValue("@value3", spanClasses[h]);
                                                            cmd2.Parameters.AddWithValue("@value4", realdata[35] + "" + plus);
                                                            cmd2.Parameters.AddWithValue("@value5", realdata[53]);
                                                            cmd2.Parameters.AddWithValue("@value6", group);
                                                            cmd2.Parameters.AddWithValue("@value7", vrijeme);
                                                            cmd2.Parameters.AddWithValue("@value8", raiting);
                                                            cmd2.ExecuteNonQuery();

                                                        }

                                                    }
                                                    connection_sd.Close();
                                                }

                                            }
                                        }
                                        else
                                        {
                                            for (int h = 0; h < playernames.Length; h++)
                                            {

                                                using (MySqlConnection connectionplayer = new MySqlConnection(connectionString))
                                                {
                                                    connectionplayer.Open();

                                                    string query = @"INSERT INTO raiting_total (PlayerName, PlayerRaiting, PlayerClass)
                                                                 SELECT @PlayerName, @PlayerRaiting, @PlayerClass
                                                                 FROM DUAL
                                                                 WHERE NOT EXISTS (SELECT 1 FROM raiting_total WHERE PlayerName = @PlayerName);";

                                                    using (MySqlCommand command = new MySqlCommand(query, connection))
                                                    {
                                                        command.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        command.Parameters.AddWithValue("@PlayerRaiting", 0);
                                                        command.Parameters.AddWithValue("@PlayerClass", spanClasses[h]);

                                                        command.ExecuteNonQuery();
                                                    }

                                                    connectionplayer.Close();
                                                }


                                                using (MySqlConnection connection_sd = new MySqlConnection(connectionString))
                                                {
                                                    connection_sd.Open();
                                                    using (MySqlCommand cmd_sd_raiting = new MySqlCommand("SELECT Raiting FROM playerlist_nw_tyra WHERE PlayerName = '" + playernames[h] + "' ORDER BY Raiting DESC LIMIT 1", connection_sd))
                                                    {
                                                        cmd_sd_raiting.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        object result_sd_raiting = cmd_sd_raiting.ExecuteScalar();

                                                        double old_raiting = 0.0;

                                                        if (result_sd_raiting == DBNull.Value || result_sd_raiting == null)
                                                        {
                                                            old_raiting = 0.0;
                                                        }
                                                        else
                                                        {
                                                            old_raiting = (double)result_sd_raiting;
                                                        }


                                                        if (raiting > old_raiting)
                                                        {
                                                            string sql2 = "REPLACE INTO playerlist_nw_tyra(PlayerName, PlayerRaiting, PlayerClass ,MythicLevel, Time, Groups, Completed, Raiting) VALUES (@value1, @value2, @value3, @value4, @value5, @value6 ,@value7, @value8)";
                                                            MySqlCommand cmd2 = new MySqlCommand(sql2, connection);
                                                            cmd2.Parameters.AddWithValue("@value1", playernames[h]);
                                                            cmd2.Parameters.AddWithValue("@value2", raiting);
                                                            cmd2.Parameters.AddWithValue("@value3", spanClasses[h]);
                                                            cmd2.Parameters.AddWithValue("@value4", realdata[35] + "" + plus);
                                                            cmd2.Parameters.AddWithValue("@value5", realdata[53]);
                                                            cmd2.Parameters.AddWithValue("@value6", group);
                                                            cmd2.Parameters.AddWithValue("@value7", vrijeme);
                                                            cmd2.Parameters.AddWithValue("@value8", raiting);
                                                            cmd2.ExecuteNonQuery();

                                                        }

                                                    }
                                                    connection_sd.Close();
                                                }

                                            }
                                        }
                                        dungeon = "NW";
                                    }
                                    else if (j == 6)
                                    {
                                        string weburl = "https://firestorm-servers.com/en/challenge/index";
                                        HtmlWeb web = new HtmlWeb();
                                        HtmlDocument doc1 = web.Load(weburl);
                                        HtmlNode div = doc1.DocumentNode.SelectSingleNode("//div[@class='mutator' and (contains(., 'Fortified') or contains(., 'Tyrannical'))]");
                                        string innerText = div.InnerText.Trim();


                                        string sql = "";
                                        if (innerText == "Fortified")
                                        {
                                            sql = "REPLACE INTO dos_forti (Rank, MythicLevel, Time, Groups, Completed, Raiting, InsertTime) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                                        }
                                        else
                                        {
                                            sql = "REPLACE INTO dos_tyra (Rank, MythicLevel, Time, Groups, Completed, Raiting, InsertTime) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                                        }
                                        MySqlCommand cmd = new MySqlCommand(sql, connection);

                                        string test = Regex.Unescape(data[i]);



                                        string[] realdata = test.Split(" ");


                                        realdata[17] = realdata[17].Trim();
                                        realdata[35] = realdata[35].Trim();
                                        realdata[53] = realdata[53].Trim();
                                        string group = realdata[71] + " " + realdata[72] + " " + realdata[73] + " " + realdata[74] + " " + realdata[75];
                                        string vrijeme = realdata[93] + " " + realdata[94] + " " + realdata[95] + realdata[96];
                                        string[] values = { realdata[53], "00:43:00", "00:34:24", "00:25:48", realdata[35] };
                                        string raiting_multiplier = raitingcalculation(values);
                                        string[] raitingdata = raiting_multiplier.Split(' ');

                                        double raiting = double.Parse(raitingdata[0]);

                                        string plus = raitingdata[1];
                                        int rank = Int32.Parse(realdata[17]);
                                        cmd.Parameters.AddWithValue("@value1", rank);
                                        cmd.Parameters.AddWithValue("@value2", realdata[35] + "" + plus);
                                        cmd.Parameters.AddWithValue("@value3", realdata[53]);
                                        cmd.Parameters.AddWithValue("@value4", group);
                                        cmd.Parameters.AddWithValue("@value5", vrijeme);


                                        cmd.Parameters.AddWithValue("@value6", raiting);

                                        DateTime dateTimeVariable = DateTime.Now;
                                        cmd.Parameters.AddWithValue("@value7", dateTimeVariable);
                                        cmd.ExecuteNonQuery();



                                        string[] playernames = { realdata[71], realdata[72], realdata[73], realdata[74], realdata[75] };


                                        if (innerText == "Fortified")
                                        {
                                            for (int h = 0; h < playernames.Length; h++)
                                            {


                                                using (MySqlConnection connectionplayer = new MySqlConnection(connectionString))
                                                {
                                                    connectionplayer.Open();

                                                    string query = @"INSERT INTO raiting_total (PlayerName, PlayerRaiting, PlayerClass)
                                                                 SELECT @PlayerName, @PlayerRaiting, @PlayerClass
                                                                 FROM DUAL
                                                                 WHERE NOT EXISTS (SELECT 1 FROM raiting_total WHERE PlayerName = @PlayerName);";

                                                    using (MySqlCommand command = new MySqlCommand(query, connection))
                                                    {
                                                        command.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        command.Parameters.AddWithValue("@PlayerRaiting", 0);
                                                        command.Parameters.AddWithValue("@PlayerClass", spanClasses[h]);

                                                        command.ExecuteNonQuery();
                                                    }

                                                    connectionplayer.Close();
                                                }


                                                using (MySqlConnection connection_sd = new MySqlConnection(connectionString))
                                                {
                                                    connection_sd.Open();
                                                    using (MySqlCommand cmd_sd_raiting = new MySqlCommand("SELECT Raiting FROM playerlist_dos_forti WHERE PlayerName = '" + playernames[h] + "' ORDER BY Raiting DESC LIMIT 1", connection_sd))
                                                    {
                                                        cmd_sd_raiting.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        object result_sd_raiting = cmd_sd_raiting.ExecuteScalar();

                                                        double old_raiting = 0.0;

                                                        if (result_sd_raiting == DBNull.Value || result_sd_raiting == null)
                                                        {
                                                            old_raiting = 0.0;
                                                        }
                                                        else
                                                        {
                                                            old_raiting = (double)result_sd_raiting;
                                                        }


                                                        if (raiting > old_raiting)
                                                        {
                                                            string sql2 = "REPLACE INTO playerlist_dos_forti(PlayerName, PlayerRaiting, PlayerClass ,MythicLevel, Time, Groups, Completed, Raiting) VALUES (@value1, @value2, @value3, @value4, @value5, @value6 ,@value7, @value8)";
                                                            MySqlCommand cmd2 = new MySqlCommand(sql2, connection);
                                                            cmd2.Parameters.AddWithValue("@value1", playernames[h]);
                                                            cmd2.Parameters.AddWithValue("@value2", raiting);
                                                            cmd2.Parameters.AddWithValue("@value3", spanClasses[h]);
                                                            cmd2.Parameters.AddWithValue("@value4", realdata[35] + "" + plus);
                                                            cmd2.Parameters.AddWithValue("@value5", realdata[53]);
                                                            cmd2.Parameters.AddWithValue("@value6", group);
                                                            cmd2.Parameters.AddWithValue("@value7", vrijeme);
                                                            cmd2.Parameters.AddWithValue("@value8", raiting);
                                                            cmd2.ExecuteNonQuery();

                                                        }

                                                    }
                                                    connection_sd.Close();
                                                }

                                            }
                                        }
                                        else
                                        {
                                            for (int h = 0; h < playernames.Length; h++)
                                            {


                                                using (MySqlConnection connectionplayer = new MySqlConnection(connectionString))
                                                {
                                                    connectionplayer.Open();

                                                    string query = @"INSERT INTO raiting_total (PlayerName, PlayerRaiting, PlayerClass)
                                                                 SELECT @PlayerName, @PlayerRaiting, @PlayerClass
                                                                 FROM DUAL
                                                                 WHERE NOT EXISTS (SELECT 1 FROM raiting_total WHERE PlayerName = @PlayerName);";

                                                    using (MySqlCommand command = new MySqlCommand(query, connection))
                                                    {
                                                        command.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        command.Parameters.AddWithValue("@PlayerRaiting", 0);
                                                        command.Parameters.AddWithValue("@PlayerClass", spanClasses[h]);

                                                        command.ExecuteNonQuery();
                                                    }

                                                    connectionplayer.Close();
                                                }


                                                using (MySqlConnection connection_sd = new MySqlConnection(connectionString))
                                                {
                                                    connection_sd.Open();
                                                    using (MySqlCommand cmd_sd_raiting = new MySqlCommand("SELECT Raiting FROM playerlist_dos_tyra WHERE PlayerName = '" + playernames[h] + "' ORDER BY Raiting DESC LIMIT 1", connection_sd))
                                                    {
                                                        cmd_sd_raiting.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        object result_sd_raiting = cmd_sd_raiting.ExecuteScalar();

                                                        double old_raiting = 0.0;

                                                        if (result_sd_raiting == DBNull.Value || result_sd_raiting == null)
                                                        {
                                                            old_raiting = 0.0;
                                                        }
                                                        else
                                                        {
                                                            old_raiting = (double)result_sd_raiting;
                                                        }


                                                        if (raiting > old_raiting)
                                                        {
                                                            string sql2 = "REPLACE INTO playerlist_dos_tyra(PlayerName, PlayerRaiting, PlayerClass ,MythicLevel, Time, Groups, Completed, Raiting) VALUES (@value1, @value2, @value3, @value4, @value5, @value6 ,@value7, @value8)";
                                                            MySqlCommand cmd2 = new MySqlCommand(sql2, connection);
                                                            cmd2.Parameters.AddWithValue("@value1", playernames[h]);
                                                            cmd2.Parameters.AddWithValue("@value2", raiting);
                                                            cmd2.Parameters.AddWithValue("@value3", spanClasses[h]);
                                                            cmd2.Parameters.AddWithValue("@value4", realdata[35] + "" + plus);
                                                            cmd2.Parameters.AddWithValue("@value5", realdata[53]);
                                                            cmd2.Parameters.AddWithValue("@value6", group);
                                                            cmd2.Parameters.AddWithValue("@value7", vrijeme);
                                                            cmd2.Parameters.AddWithValue("@value8", raiting);
                                                            cmd2.ExecuteNonQuery();

                                                        }

                                                    }
                                                    connection_sd.Close();
                                                }

                                            }
                                        }
                                        dungeon = "DOS";
                                    }
                                    else if (j == 7)
                                    {
                                        string weburl = "https://firestorm-servers.com/en/challenge/index";
                                        HtmlWeb web = new HtmlWeb();
                                        HtmlDocument doc1 = web.Load(weburl);
                                        HtmlNode div = doc1.DocumentNode.SelectSingleNode("//div[@class='mutator' and (contains(., 'Fortified') or contains(., 'Tyrannical'))]");
                                        string innerText = div.InnerText.Trim();


                                        string sql = "";
                                        if (innerText == "Fortified")
                                        {
                                            sql = "REPLACE INTO top_forti (Rank, MythicLevel, Time, Groups, Completed, Raiting, InsertTime) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                                        }
                                        else
                                        {
                                            sql = "REPLACE INTO top_tyra (Rank, MythicLevel, Time, Groups, Completed, Raiting, InsertTime) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                                        }
                                        MySqlCommand cmd = new MySqlCommand(sql, connection);

                                        string test = Regex.Unescape(data[i]);



                                        string[] realdata = test.Split(" ");

                                        realdata[17] = realdata[17].Trim();
                                        realdata[35] = realdata[35].Trim();
                                        realdata[53] = realdata[53].Trim();
                                        string group = realdata[71] + " " + realdata[72] + " " + realdata[73] + " " + realdata[74] + " " + realdata[75];
                                        string vrijeme = realdata[93] + " " + realdata[94] + " " + realdata[95] + realdata[96];
                                        string[] values = { realdata[53], "00:38:00", "00:30:24", "00:22:48", realdata[35] };
                                        string raiting_multiplier = raitingcalculation(values);
                                        string[] raitingdata = raiting_multiplier.Split(' ');

                                        double raiting = double.Parse(raitingdata[0]);

                                        string plus = raitingdata[1];
                                        int rank = Int32.Parse(realdata[17]);
                                        cmd.Parameters.AddWithValue("@value1", rank);
                                        cmd.Parameters.AddWithValue("@value2", realdata[35] + "" + plus);
                                        cmd.Parameters.AddWithValue("@value3", realdata[53]);
                                        cmd.Parameters.AddWithValue("@value4", group);
                                        cmd.Parameters.AddWithValue("@value5", vrijeme);


                                        cmd.Parameters.AddWithValue("@value6", raiting);

                                        DateTime dateTimeVariable = DateTime.Now;
                                        cmd.Parameters.AddWithValue("@value7", dateTimeVariable);
                                        cmd.ExecuteNonQuery();



                                        string[] playernames = { realdata[71], realdata[72], realdata[73], realdata[74], realdata[75] };


                                        if (innerText == "Fortified")
                                        {
                                            for (int h = 0; h < playernames.Length; h++)
                                            {

                                                using (MySqlConnection connectionplayer = new MySqlConnection(connectionString))
                                                {
                                                    connectionplayer.Open();

                                                    string query = @"INSERT INTO raiting_total (PlayerName, PlayerRaiting, PlayerClass)
                                                                 SELECT @PlayerName, @PlayerRaiting, @PlayerClass
                                                                 FROM DUAL
                                                                 WHERE NOT EXISTS (SELECT 1 FROM raiting_total WHERE PlayerName = @PlayerName);";

                                                    using (MySqlCommand command = new MySqlCommand(query, connection))
                                                    {
                                                        command.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        command.Parameters.AddWithValue("@PlayerRaiting", 0);
                                                        command.Parameters.AddWithValue("@PlayerClass", spanClasses[h]);

                                                        command.ExecuteNonQuery();
                                                    }

                                                    connectionplayer.Close();
                                                }


                                                using (MySqlConnection connection_sd = new MySqlConnection(connectionString))
                                                {
                                                    connection_sd.Open();
                                                    using (MySqlCommand cmd_sd_raiting = new MySqlCommand("SELECT Raiting FROM playerlist_top_forti WHERE PlayerName = '" + playernames[h] + "' ORDER BY Raiting DESC LIMIT 1", connection_sd))
                                                    {
                                                        cmd_sd_raiting.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        object result_sd_raiting = cmd_sd_raiting.ExecuteScalar();

                                                        double old_raiting = 0.0;

                                                        if (result_sd_raiting == DBNull.Value || result_sd_raiting == null)
                                                        {
                                                            old_raiting = 0.0;
                                                        }
                                                        else
                                                        {
                                                            old_raiting = (double)result_sd_raiting;
                                                        }


                                                        if (raiting > old_raiting)
                                                        {
                                                            string sql2 = "REPLACE INTO playerlist_top_forti(PlayerName, PlayerRaiting, PlayerClass ,MythicLevel, Time, Groups, Completed, Raiting) VALUES (@value1, @value2, @value3, @value4, @value5, @value6 ,@value7, @value8)";
                                                            MySqlCommand cmd2 = new MySqlCommand(sql2, connection);
                                                            cmd2.Parameters.AddWithValue("@value1", playernames[h]);
                                                            cmd2.Parameters.AddWithValue("@value2", raiting);
                                                            cmd2.Parameters.AddWithValue("@value3", spanClasses[h]);
                                                            cmd2.Parameters.AddWithValue("@value4", realdata[35] + "" + plus);
                                                            cmd2.Parameters.AddWithValue("@value5", realdata[53]);
                                                            cmd2.Parameters.AddWithValue("@value6", group);
                                                            cmd2.Parameters.AddWithValue("@value7", vrijeme);
                                                            cmd2.Parameters.AddWithValue("@value8", raiting);
                                                            cmd2.ExecuteNonQuery();

                                                        }

                                                    }
                                                    connection_sd.Close();
                                                }

                                            }
                                        }
                                        else
                                        {
                                            for (int h = 0; h < playernames.Length; h++)
                                            {

                                                using (MySqlConnection connectionplayer = new MySqlConnection(connectionString))
                                                {
                                                    connectionplayer.Open();

                                                    string query = @"INSERT INTO raiting_total (PlayerName, PlayerRaiting, PlayerClass)
                                                                 SELECT @PlayerName, @PlayerRaiting, @PlayerClass
                                                                 FROM DUAL
                                                                 WHERE NOT EXISTS (SELECT 1 FROM raiting_total WHERE PlayerName = @PlayerName);";

                                                    using (MySqlCommand command = new MySqlCommand(query, connection))
                                                    {
                                                        command.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        command.Parameters.AddWithValue("@PlayerRaiting", 0);
                                                        command.Parameters.AddWithValue("@PlayerClass", spanClasses[h]);

                                                        command.ExecuteNonQuery();
                                                    }

                                                    connectionplayer.Close();
                                                }


                                                using (MySqlConnection connection_sd = new MySqlConnection(connectionString))
                                                {
                                                    connection_sd.Open();
                                                    using (MySqlCommand cmd_sd_raiting = new MySqlCommand("SELECT Raiting FROM playerlist_top_tyra WHERE PlayerName = '" + playernames[h] + "' ORDER BY Raiting DESC LIMIT 1", connection_sd))
                                                    {
                                                        cmd_sd_raiting.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        object result_sd_raiting = cmd_sd_raiting.ExecuteScalar();

                                                        double old_raiting = 0.0;

                                                        if (result_sd_raiting == DBNull.Value || result_sd_raiting == null)
                                                        {
                                                            old_raiting = 0.0;
                                                        }
                                                        else
                                                        {
                                                            old_raiting = (double)result_sd_raiting;
                                                        }


                                                        if (raiting > old_raiting)
                                                        {
                                                            string sql2 = "REPLACE INTO playerlist_top_tyra(PlayerName, PlayerRaiting, PlayerClass ,MythicLevel, Time, Groups, Completed, Raiting) VALUES (@value1, @value2, @value3, @value4, @value5, @value6 ,@value7, @value8)";
                                                            MySqlCommand cmd2 = new MySqlCommand(sql2, connection);
                                                            cmd2.Parameters.AddWithValue("@value1", playernames[h]);
                                                            cmd2.Parameters.AddWithValue("@value2", raiting);
                                                            cmd2.Parameters.AddWithValue("@value3", spanClasses[h]);
                                                            cmd2.Parameters.AddWithValue("@value4", realdata[35] + "" + plus);
                                                            cmd2.Parameters.AddWithValue("@value5", realdata[53]);
                                                            cmd2.Parameters.AddWithValue("@value6", group);
                                                            cmd2.Parameters.AddWithValue("@value7", vrijeme);
                                                            cmd2.Parameters.AddWithValue("@value8", raiting);
                                                            cmd2.ExecuteNonQuery();

                                                        }

                                                    }
                                                    connection_sd.Close();
                                                }

                                            }
                                        }
                                        dungeon = "TOP";
                                    }
                                    else if (j == 8)
                                    {
                                        string weburl = "https://firestorm-servers.com/en/challenge/index";
                                        HtmlWeb web = new HtmlWeb();
                                        HtmlDocument doc1 = web.Load(weburl);
                                        HtmlNode div = doc1.DocumentNode.SelectSingleNode("//div[@class='mutator' and (contains(., 'Fortified') or contains(., 'Tyrannical'))]");
                                        string innerText = div.InnerText.Trim();


                                        string sql = "";
                                        if (innerText == "Fortified")
                                        {
                                            sql = "REPLACE INTO sg_forti (Rank, MythicLevel, Time, Groups, Completed, Raiting, InsertTime) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                                        }
                                        else
                                        {
                                            sql = "REPLACE INTO sg_tyra (Rank, MythicLevel, Time, Groups, Completed, Raiting, InsertTime) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                                        }
                                        MySqlCommand cmd = new MySqlCommand(sql, connection);

                                        string test = Regex.Unescape(data[i]);



                                        string[] realdata = test.Split(" ");



                                        realdata[17] = realdata[17].Trim();
                                        realdata[35] = realdata[35].Trim();
                                        realdata[53] = realdata[53].Trim();
                                        string group = realdata[71] + " " + realdata[72] + " " + realdata[73] + " " + realdata[74] + " " + realdata[75];
                                        string vrijeme = realdata[93] + " " + realdata[94] + " " + realdata[95] + realdata[96];
                                        string[] values = { realdata[53], "00:30:00", "00:24:00", "00:18:00", realdata[35] };
                                        string raiting_multiplier = raitingcalculation(values);
                                        string[] raitingdata = raiting_multiplier.Split(' ');

                                        double raiting = double.Parse(raitingdata[0]);

                                        string plus = raitingdata[1];
                                        int rank = Int32.Parse(realdata[17]);
                                        cmd.Parameters.AddWithValue("@value1", rank);
                                        cmd.Parameters.AddWithValue("@value2", realdata[35] + "" + plus);
                                        cmd.Parameters.AddWithValue("@value3", realdata[53]);
                                        cmd.Parameters.AddWithValue("@value4", group);
                                        cmd.Parameters.AddWithValue("@value5", vrijeme);


                                        cmd.Parameters.AddWithValue("@value6", raiting);

                                        DateTime dateTimeVariable = DateTime.Now;
                                        cmd.Parameters.AddWithValue("@value7", dateTimeVariable);
                                        cmd.ExecuteNonQuery();



                                        string[] playernames = { realdata[71], realdata[72], realdata[73], realdata[74], realdata[75] };


                                        if (innerText == "Fortified")
                                        {
                                            for (int h = 0; h < playernames.Length; h++)
                                            {

                                                using (MySqlConnection connectionplayer = new MySqlConnection(connectionString))
                                                {
                                                    connectionplayer.Open();

                                                    string query = @"INSERT INTO raiting_total (PlayerName, PlayerRaiting, PlayerClass)
                                                                 SELECT @PlayerName, @PlayerRaiting, @PlayerClass
                                                                 FROM DUAL
                                                                 WHERE NOT EXISTS (SELECT 1 FROM raiting_total WHERE PlayerName = @PlayerName);";

                                                    using (MySqlCommand command = new MySqlCommand(query, connection))
                                                    {
                                                        command.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        command.Parameters.AddWithValue("@PlayerRaiting", 0);
                                                        command.Parameters.AddWithValue("@PlayerClass", spanClasses[h]);

                                                        command.ExecuteNonQuery();
                                                    }

                                                    connectionplayer.Close();
                                                }


                                                using (MySqlConnection connection_sd = new MySqlConnection(connectionString))
                                                {
                                                    connection_sd.Open();
                                                    using (MySqlCommand cmd_sd_raiting = new MySqlCommand("SELECT Raiting FROM playerlist_sg_forti WHERE PlayerName = '" + playernames[h] + "' ORDER BY Raiting DESC LIMIT 1", connection_sd))
                                                    {
                                                        cmd_sd_raiting.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        object result_sd_raiting = cmd_sd_raiting.ExecuteScalar();

                                                        double old_raiting = 0.0;

                                                        if (result_sd_raiting == DBNull.Value || result_sd_raiting == null)
                                                        {
                                                            old_raiting = 0.0;
                                                        }
                                                        else
                                                        {
                                                            old_raiting = (double)result_sd_raiting;
                                                        }


                                                        if (raiting > old_raiting)
                                                        {
                                                            string sql2 = "REPLACE INTO playerlist_sg_forti(PlayerName, PlayerRaiting, PlayerClass ,MythicLevel, Time, Groups, Completed, Raiting) VALUES (@value1, @value2, @value3, @value4, @value5, @value6 ,@value7, @value8)";
                                                            MySqlCommand cmd2 = new MySqlCommand(sql2, connection);
                                                            cmd2.Parameters.AddWithValue("@value1", playernames[h]);
                                                            cmd2.Parameters.AddWithValue("@value2", raiting);
                                                            cmd2.Parameters.AddWithValue("@value3", spanClasses[h]);
                                                            cmd2.Parameters.AddWithValue("@value4", realdata[35] + "" + plus);
                                                            cmd2.Parameters.AddWithValue("@value5", realdata[53]);
                                                            cmd2.Parameters.AddWithValue("@value6", group);
                                                            cmd2.Parameters.AddWithValue("@value7", vrijeme);
                                                            cmd2.Parameters.AddWithValue("@value8", raiting);
                                                            cmd2.ExecuteNonQuery();

                                                        }

                                                    }
                                                    connection_sd.Close();
                                                }

                                            }
                                        }

                                        else
                                        {
                                            for (int h = 0; h < playernames.Length; h++)
                                            {

                                                using (MySqlConnection connectionplayer = new MySqlConnection(connectionString))
                                                {
                                                    connectionplayer.Open();

                                                    string query = @"INSERT INTO raiting_total (PlayerName, PlayerRaiting, PlayerClass)
                                                                 SELECT @PlayerName, @PlayerRaiting, @PlayerClass
                                                                 FROM DUAL
                                                                 WHERE NOT EXISTS (SELECT 1 FROM raiting_total WHERE PlayerName = @PlayerName);";

                                                    using (MySqlCommand command = new MySqlCommand(query, connection))
                                                    {
                                                        command.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        command.Parameters.AddWithValue("@PlayerRaiting", 0);
                                                        command.Parameters.AddWithValue("@PlayerClass", spanClasses[h]);

                                                        command.ExecuteNonQuery();
                                                    }

                                                    connectionplayer.Close();
                                                }


                                                using (MySqlConnection connection_sd = new MySqlConnection(connectionString))
                                                {
                                                    connection_sd.Open();
                                                    using (MySqlCommand cmd_sd_raiting = new MySqlCommand("SELECT Raiting FROM playerlist_sg_tyra WHERE PlayerName = '" + playernames[h] + "' ORDER BY Raiting DESC LIMIT 1", connection_sd))
                                                    {
                                                        cmd_sd_raiting.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        object result_sd_raiting = cmd_sd_raiting.ExecuteScalar();

                                                        double old_raiting = 0.0;

                                                        if (result_sd_raiting == DBNull.Value || result_sd_raiting == null)
                                                        {
                                                            old_raiting = 0.0;
                                                        }
                                                        else
                                                        {
                                                            old_raiting = (double)result_sd_raiting;
                                                        }


                                                        if (raiting > old_raiting)
                                                        {
                                                            string sql2 = "REPLACE INTO playerlist_sg_tyra(PlayerName, PlayerRaiting, PlayerClass ,MythicLevel, Time, Groups, Completed, Raiting) VALUES (@value1, @value2, @value3, @value4, @value5, @value6 ,@value7, @value8)";
                                                            MySqlCommand cmd2 = new MySqlCommand(sql2, connection);
                                                            cmd2.Parameters.AddWithValue("@value1", playernames[h]);
                                                            cmd2.Parameters.AddWithValue("@value2", raiting);
                                                            cmd2.Parameters.AddWithValue("@value3", spanClasses[h]);
                                                            cmd2.Parameters.AddWithValue("@value4", realdata[35] + "" + plus);
                                                            cmd2.Parameters.AddWithValue("@value5", realdata[53]);
                                                            cmd2.Parameters.AddWithValue("@value6", group);
                                                            cmd2.Parameters.AddWithValue("@value7", vrijeme);
                                                            cmd2.Parameters.AddWithValue("@value8", raiting);
                                                            cmd2.ExecuteNonQuery();

                                                        }

                                                    }
                                                    connection_sd.Close();
                                                }

                                            }
                                        }
                                        dungeon = "SG";
                                    }
                                    else if (j == 9)
                                    {
                                        string weburl = "https://firestorm-servers.com/en/challenge/index";
                                        HtmlWeb web = new HtmlWeb();
                                        HtmlDocument doc1 = web.Load(weburl);
                                        HtmlNode div = doc1.DocumentNode.SelectSingleNode("//div[@class='mutator' and (contains(., 'Fortified') or contains(., 'Tyrannical'))]");
                                        string innerText = div.InnerText.Trim();


                                        string sql = "";
                                        if (innerText == "Fortified")
                                        {
                                            sql = "REPLACE INTO sow_forti (Rank, MythicLevel, Time, Groups, Completed, Raiting, InsertTime) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                                        }
                                        else
                                        {
                                            sql = "REPLACE INTO sow_tyra (Rank, MythicLevel, Time, Groups, Completed, Raiting, InsertTime) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                                        }
                                        MySqlCommand cmd = new MySqlCommand(sql, connection);

                                        string test = Regex.Unescape(data[i]);



                                        string[] realdata = test.Split(" ");



                                        realdata[17] = realdata[17].Trim();
                                        realdata[35] = realdata[35].Trim();
                                        realdata[53] = realdata[53].Trim();
                                        string group = realdata[71] + " " + realdata[72] + " " + realdata[73] + " " + realdata[74] + " " + realdata[75];
                                        string vrijeme = realdata[93] + " " + realdata[94] + " " + realdata[95] + realdata[96];
                                        string[] values = { realdata[53], "00:39:00", "00:31:12", "00:23:24", realdata[35] };
                                        string raiting_multiplier = raitingcalculation(values);
                                        string[] raitingdata = raiting_multiplier.Split(' ');

                                        double raiting = double.Parse(raitingdata[0]);

                                        string plus = raitingdata[1];
                                        int rank = Int32.Parse(realdata[17]);
                                        cmd.Parameters.AddWithValue("@value1", rank);
                                        cmd.Parameters.AddWithValue("@value2", realdata[35] + "" + plus);
                                        cmd.Parameters.AddWithValue("@value3", realdata[53]);
                                        cmd.Parameters.AddWithValue("@value4", group);
                                        cmd.Parameters.AddWithValue("@value5", vrijeme);


                                        cmd.Parameters.AddWithValue("@value6", raiting);

                                        DateTime dateTimeVariable = DateTime.Now;
                                        cmd.Parameters.AddWithValue("@value7", dateTimeVariable);
                                        cmd.ExecuteNonQuery();



                                        string[] playernames = { realdata[71], realdata[72], realdata[73], realdata[74], realdata[75] };


                                        if (innerText == "Fortified")
                                        {
                                            for (int h = 0; h < playernames.Length; h++)
                                            {

                                                using (MySqlConnection connectionplayer = new MySqlConnection(connectionString))
                                                {
                                                    connectionplayer.Open();

                                                    string query = @"INSERT INTO raiting_total (PlayerName, PlayerRaiting, PlayerClass)
                                                                 SELECT @PlayerName, @PlayerRaiting, @PlayerClass
                                                                 FROM DUAL
                                                                 WHERE NOT EXISTS (SELECT 1 FROM raiting_total WHERE PlayerName = @PlayerName);";

                                                    using (MySqlCommand command = new MySqlCommand(query, connection))
                                                    {
                                                        command.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        command.Parameters.AddWithValue("@PlayerRaiting", 0);
                                                        command.Parameters.AddWithValue("@PlayerClass", spanClasses[h]);

                                                        command.ExecuteNonQuery();
                                                    }

                                                    connectionplayer.Close();
                                                }

                                                using (MySqlConnection connection_sd = new MySqlConnection(connectionString))
                                                {
                                                    connection_sd.Open();
                                                    using (MySqlCommand cmd_sd_raiting = new MySqlCommand("SELECT Raiting FROM playerlist_sow_forti WHERE PlayerName = '" + playernames[h] + "' ORDER BY Raiting DESC LIMIT 1", connection_sd))
                                                    {
                                                        cmd_sd_raiting.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        object result_sd_raiting = cmd_sd_raiting.ExecuteScalar();

                                                        double old_raiting = 0.0;

                                                        if (result_sd_raiting == DBNull.Value || result_sd_raiting == null)
                                                        {
                                                            old_raiting = 0.0;
                                                        }
                                                        else
                                                        {
                                                            old_raiting = (double)result_sd_raiting;
                                                        }


                                                        if (raiting > old_raiting)
                                                        {
                                                            string sql2 = "REPLACE INTO playerlist_sow_forti(PlayerName, PlayerRaiting, PlayerClass ,MythicLevel, Time, Groups, Completed, Raiting) VALUES (@value1, @value2, @value3, @value4, @value5, @value6 ,@value7, @value8)";
                                                            MySqlCommand cmd2 = new MySqlCommand(sql2, connection);
                                                            cmd2.Parameters.AddWithValue("@value1", playernames[h]);
                                                            cmd2.Parameters.AddWithValue("@value2", raiting);
                                                            cmd2.Parameters.AddWithValue("@value3", spanClasses[h]);
                                                            cmd2.Parameters.AddWithValue("@value4", realdata[35] + "" + plus);
                                                            cmd2.Parameters.AddWithValue("@value5", realdata[53]);
                                                            cmd2.Parameters.AddWithValue("@value6", group);
                                                            cmd2.Parameters.AddWithValue("@value7", vrijeme);
                                                            cmd2.Parameters.AddWithValue("@value8", raiting);
                                                            cmd2.ExecuteNonQuery();

                                                        }

                                                    }
                                                    connection_sd.Close();
                                                }

                                            }
                                        }
                                        else
                                        {
                                            for (int h = 0; h < playernames.Length; h++)
                                            {

                                                using (MySqlConnection connectionplayer = new MySqlConnection(connectionString))
                                                {
                                                    connectionplayer.Open();

                                                    string query = @"INSERT INTO raiting_total (PlayerName, PlayerRaiting, PlayerClass)
                                                                 SELECT @PlayerName, @PlayerRaiting, @PlayerClass
                                                                 FROM DUAL
                                                                 WHERE NOT EXISTS (SELECT 1 FROM raiting_total WHERE PlayerName = @PlayerName);";

                                                    using (MySqlCommand command = new MySqlCommand(query, connection))
                                                    {
                                                        command.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        command.Parameters.AddWithValue("@PlayerRaiting", 0);
                                                        command.Parameters.AddWithValue("@PlayerClass", spanClasses[h]);

                                                        command.ExecuteNonQuery();
                                                    }

                                                    connectionplayer.Close();
                                                }


                                                using (MySqlConnection connection_sd = new MySqlConnection(connectionString))
                                                {
                                                    connection_sd.Open();
                                                    using (MySqlCommand cmd_sd_raiting = new MySqlCommand("SELECT Raiting FROM playerlist_sow_tyra WHERE PlayerName = '" + playernames[h] + "' ORDER BY Raiting DESC LIMIT 1", connection_sd))
                                                    {
                                                        cmd_sd_raiting.Parameters.AddWithValue("@PlayerName", playernames[h]);
                                                        object result_sd_raiting = cmd_sd_raiting.ExecuteScalar();

                                                        double old_raiting = 0.0;

                                                        if (result_sd_raiting == DBNull.Value || result_sd_raiting == null)
                                                        {
                                                            old_raiting = 0.0;
                                                        }
                                                        else
                                                        {
                                                            old_raiting = (double)result_sd_raiting;
                                                        }


                                                        if (raiting > old_raiting)
                                                        {
                                                            string sql2 = "REPLACE INTO playerlist_sow_tyra(PlayerName, PlayerRaiting, PlayerClass ,MythicLevel, Time, Groups, Completed, Raiting) VALUES (@value1, @value2, @value3, @value4, @value5, @value6 ,@value7, @value8)";
                                                            MySqlCommand cmd2 = new MySqlCommand(sql2, connection);
                                                            cmd2.Parameters.AddWithValue("@value1", playernames[h]);
                                                            cmd2.Parameters.AddWithValue("@value2", raiting);
                                                            cmd2.Parameters.AddWithValue("@value3", spanClasses[h]);
                                                            cmd2.Parameters.AddWithValue("@value4", realdata[35] + "" + plus);
                                                            cmd2.Parameters.AddWithValue("@value5", realdata[53]);
                                                            cmd2.Parameters.AddWithValue("@value6", group);
                                                            cmd2.Parameters.AddWithValue("@value7", vrijeme);
                                                            cmd2.Parameters.AddWithValue("@value8", raiting);
                                                            cmd2.ExecuteNonQuery();

                                                        }

                                                    }
                                                    connection_sd.Close();
                                                }

                                            }
                                        }

                                        dungeon = "SOW";
                                    }
                                    else
                                    {
                                        string sql = "REPLACE INTO sd (Rank, MythicLevel, Time, Groups, Completed) VALUES (@value1, @value2, @value3, @value4, @value5)";
                                        MySqlCommand cmd = new MySqlCommand(sql, connection);

                                        string test = Regex.Unescape(data[i]);



                                        string[] realdata = test.Split(" ");



                                        realdata[17] = realdata[17].Trim();
                                        realdata[35] = realdata[35].Trim();
                                        realdata[53] = realdata[53].Trim();
                                        string group = realdata[71] + " " + realdata[72] + " " + realdata[73] + " " + realdata[74] + " " + realdata[75];
                                        string vrijeme = realdata[93] + " " + realdata[94] + " " + realdata[95] + realdata[96];

                                        int rank = Int32.Parse(realdata[17]);
                                        cmd.Parameters.AddWithValue("@value1", rank);
                                        cmd.Parameters.AddWithValue("@value2", realdata[35]);
                                        cmd.Parameters.AddWithValue("@value3", realdata[53]);
                                        cmd.Parameters.AddWithValue("@value4", group);
                                        cmd.Parameters.AddWithValue("@value5", vrijeme);
                                        cmd.ExecuteNonQuery();
                                    }


                                }

                            }



                            reader.Close();

                        }

                    }


                    string dos_forti_player_group = "SELECT Raiting FROM dos_forti WHERE Groups  REGEXP CONCAT('(^| )', @playerName, '( |$)') ORDER BY Raiting DESC LIMIT 1";








                    string hoa = "INSERT INTO playerlist_hoa_total (PlayerName, PlayerRaiting, PlayerClass) SELECT PlayerName, ROUND(SUM(PlayerRaiting), 2), PlayerClass FROM ( SELECT PlayerName, PlayerRaiting, PlayerClass FROM playerlist_hoa_forti UNION SELECT PlayerName, PlayerRaiting, PlayerClass FROM playerlist_hoa_tyra ) t GROUP BY PlayerName ON DUPLICATE KEY UPDATE PlayerRaiting = VALUES(PlayerRaiting)";
                    MySqlCommand cmd3 = new MySqlCommand(hoa, connection);
                    cmd3.ExecuteNonQuery();

                    string sd = "INSERT INTO playerlist_sd_total (PlayerName, PlayerRaiting, PlayerClass) SELECT PlayerName, ROUND(SUM(PlayerRaiting), 2), PlayerClass FROM ( SELECT PlayerName, PlayerRaiting, PlayerClass FROM playerlist_sd_forti UNION SELECT PlayerName, PlayerRaiting, PlayerClass FROM playerlist_sd_tyra ) t GROUP BY PlayerName ON DUPLICATE KEY UPDATE PlayerRaiting = VALUES(PlayerRaiting)";
                    MySqlCommand cmd4 = new MySqlCommand(sd, connection);
                    cmd4.ExecuteNonQuery();

                    string soa = "INSERT INTO playerlist_soa_total (PlayerName, PlayerRaiting, PlayerClass) SELECT PlayerName, ROUND(SUM(PlayerRaiting), 2), PlayerClass FROM ( SELECT PlayerName, PlayerRaiting, PlayerClass FROM playerlist_soa_forti UNION SELECT PlayerName, PlayerRaiting, PlayerClass FROM playerlist_soa_tyra ) t GROUP BY PlayerName ON DUPLICATE KEY UPDATE PlayerRaiting = VALUES(PlayerRaiting)";
                    MySqlCommand cmd5 = new MySqlCommand(soa, connection);
                    cmd5.ExecuteNonQuery();

                    string mots = "INSERT INTO playerlist_mots_total (PlayerName, PlayerRaiting, PlayerClass) SELECT PlayerName, ROUND(SUM(PlayerRaiting), 2), PlayerClass FROM ( SELECT PlayerName, PlayerRaiting, PlayerClass FROM playerlist_mots_forti UNION SELECT PlayerName, PlayerRaiting, PlayerClass FROM playerlist_mots_tyra ) t GROUP BY PlayerName ON DUPLICATE KEY UPDATE PlayerRaiting = VALUES(PlayerRaiting)";
                    MySqlCommand cmd6 = new MySqlCommand(mots, connection);
                    cmd6.ExecuteNonQuery();

                    string nw = "INSERT INTO playerlist_nw_total (PlayerName, PlayerRaiting, PlayerClass) SELECT PlayerName, ROUND(SUM(PlayerRaiting), 2), PlayerClass FROM ( SELECT PlayerName, PlayerRaiting, PlayerClass FROM playerlist_nw_forti UNION SELECT PlayerName, PlayerRaiting, PlayerClass FROM playerlist_nw_tyra ) t GROUP BY PlayerName ON DUPLICATE KEY UPDATE PlayerRaiting = VALUES(PlayerRaiting)";
                    MySqlCommand cmd7 = new MySqlCommand(nw, connection);
                    cmd7.ExecuteNonQuery();

                    string dos = "INSERT INTO playerlist_dos_total (PlayerName, PlayerRaiting, PlayerClass) SELECT PlayerName, ROUND(SUM(PlayerRaiting), 2), PlayerClass FROM ( SELECT PlayerName, PlayerRaiting, PlayerClass FROM playerlist_dos_forti UNION SELECT PlayerName, PlayerRaiting, PlayerClass FROM playerlist_dos_tyra ) t GROUP BY PlayerName ON DUPLICATE KEY UPDATE PlayerRaiting = VALUES(PlayerRaiting)";
                    MySqlCommand cmd8 = new MySqlCommand(dos, connection);
                    cmd8.ExecuteNonQuery();

                    string top = "INSERT INTO playerlist_top_total (PlayerName, PlayerRaiting, PlayerClass) SELECT PlayerName, ROUND(SUM(PlayerRaiting), 2), PlayerClass FROM ( SELECT PlayerName, PlayerRaiting, PlayerClass FROM playerlist_top_forti UNION SELECT PlayerName, PlayerRaiting, PlayerClass FROM playerlist_top_tyra ) t GROUP BY PlayerName ON DUPLICATE KEY UPDATE PlayerRaiting = VALUES(PlayerRaiting)";
                    MySqlCommand cmd9 = new MySqlCommand(top, connection);
                    cmd9.ExecuteNonQuery();

                    string pf = "INSERT INTO playerlist_pf_total (PlayerName, PlayerRaiting, PlayerClass) SELECT PlayerName, ROUND(SUM(PlayerRaiting), 2), PlayerClass FROM ( SELECT PlayerName, PlayerRaiting, PlayerClass FROM playerlist_pf_forti UNION SELECT PlayerName, PlayerRaiting, PlayerClass FROM playerlist_pf_tyra ) t GROUP BY PlayerName ON DUPLICATE KEY UPDATE PlayerRaiting = VALUES(PlayerRaiting)";
                    MySqlCommand cmd10 = new MySqlCommand(pf, connection);
                    cmd10.ExecuteNonQuery();

                    string sg = "INSERT INTO playerlist_sg_total (PlayerName, PlayerRaiting, PlayerClass) SELECT PlayerName, ROUND(SUM(PlayerRaiting), 2), PlayerClass FROM ( SELECT PlayerName, PlayerRaiting, PlayerClass FROM playerlist_sg_forti UNION SELECT PlayerName, PlayerRaiting, PlayerClass FROM playerlist_sg_tyra ) t GROUP BY PlayerName ON DUPLICATE KEY UPDATE PlayerRaiting = VALUES(PlayerRaiting)";
                    MySqlCommand cmd11 = new MySqlCommand(sg, connection);
                    cmd11.ExecuteNonQuery();

                    string sow = "INSERT INTO playerlist_sow_total (PlayerName, PlayerRaiting, PlayerClass) SELECT PlayerName, ROUND(SUM(PlayerRaiting), 2), PlayerClass FROM ( SELECT PlayerName, PlayerRaiting, PlayerClass FROM playerlist_sow_forti UNION SELECT PlayerName, PlayerRaiting, PlayerClass FROM playerlist_sow_tyra ) t GROUP BY PlayerName ON DUPLICATE KEY UPDATE PlayerRaiting = VALUES(PlayerRaiting)";
                    MySqlCommand cmd12 = new MySqlCommand(sow, connection);
                    cmd12.ExecuteNonQuery();




                    string data_refreshed = ("REPLACE INTO data_refreshed(refreshed) VALUES (@value1)");
                    MySqlCommand cmd14 = new MySqlCommand(data_refreshed, connection);
                    cmd14.Parameters.AddWithValue("@value1", DateTime.Now);
                    cmd14.ExecuteNonQuery();
                    connection.Close();






                    using (var conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        using (var tr = conn.BeginTransaction())
                        {
                            //First Command 
                            using (var cmd15 = new MySqlCommand("SELECT DISTINCT PlayerName FROM raiting_total", conn, tr))
                            {
                                using (var reader15 = cmd15.ExecuteReader())
                                {
                                    var playerNames = new List<string>();
                                    while (reader15.Read())
                                    {
                                        playerNames.Add(reader15["PlayerName"].ToString());
                                    }
                                    // Close the reader here before executing cmd17
                                    reader15.Close();

                                    foreach (var playerName in playerNames)
                                    {
                                        //Creating new command here
                                        string subquery = $@"SELECT SUM(COALESCE(ROUND(Raiting, 2), 0)) AS sum_rating FROM 
                                             (
                                            (SELECT Raiting FROM playerlist_dos_forti WHERE PlayerName = @playerName ORDER BY Raiting DESC LIMIT 1)
                                            UNION ALL
                                            (SELECT Raiting FROM playerlist_pf_forti WHERE PlayerName = @playerName ORDER BY Raiting DESC LIMIT 1)
                                            UNION ALL
                                            (SELECT Raiting FROM playerlist_hoa_forti WHERE PlayerName = @playerName ORDER BY Raiting DESC LIMIT 1)
                                            UNION ALL
                                            (SELECT Raiting FROM playerlist_mots_forti WHERE PlayerName = @playerName ORDER BY Raiting DESC LIMIT 1)
                                              UNION ALL 
                                             (SELECT Raiting FROM playerlist_soa_forti WHERE PlayerName = @playerName ORDER BY Raiting DESC LIMIT 1) 
                                             UNION ALL
                                             (SELECT Raiting FROM playerlist_sg_forti  WHERE PlayerName = @playerName ORDER BY Raiting DESC LIMIT 1)
                                             UNION ALL 
                                             (SELECT Raiting FROM playerlist_sow_forti WHERE PlayerName = @playerName ORDER BY Raiting DESC LIMIT 1)
                                             UNION ALL
                                             (SELECT Raiting FROM playerlist_top_forti  WHERE PlayerName = @playerName ORDER BY Raiting DESC LIMIT 1)
                                             UNION ALL 
                                             (SELECT Raiting FROM playerlist_nw_forti  WHERE PlayerName = @playerName ORDER BY Raiting DESC LIMIT 1)
                                             UNION ALL 
                                             (SELECT Raiting FROM playerlist_sd_forti  WHERE PlayerName = @playerName ORDER BY Raiting DESC LIMIT 1)
                                             UNION ALL 
                                             (SELECT Raiting FROM playerlist_sd_tyra  WHERE PlayerName = @playerName ORDER BY Raiting DESC LIMIT 1)
                                             UNION ALL 
                                             (SELECT Raiting FROM playerlist_nw_tyra  WHERE PlayerName = @playerName ORDER BY Raiting DESC LIMIT 1)
                                             UNION ALL 
                                             (SELECT Raiting FROM playerlist_top_tyra  WHERE PlayerName = @playerName ORDER BY Raiting DESC LIMIT 1)
                                             UNION ALL 
                                             (SELECT Raiting FROM playerlist_sow_tyra  WHERE PlayerName = @playerName ORDER BY Raiting DESC LIMIT 1)
                                             UNION ALL 
                                             (SELECT Raiting FROM playerlist_sg_tyra  WHERE PlayerName = @playerName ORDER BY Raiting DESC LIMIT 1)
                                             UNION ALL 
                                             (SELECT Raiting FROM playerlist_soa_tyra  WHERE PlayerName = @playerName ORDER BY Raiting DESC LIMIT 1)
                                             UNION ALL 
                                             (SELECT Raiting FROM playerlist_mots_tyra  WHERE PlayerName = @playerName ORDER BY Raiting DESC LIMIT 1)
                                             UNION ALL 
                                             (SELECT Raiting FROM playerlist_hoa_tyra  WHERE PlayerName = @playerName ORDER BY Raiting DESC LIMIT 1)
                                             UNION ALL 
                                             (SELECT Raiting FROM playerlist_pf_tyra  WHERE PlayerName = @playerName ORDER BY Raiting DESC LIMIT 1)
                                             UNION ALL 
                                             (SELECT Raiting FROM playerlist_dos_tyra  WHERE PlayerName = @playerName ORDER BY Raiting DESC LIMIT 1)
                                                                                           
                                        ) subquery";
                                        using (var cmd17 = new MySqlCommand(subquery, conn, tr))
                                        {
                                            cmd17.Parameters.AddWithValue("@playerName", playerName);
                                            var sum_rating = cmd17.ExecuteScalar();
                                            if (!string.IsNullOrEmpty(sum_rating.ToString()))
                                            {
                                                //Creating new command here
                                                using (var cmd16 = new MySqlCommand("UPDATE raiting_total SET PlayerRaiting = @sum_rating WHERE PlayerName = @playerName", conn, tr))
                                                {
                                                    cmd16.Parameters.AddWithValue("@sum_rating", sum_rating);
                                                    cmd16.Parameters.AddWithValue("@playerName", playerName);
                                                    cmd16.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            tr.Commit();
                        }
                    }









                    connection.Close();

                    Console.WriteLine("Data collected and refreshed:" + DateTime.Now);

                    Thread.Sleep(600000);
                }
            }
        
        public static string raitingcalculation(string[] data)
        {
            //Testing with +3
            string runtime = data[0];
            string[] samplesplit = runtime.Split(':');
            double time = double.Parse(samplesplit[0]) + double.Parse(samplesplit[1]) / 60 + double.Parse(samplesplit[2]) / 6000;

            string time_treshold1 = data[1];
            string[] split1 = time_treshold1.Split(':');
            double time1 = double.Parse(split1[0]) + double.Parse(split1[1]) / 60 + double.Parse(split1[2]) / 6000;

            string time_treshold2 = data[2];
            string[] split2 = time_treshold2.Split(':');
            double time2 = double.Parse(split2[0]) + double.Parse(split2[1]) / 60 + double.Parse(split2[2]) / 6000;

            string time_treshold3 = data[3];
            string[] split3 = time_treshold3.Split(':');
            double time3 = double.Parse(split3[0]) + double.Parse(split3[1]) / 60 + double.Parse(split3[2]) / 6000;

            string multiplier = "";

            if(time < time3)
            {
                multiplier = "***";
            }
            else if(time < time2 && time > time3)
            {
                multiplier = "**";
            }
            else if (time < time1 && time > time2)
            {
                multiplier = "*";
            }
            else
            {
                multiplier = "";
            }


            int lvl = Int32.Parse(data[4]);
            double[] dungTimers = { time1, time2, time3 };
            double[] keylvltoscore = { 0.01, 0.01, 40, 45, 55, 60, 65, 75, 80, 85, 100, 105, 110, 115, 120, 125, 130, 135, 140, 145, 150, 155, 160, 165, 170, 175, 180, 185, 190, 195, 200, 205, 210, 215, 220, 225 };  // taken from raider.io addon

            double maxSoftDepleteTime = dungTimers[0] * 2.5;  // assume max soft deplete time is 250% of timer.

            double softDepleteStartScore = keylvltoscore[lvl - 1] + 0.01;
            double softDeplete125 = keylvltoscore[lvl - 2] + 0.01;
            double softDeplete150 = (lvl - 3 < 0) ? 0.01 : keylvltoscore[lvl - 3] + 0.01;
            double softDeplete200 = (lvl - 4 < 0) ? 0.01 : keylvltoscore[lvl - 4] + 0.01;
            double softDeplete250 = (lvl - 5 < 0) ? 0.01 : keylvltoscore[lvl - 5] + 0.01;

            double timedScore = keylvltoscore[lvl];
            double upScore = keylvltoscore[lvl + 1];

            double score;

            if (time >= maxSoftDepleteTime)  // if went beyond
            {
                score = softDeplete250;
            }
            else if (dungTimers[0] * 2.5 > time && time >= dungTimers[0] * 2)  // if between (2 - 2.5) * dungtimer
            {
                score = (((time - (dungTimers[0] * 2.5)) / ((dungTimers[0] * 2.50) - (dungTimers[0] * 2))) * (softDeplete250 - softDeplete200)) + softDeplete250;
            }
            else if (dungTimers[0] * 2 > time && time >= dungTimers[0] * 1.50)  // if between (1.50 - 2) * dungtimer
            {
                score = (((time - (dungTimers[0] * 2)) / ((dungTimers[0] * 2) - (dungTimers[0] * 1.50))) * (softDeplete200 - softDeplete150)) + softDeplete200;
            }
            else if (dungTimers[0] * 1.50 > time && time >= dungTimers[0] * 1.25)  // if between (1.25 - 1.50) * dungtimer
            {
                score = (((time - (dungTimers[0] * 1.50)) / ((dungTimers[0] * 1.50) - (dungTimers[0] * 1.25))) * (softDeplete150 - softDeplete125)) + softDeplete150;
            }
            else if (dungTimers[0] * 1.25 > time && time > dungTimers[0])  // if between (1 - 1.25) * dungtimer
            {
                score = (((time - (dungTimers[0] * 1.25)) / ((dungTimers[0] * 1.25) - dungTimers[0])) * (softDeplete125 - softDepleteStartScore)) + softDeplete125;
            }
            else
            {
                score = ((time / dungTimers[0]) * (timedScore - upScore)) + upScore;
            }

            double raiting = Math.Round(score, 2);

            string raiting_multiplier = raiting +" "+ multiplier;

            

            return raiting_multiplier;
           
        }
    }
}
