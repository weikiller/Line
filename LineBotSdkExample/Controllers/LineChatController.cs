using isRock.LineBot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;


namespace LineBotSdkExample.Controllers
{
    public class LineChatController : ApiController
    {
        [HttpPost]
        public IHttpActionResult POST()
        {
            //設定你的Channel Access Token
            string ChannelAccessToken = "FRIVkZ1ddfC90UObByPBHb/RfaotMELEVzQ4YPlKJOzSPrDCJ2uzeewA1mVaSX7+e3Qip3deQ5xjpHU9ut8v+HRpt4xwmxefP8O8GlUb8ynT7v6gmErTDz5+Xl1kyI1+YGpu3DdE24G1oQ/om/gi3QdB04t89/1O/w1cDnyilFU=";
            isRock.LineBot.Bot bot;
            //如果有Web.Config app setting，以此優先
            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("LineChannelAccessToken"))
            {
                ChannelAccessToken = System.Configuration.ConfigurationManager.AppSettings["LineChannelAccessToken"];
            }            
          
            //create bot instance
            bot = new isRock.LineBot.Bot(ChannelAccessToken);

            try
            {
                //取得 http Post RawData(should be JSON)
                string postData = Request.Content.ReadAsStringAsync().Result;
                //剖析JSON
                var ReceivedMessage = isRock.LineBot.Utility.Parsing(postData);
                var UserSays = ReceivedMessage.events[0].message.text;
                var ReplyToken = ReceivedMessage.events[0].replyToken;

                string Message = "";
                 MysqlLineID mysqlLineID = new MysqlLineID();
                var item = ReceivedMessage.events.FirstOrDefault();
                //依照用戶說的特定關鍵字來回應
                switch (UserSays.ToLower())
                {
                    case "/teststicker":
                        //回覆貼圖
                        bot.ReplyMessage(ReplyToken, 1, 1);
                        break;
                    case "/testimage":
                        //回覆圖片
                        bot.ReplyMessage(ReplyToken, new Uri("https://scontent-tpe1-1.xx.fbcdn.net/v/t31.0-8/15800635_1324407647598805_917901174271992826_o.jpg?oh=2fe14b080454b33be59cdfea8245406d&oe=591D5C94"));
                        break;
                    case "b":
                        
                        if(mysqlLineID.DBConnectionInsert(@"INSERT INTO `linemessage`(`Msg`, `ID`) VALUES ('" + UserSays + "','" + ReceivedMessage.events[0].source.userId + "');")=="true")
                        {
                            Message += "預約成功,您預約資料為：" + UserSays;
                        }
                        else
                        {
                            Message+= "輸入錯誤!!,請重新輸入!!";
                        }
                        bot.ReplyMessage(ReplyToken, Message + "!");
                        break;
                    default:
                        //回覆訊息
                         Message += "哈囉, 歡迎您加入,預約請輸入 b :";//+ UserSays; //+"ID："+ReceivedMessage.events[0].source.userId;
                                                          //測試方法: 紀錄
                        if (!mysqlLineID.DBConnectionjudge("lineid", ReceivedMessage.events[0].source.userId))
                        {
                            Message += mysqlLineID.DBConnectionInsert(@"INSERT INTO `lineid`(`Name`, `ID`) VALUES ('Test','" + ReceivedMessage.events[0].source.userId + "');");
                            //Message += mysqlLineID.DBConnectionInsert(@"INSERT INTO `linemessage`(`Msg`, `ID`) VALUES ('" + UserSays + "','" + ReceivedMessage.events[0].source.userId + "');");
                        }
                  
                        
                        //回覆用戶
                        bot.ReplyMessage(ReplyToken, Message+ "!");
                       
                        break;
                }
                //回覆API OK
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok();
            }
        }

        private string readFile()
        {
            String line;
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader("LineID.txt");

                //Read the first line of text
                line = sr.ReadLine();

                //Continue to read until you reach end of file
                while (line != null)
                {
                    //write the lie to console window
                    Console.WriteLine(line);
                    //Read the next line
                    line = sr.ReadLine()+",";
                }

                //close the file
                sr.Close();
                // Console.ReadLine();
                return line;
            }
            catch (Exception e)
            {
                //  Console.WriteLine("Exception: " + e.Message);
                return "Exception: " + e.Message;
            }           
        }

        private string InsertID(string userId)
        {
            try
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter("LineID.txt");

                //Write a line of text
                sw.WriteLine(userId);            

                //Close the file
                sw.Close();
                return "OK";
            }
            catch (Exception e)
            {
                return "Exception: " + e.Message;
            }            
        }
    }
}
