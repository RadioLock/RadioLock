using RadioLock.Properties;
using ServiceStack.ServiceInterface;
using System;
using System.Collections;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace RadioLock
{
    public class CardInfoService : ServiceBase<CardInfo2>
    {
        private Hashtable roomList;

        public string getReservationRoomId(string roomName, string cinTime)
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

        public void logReservationRoomId(string roomName, string cinTime, string reservationRoomId)
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

        public string getLockNo(string roomName)
        {
            string password = ConfigurationSettings.AppSettings["dbPassword"];
            string fileDb = ConfigurationSettings.AppSettings["fileDb"];
            RadioLockConnector.ConnectionString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}" + fileDb + ";Jet OLEDB:Database Password=" + password + ";", Settings.Default.LockFolder);
            WriteLog("ConnectionString:" + RadioLockConnector.ConnectionString);
            string _LockNo = "";

            string queryString = "SELECT LockNo FROM RoomInfo where RoomName='" + roomName + "'";
            WriteLog("queryString:" + queryString);
            RadioLockConnector.ConnectionString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}" + fileDb + ";Jet OLEDB:Database Password=" + password + ";", Settings.Default.LockFolder);

            using (OleDbConnection connection = new OleDbConnection(RadioLockConnector.ConnectionString))
            using (OleDbCommand command = new OleDbCommand(queryString, connection))
            {
                try
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        _LockNo = reader[0].ToString();
                    }
                    reader.Close();
                    //frmMain.reloadRoomList = false;
                    connection.Close();
                }
                catch (Exception ex)
                {
                    _LockNo = "";
                }
            }

            if (_LockNo.Length == 0)
            {
                if (roomName.Length == 3) roomName = "0" + roomName;
                if (roomName.Length == 2) roomName = "00" + roomName;
                queryString = "SELECT LockNo FROM RoomInfo where RoomName='" + roomName + "'";
                WriteLog("queryString:" + queryString);
                ClockConnector.ConnectionString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}" + fileDb + ";Jet OLEDB:Database Password=" + password + ";", Settings.Default.LockFolder);

                using (OleDbConnection connection = new OleDbConnection(ClockConnector.ConnectionString))
                using (OleDbCommand command = new OleDbCommand(queryString, connection))
                {
                    try
                    {
                        connection.Open();
                        OleDbDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            _LockNo = reader[0].ToString();
                        }
                        reader.Close();
                        //frmMain.reloadRoomList = false;
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        _LockNo = "";
                    }
                }
            }

            return _LockNo;
        }

        protected override object Run(CardInfo2 request)
        {
            string password = ConfigurationSettings.AppSettings["dbPassword"];
            string fileDb = ConfigurationSettings.AppSettings["fileDb"];
            RadioLockConnector.ConnectionString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}" + fileDb + ";Jet OLEDB:Database Password=" + password + ";", Settings.Default.LockFolder);
            WriteLog("ConnectionString:" + RadioLockConnector.ConnectionString);
            var response = new CardInfoResponse2 { Result = 1 }; //0: OK
            if (!frmMain.isValid) return response;
            RadioLockConnector obj = new RadioLockConnector();

            switch (this.Request.PathInfo)
            {
                case "/readcard": //read card info
                    var Info = obj.ReadCard();
                    response.Result = Info.result;
                    WriteLog("start Read card: Resuld-" + Info.result + ", CardNo:" + Info.cardNo + ",RoomName-" + Info.room + " Arrival date-" + Info.arrivalDate + ", Departure Date-" + Info.departureDate);
                    if (Info.result == 0)
                    {
                        string roomName = Info.room.Insert(0, "00").Insert(4, "00").Insert(8, "00");
                        response.TravellerId = getReservationRoomId(roomName, Info.arrivalDate.ToString("yyyyMMddHHmm"));
                        response.ArrivalDate = Info.arrivalDate;
                        response.DepartureDate = Info.departureDate;
                        response.RoomName = Info.room;

                        WriteLog("Thông tin response: " + "Result:" + response.Result + "TravellerId:" + response.TravellerId + "TravelllerName:" + response.TravellerName + "ArivalDate:" + response.ArrivalDate + "DepartureDate:" + response.DepartureDate +
                            "RoomName:" + response.RoomName + "cardNumber:" + response.cardNumber);
                    }
                    break;

                case "/writecard":
                    WriteLog("start writecard:RoomName-" + request.RoomName + ", Arr-" + request.ArrivalDate + ", Dep -" + request.DepartureDate + "TraellerId -" +
                    request.TravellerId + "TravellerName:" + request.TravellerName);

                    string roomAddress = getLockNo(request.RoomName);
                    WriteLog("roomAddress:" + roomAddress);

                    int travellerId = 0;
                    Int32.TryParse(request.TravellerId, out travellerId);
                    //string LockNo = "";
                    //LockNo=roomList[request.RoomName].ToString();
                    string HotelCode = Settings.Default.HotelCode;

                    response.Result = obj.WriteCard(roomAddress, request.ArrivalDate.ToString("yyyy-MM-dd HH:mm:ss"), request.DepartureDate.ToString("yyyy-MM-dd HH:mm:ss"), 0);
                    WriteLog("Kết quả: " + response.Result);
                    if (this.Request.PathInfo == "/writecard")
                    {
                        logReservationRoomId(roomAddress, request.ArrivalDate.ToString("yyyyMMddHHmm"), request.TravellerId);
                    }

                    break;

                case "/deletecard":

                    response.Result = obj.DeleteCard();
                    WriteLog("Kết quả: " + response.Result);
                    break;

                default:
                    break;
            }
            return response;
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