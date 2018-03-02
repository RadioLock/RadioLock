using RadioLock.Properties;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace RadioLock
{
    public partial class frmMain : Form
    {
        public static bool isValid = false;
        private bool newlySelected = false;
        public static bool reloadRoomList = false;
        public static int COMPort;
        internal static Dev_C_Sharp.Dev_C_Sharp devCommand = Dev_C_Sharp.Dev_C_Sharp.Instance;
        private string listeningOn = "http://*:2000/";

        public frmMain()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //CardInfoService.WriteLog(DateTime.Now.ToString() + " : ");
            CardInfoService.WriteLog(DateTime.Now.ToString() + " : Form Load");
            var appHost = new AppHost();
            appHost.Init();
            appHost.Start(listeningOn);
            ConnectDB();
            OpenPort();
        }

        private void ConnectDB()
        {
            using (SqlConnection connection = new SqlConnection(RadioLockConnector.ConnectionString))
            {
                var queryString = string.Format("SELECT TOP 1 Build_Code FROM D_Build");
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        // just check if we can connect to DB
                    }
                    reader.Close();
                    CardInfoService.WriteLog(DateTime.Now.ToString() + " : ConnectionString : " + RadioLockConnector.ConnectionString);
                }
                catch (Exception ex)
                {
                    //Not correct folder
                    CardInfoService.WriteLog("Error:" + ex.Message);
                    lblStatus.Text = "Không thể kết nối tới Database";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }
        }

        public void OpenPort()
        {
            int m_portnum = int.Parse(ConfigurationManager.AppSettings["port"]);
            int baud = int.Parse(ConfigurationManager.AppSettings["baud"]);
            var st = devCommand.OpenPort(m_portnum, baud, true);
            if (st < 4 && st!= -800)
            {
                //devCommand.ClosePort(m_portnum);
                CardInfoService.WriteLog(DateTime.Now.ToString() + " : Can't open port");
                lblStatus.Text = "Không thể mở port";
                lblStatus.ForeColor = Color.Red;
            }
            else
            {
                CardInfoService.WriteLog(DateTime.Now.ToString() + " : Port opened");
            }
        }
    }

    public class AppHost : AppHostHttpListenerBase
    {
        public AppHost()
        : base("HttpListener Self-Host Orbita", typeof(CardInfoService).Assembly)
        { }

        public override void Configure(Funq.Container container)
        {
            base.SetConfig(new EndpointHostConfig
            {
                GlobalResponseHeaders = {
                    { "Access-Control-Allow-Origin", "*" },
                    { "Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS" },
                    { "Access-Control-Allow-Headers", "Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With" },
                },
            });

            Routes
            .Add<CardInfoRequest1>("/readcard")
            .Add<CardInfoRequest1>("/deletecard")
            .Add<CardInfoRequest1>("/writecard");
        }
    }
}