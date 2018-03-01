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
            this.CenterToScreen();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            var appHost = new AppHost();
            appHost.Init();
            appHost.Start(listeningOn);
            SelectInstallationFolder();
            //string[] ports = SerialPort.GetPortNames();

            //foreach (string port in ports)
            //{
            //    comboBox1.Items.Add(port);
            //}

            //fromDate.Value = DateTime.Now;
            //toDate.Value = DateTime.Now.AddDays(1);
        }

        private void SelectInstallationFolder()
        {
            txtLocation.Text = Settings.Default.LockFolder;

            if (!txtLocation.Text.EndsWith("\\"))
                txtLocation.Text += "\\";
            string queryString = "";
            string password = ConfigurationSettings.AppSettings["dbPassword"];
            string fileDb = ConfigurationSettings.AppSettings["fileDb"];
            string datasource = ConfigurationSettings.AppSettings["dataSource"];
            string dbname = ConfigurationSettings.AppSettings["dbName"];

            queryString = "SELECT TOP 1 BuildingCode FROM RoomInfo";
            RadioLockConnector.ConnectionString = String.Format("Data Source=" + datasource + ";Initial Catalog=" + dbname + ";Integrated Security=False;User Id=sa;Password=" + password + ";MultipleActiveResultSets=True", txtLocation.Text);
            using (SqlConnection connection = new SqlConnection(RadioLockConnector.ConnectionString))
            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        txtSystemCode.Text = reader[0].ToString();
                        if (Settings.Default.SystemCode != reader[0].ToString())
                        {
                            Settings.Default.SystemCode = "5";// reader[0].ToString();
                            Settings.Default.Save();
                        }
                    }
                    reader.Close();
                    reloadRoomList = true;

                    Settings.Default.LockFolder = txtLocation.Text;
                }
                catch (Exception ex)
                {
                    //Not correct folder
                    CardInfoService.WriteLog("Error:" + ex.Message);
                    MessageBox.Show("Thư mục khóa chưa đúng. Mời bạn chọn");
                    button2.ForeColor = Color.Red;
                    return;
                }
            }

            if (Settings.Default.SystemCode.Length > 0)
            {
                RadioLockConnector obj = new RadioLockConnector();
                Int16 locktype = 0;

                locktype = Convert.ToInt16(Settings.Default.SystemCode);

                int st = 0;
                st = obj.connecToLock(locktype);
                CardInfoService.WriteLog("Read Database:LockType:" + locktype.ToString() + ", Result:" + st.ToString());
                //st = 1;
                if (st == 1)
                {
                    isValid = true;
                    this.lblStatus.Text = "Đã kết nối, Bạn có thể tiến hành tạo khóa khách hàng!";
                    lblStatus.ForeColor = Color.Blue;
                    button2.Enabled = false;
                }
                else
                {
                    isValid = false;
                    CardInfoService.CheckErr(st);
                }
            }
        }

        //private void btnCreate_Click(object sender, EventArgs e)
        //{
            /*int comPort = Int32.Parse(comboBox1.SelectedItem.ToString().Substring(3));
            //var card = CommunicationClass.ReadCard(comPort);
            int card = Int32.Parse(txtCardNo.Text);
            //string Address = CommunicationClass.CreateRoomAddress(txtBuilding.Text, txtFloor.Text, txtRoom.Text);
            CommunicationClass.CreateCard(comPort, card, CommunicationClass.CharFromString(txtSystemCode.Text), CommunicationClass.CharFromString(txtHotelCode.Text), CommunicationClass.CharFromString(Address), fromDate.Value, toDate.Value);
            MessageBox.Show("Card Created for Room " + txtRoom.Text);
            */
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
            //txtSystemCode.Text = card.SystemCode;
        //}

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

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    GLockConnector objs = new GLockConnector();
        //    int result = objs.orbita_Connect();
        //    MessageBox.Show("Ket qua ket noi:" + result.ToString());
        //    //int result = CommunicationClass.dv_connect(1000);
        //    if (result == 0)
        //    {
        //        button3.Visible = false;
        //        isValid = true;
        //    }
        //    else
        //        MessageBox.Show("Không kết nối được với đầu đọc thẻ");
        //}

        //private void button4_Click(object sender, EventArgs e)
        //{
        //    string roomAddress = "101000101";
        //    int travellerId = 10;
        //    string queryString = "Update RoomSetting set Resvers4='" + travellerId.ToString() + "' where RoomNumber='" + roomAddress + "'";
        //    using (OleDbConnection connection = new OleDbConnection(ClockConnector.ConnectionString))
        //    using (OleDbCommand command = new OleDbCommand(queryString, connection))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            command.ExecuteNonQuery();
        //            connection.Close();
        //            // WriteLog("update Resvers4=" + travellerId.ToString() + " where RoomNumber:" + roomAddress);
        //        }
        //        catch (Exception ex)
        //        {
        //            //Not correct folder
        //            //return response;
        //            //WriteLog("Có lỗi khi update db cho Resvers4=" + travellerId.ToString() + " where RoomNumber:" + roomAddress + ", error:" + ex.Message);
        //        }
        //    }
        //}
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
            .Add<CardInfo2>("/readcard")
            .Add<CardInfo2>("/deletecard")
            .Add<CardInfo2>("/writecard");
        }
    }

    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}