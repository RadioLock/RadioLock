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

        private string listeningOn = "http://*:2000/";

        public frmMain()
        {
            InitializeComponent();
            this.lblStatus.Text = "Đã kết nối, Bạn có thể tiến hành tạo khóa khách hàng!";
            lblStatus.ForeColor = Color.Blue;
            button2.Enabled = false;
            this.CenterToScreen();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            CardInfoService.WriteLog("Start" + DateTime.Now.ToString());
            var appHost = new AppHost();
            appHost.Init();
            appHost.Start(listeningOn);
            SelectInstallationFolder();
        }

        private void SelectInstallationFolder()
        {
           
        }
        private void button2_Click(object sender, EventArgs e)
        {
            var folder = folderBrowserDialog1.ShowDialog();
            if (folder == DialogResult.OK)
            {
                newlySelected = true;
                Settings.Default.SystemCode = "";
                Settings.Default.LockFolder = folderBrowserDialog1.SelectedPath;
                Settings.Default.Save();
                SelectInstallationFolder();
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