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
    public class CardInfoService : ServiceBase<CardInfoRequest1>
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
            string datasource = ConfigurationSettings.AppSettings["dataSource"];
            string dbname = ConfigurationSettings.AppSettings["dbName"];
            RadioLockConnector.ConnectionString = String.Format("Data Source=" + datasource + ";Initial Catalog=" + dbname + ";Integrated Security=False;User Id=sa;Password=" + password + ";MultipleActiveResultSets=True", Settings.Default.LockFolder);
            WriteLog("ConnectionString:" + RadioLockConnector.ConnectionString);
            string _LockNo = "";

            string queryString = "SELECT LockNo FROM RoomInfo where RoomName='" + roomName + "'"; //tớ không thấy bảng nào có lockno
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
            }

            return _LockNo;
        }

        protected override object Run(CardInfoRequest1 request)
        {
            //string password = ConfigurationSettings.AppSettings["dbPassword"];
            //string fileDb = ConfigurationSettings.AppSettings["fileDb"];
            //string datasource = ConfigurationSettings.AppSettings["dataSource"];
            //string dbname = ConfigurationSettings.AppSettings["dbName"];
            //RadioLockConnector.ConnectionString = String.Format("Data Source=" + datasource + ";Initial Catalog="  + dbname + ";Integrated Security=False;User Id=sa;Password=" + password + ";MultipleActiveResultSets=True", Settings.Default.LockFolder);
            WriteLog("ConnectionString:" + RadioLockConnector.ConnectionString);
            var response = new CardInfoResponse1 { result = 0 }; //0: OK
            //if (!frmMain.isValid) return response;
            RadioLockConnector obj = new RadioLockConnector();

            switch (this.Request.PathInfo)
            {
                case "/readcard": //read card info
                    var result = obj.ReadCard();
                    response.response = result;
                    WriteLog("start Read card: Result-" + result);                  
                    break;

                case "/writecard":
                    //get build code, room code, room sub code, floor code from roomId
                    response.result = obj.WriteCard(request.startDate,"1","1","1","1" );
                    WriteLog("Kết quả: " + response.result);
                    if (this.Request.PathInfo == "/writecard")
                    {
                        //logReservationRoomId();
                    }

                    break;

                case "/deletecard":

                    response.result = obj.DeleteCard();
                    WriteLog("Kết quả: " + response.result);
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