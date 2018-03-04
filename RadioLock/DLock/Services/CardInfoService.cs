using RadioLock.Properties;
using ServiceStack.ServiceInterface;
using System;
using System.Collections;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Web.Script.Serialization;

namespace RadioLock
{
    public class CardInfoService : ServiceBase<CardInfoRequest1>
    {

        public static string getReservationRoomId(string roomName, string cinTime)
        {
            string reservaionRoomId = "0";
            string[] ReservationRoomLines = File.ReadAllLines("ReservationRoomId.txt");
            string sKey = roomName.Trim() + "_" + cinTime.Trim();
            File.AppendAllText("logs.txt", "\r\n sKey:=" + sKey, Encoding.ASCII);

            foreach (string line in ReservationRoomLines)
            {
                // Use a tab to indent each line of the file.
                string[] words = line.Split('=');
                File.AppendAllText("logs.txt", "\r\n line:=" + line + ",w0: " + words.Length.ToString(), Encoding.ASCII);
                if (words.Length > 1)
                {
                    File.AppendAllText("logs.txt", "\r\n line:=" + line + ",w0: " + words[0] + ", length:" + words[0].Length + ",w1:" + words[1], Encoding.ASCII);
                    if (sKey == words[0].Trim())
                    {
                        reservaionRoomId = words[1].Trim();
                        File.AppendAllText("logs.txt", "\r\n reservaionRoomId:=" + reservaionRoomId, Encoding.ASCII);
                    }
                }
            }
            return reservaionRoomId;
        }

        public static void logReservationRoomId(string roomName, string cinTime, string reservationRoomId)
        {
            // Tao file ghi nhan thong tin ReservationRoomId
            string logFilePath = "ReservationRoomId.txt";

            FileStream fileStream = null;
            FileInfo logFileInfo;
            logFileInfo = new FileInfo(logFilePath);
            string ReservationRoomValue = "";
            string sKey = roomName.Trim() + "_" + cinTime.Trim();
            bool flag = true;

            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
                fileStream.Close();
                if (flag) ReservationRoomValue += sKey.Trim() + "=" + reservationRoomId.ToString();
            }
            else
            {
                string[] ReservationRoomLines = File.ReadAllLines(logFilePath);
                foreach (string line in ReservationRoomLines)
                {
                    // Use a tab to indent each line of the file.
                    string[] words = line.Split('=');
                    if (words.Length > 0)
                    {
                        string value = "";
                        if (sKey.Trim() == words[0])
                        {
                            value = words[0] + "=" + reservationRoomId.ToString();
                            flag = false;
                            ReservationRoomValue += "\r\n" + value;
                        }
                        else
                        {
                            value = line;
                            ReservationRoomValue += "\r\n" + value;
                        }
                    }
                }

                if (flag) ReservationRoomValue += "\r\n" + sKey.Trim() + "=" + reservationRoomId.ToString();
            }

            File.WriteAllText(logFilePath, ReservationRoomValue);
        }

        protected override object Run(CardInfoRequest1 request)
        {
            var response = new CardInfoResponse1(); //0: OK
            RadioLockConnector obj = new RadioLockConnector();
            switch (this.Request.PathInfo)
            {
                case "/readcard": //read card info
                    WriteLog(DateTime.Now.ToString() + " /readcard Start");
                    response = obj.ReadCard();
                    WriteLog(DateTime.Now.ToString() + " /readcard End");
                    return new JavaScriptSerializer().Serialize(response);

                case "/writecard":
                    WriteLog(DateTime.Now.ToString() + " /writecard Start");
                    var result = obj.WriteCard(request.roomName, request.endDate);
                    if (result) //success
                    {
                        logReservationRoomId(request.roomName, request.endDate.ToString("yyyyMMddHHmm"),request.reservationRoomId.ToString());
                    }
                    WriteLog(DateTime.Now.ToString() + " /writecard End");
                    return new JavaScriptSerializer().Serialize(new { isSuccess = result });

                case "/deletecard":
                    WriteLog(DateTime.Now.ToString() + " /deletecard Start");
                    var d_result = obj.DeleteCard();
                    WriteLog(DateTime.Now.ToString() + " /deletecard End");
                    return new JavaScriptSerializer().Serialize(new { isSuccess = d_result });
                   
                default:
                    break;
            }
            return new JavaScriptSerializer().Serialize(response);
        }

        public static void WriteLog(string strLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            string logFilePath = "";// "C:\\Logs\\";
            logFilePath = logFilePath + "Log-" + DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);
            strLog += "\r\n";
            log.WriteLine(strLog);
            log.Close();
        }

        public static void CheckErr(int iret)
        {
            switch (iret)
            {
                case 1:
                    MessageBox.Show("IDS_STRING_SUCCESS:" + iret.ToString());
                    break;

                case -1:
                    MessageBox.Show("IDS_STRING_ERROR_NOCARD:" + iret.ToString());
                    break;

                case -2:
                    MessageBox.Show("IDS_STRING_ERROR_NOREADE:" + iret.ToString());
                    break;

                case -3:
                    MessageBox.Show("IDS_STRING_ERROR_INVALIDCARD:" + iret.ToString());
                    break;

                case -4:
                    MessageBox.Show("IDS_STRING_ERROR_CARDTYPE:" + iret.ToString());
                    break;

                case -5:
                    MessageBox.Show("IDS_STRING_ERROR_READCARD:" + iret.ToString());
                    break;

                case -8:
                    MessageBox.Show("IDS_STRING_ERROR_INPUT:" + iret.ToString());
                    break;

                case -29:
                    MessageBox.Show("IDS_STRING_ERROR_REG:" + iret.ToString());
                    break;

                default:
                    MessageBox.Show("IDS_STRING_ERROR:" + iret.ToString());
                    break;
            }
        }
    }
}