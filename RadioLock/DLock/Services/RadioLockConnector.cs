using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;

namespace RadioLock
{
    internal class RadioLockConnector
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DB"].ToString();

        internal static Dev_C_Sharp.Dev_C_Sharp devCommand = Dev_C_Sharp.Dev_C_Sharp.Instance;
        internal static int WriteGuestCard()
        {
            return devCommand.WriteCard(0, 0, "", "", 0, false);
        }

        public RoomInfo GetAllCodebyRoomName(string roomName)
        {
            RoomInfo info = new RoomInfo();
            using (SqlConnection connection = new SqlConnection(RadioLockConnector.ConnectionString))
            {
                var queryString = string.Format(@"select D_Build.Build_Code,D_Floor.Floor_Code,D_Rooms.R_Code,R_SubCode,R_SubCodeDai
                                                from D_Rooms 
                                                inner join D_Floor on D_Rooms.R_FloorID =D_Floor.Floor_ID
                                                inner join D_Build on D_Floor.Build_ID = D_Build.Build_ID
												where D_Rooms.R_Name='{0}'", roomName);
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        info.b_code = reader[0].ToString();
                        info.f_code = reader[1].ToString();
                        info.r_code = reader[2].ToString();
                        info.r_subcode = reader[3].ToString();
                        info.r_subcodedai = reader[4].ToString();
                    }
                    reader.Close();
                    CardInfoService.WriteLog(DateTime.Now.ToString() + " : ConnectionString : " + RadioLockConnector.ConnectionString);
                }
                catch (Exception ex)
                {
                    CardInfoService.WriteLog(DateTime.Now.ToString() + " GetAllCodebyRoomName Error:" + ex.Message);
                }
            }
            return info;
        }
        public string GetRoomName(string b_code, string f_code, string r_code, string r_subcode, string r_subcodedai)
        {
            string roomName = "";
            using (SqlConnection connection = new SqlConnection(RadioLockConnector.ConnectionString))
            {
                var queryString = string.Format(@"select D_Rooms.R_Name 
                                                from D_Rooms 
                                                inner join D_Floor on D_Rooms.R_FloorID =D_Floor.Floor_ID
                                                inner join D_Build on D_Floor.Build_ID = D_Build.Build_ID
                                                where D_Rooms.R_Code={0}
                                                and D_Floor.Floor_Code={1}
                                                and D_Build.Build_Code={2} 
                                                and D_Rooms.R_SubCode={3}
                                                and D_Rooms.R_SubCodeDai={4}",r_code,f_code,b_code,r_subcode,r_subcodedai);
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        roomName = reader[0].ToString();
                    }
                    reader.Close();
                    CardInfoService.WriteLog(DateTime.Now.ToString() + " : ConnectionString : " + RadioLockConnector.ConnectionString);
                }
                catch (Exception ex)
                {
                    CardInfoService.WriteLog(DateTime.Now.ToString() + " GetRoomName Error:" + ex.Message);
                }
            }
            return roomName;
        }

        public string GetCardNumberBeforeWrite()
        {
            int st = 0;
            string carddata = "", cardNumber="0";
            byte cardType = 255;
            st = devCommand.ReadCard(out cardType, ref carddata, true);
            if (st < 0)
            {
                CardInfoService.WriteLog(DateTime.Now.ToString() + " : GetCardNumberBeforeWrite : " + st.ToString());
            }
            else
            {
                if(!string.IsNullOrEmpty(carddata))
                {
                     string[] data = carddata.Split(';');
                     cardNumber = data[0];
                }
                CardInfoService.WriteLog(DateTime.Now.ToString() + " : GetCardNumberBeforeWrite : " + carddata + " CardType : " + cardType.ToString());                
            }
            return cardNumber;
        }


        /// <summary>
        /// read card  Opening USB or COM depends on you.
        /// </summary>
        /// <param name="retdata">card written data return，string type，divided by”;”，including：card type、card number、valid time、card data。</param>        
        /// <param name="Buzzer">true：device buzzer。false：no buzzer</param>
        /// <returns> return：return value = 0 success，other value means fail，please refer to the error code of other return value。</returns>
        //public int ReadCard(ref string CardData, bool Buzzer);

        public CardInfoResponse1 ReadCard()
        {
            var cardInfoResponse = new CardInfoResponse1();
            int st = 0;
            string carddata = "";
            byte cardType = 255;
            st = devCommand.ReadCard(out cardType, ref carddata, true);
            if (st < 0)
            {
                cardInfoResponse.isSuccess = false;
                CardInfoService.WriteLog(DateTime.Now.ToString() + " : Read Card Error Code : " + st.ToString());
                cardInfoResponse.message = st.ToString();
            }
            else
            {
                cardInfoResponse.isSuccess = true;
                CardInfoService.WriteLog(DateTime.Now.ToString() + " : Read Card Success -- DataOnCard : " + carddata + " CardType : " + cardType.ToString());
                if (!string.IsNullOrEmpty(carddata))
                {
                    //format 596;2018-03-02 12:30;1,3,2,0,61 -> CardNumber;Valid Date = EndDate; build code, floor code, room code, room subcode, room subcode dai
                    try
                    {
                        string[] data = carddata.Split(';');
                        cardInfoResponse.cardNumber = data[0];
                        if(data.Length>1)
                        {
                            cardInfoResponse.validTime = data[1];
                            string roomData = data[2];
                            string[] roomSplitData = roomData.Split(',');
                            RoomInfo roomInfo = new RoomInfo();
                            roomInfo.b_code = roomSplitData[0];
                            roomInfo.f_code = roomSplitData[1];
                            roomInfo.r_code = roomSplitData[2];
                            roomInfo.r_subcode = roomSplitData[3];
                            roomInfo.r_subcodedai = roomSplitData[4];

                            string roomName = GetRoomName(roomInfo.b_code, roomInfo.f_code, roomInfo.r_code, roomInfo.r_subcode, roomInfo.r_subcodedai);
                            cardInfoResponse.roomName = roomName;
                            var validDate = DateTime.ParseExact(data[1], "yyyy-MM-dd HH:mm",System.Globalization.CultureInfo.InvariantCulture);
                            cardInfoResponse.reservationRoomId = CardInfoService.getReservationRoomId(roomName, validDate.ToString("yyyyMMddHHmm"));
                        }
                    }
                    catch(Exception ex)
                    {
                        CardInfoService.WriteLog(DateTime.Now.ToString() + " : Read Card Success -- DataOnCard -- SplitData Error : " + carddata + " CardType : " + cardType.ToString() + " Exception : " + ex.ToString());
                    }
                }
            }
            return cardInfoResponse;
        }

        /// <summary>
        /// write card
        /// </summary>
        /// <param name="cardtype">card type（0 - 255）</param>
        /// <param name="cardnum">card number（0 - 16777215）</param>
        /// <param name="datetime">valid date（format: year, month, date, hour, minute，Ex：201106012359）</param>
        /// <param name="carddata">card data（please refre to the card writing format）</param>
        /// <param name="datalen">card data length</param>
        /// <param name="Buzzer">true：device buzzering。false：no buzzering</param>
        /// <returns>return：return value = 0 success，other value means fail，please refer to the error code of other return value。</returns>
        //public int WriteCard(int cardtype, int cardnum, string datetime, string carddata, int datalen, bool Buzzer);


        public bool WriteCard(string roomName, DateTime endDate)
        {
            //string data = "0F12500100";//0F125001: building = 15(0F)、floor = 18(12)、room = 80(50)、subroom = 1(01)，baseband value = 0，change to HEX string
            int result = 0;
            int cardnum = int.Parse(GetCardNumberBeforeWrite());
            string validDate = endDate.ToString("yyyyMMddHHmm");
            RoomInfo roomInfo = GetAllCodebyRoomName(roomName);
            string data = int.Parse(roomInfo.b_code).ToString("X2") 
                + int.Parse(roomInfo.f_code).ToString("X2")
                + int.Parse(roomInfo.r_code).ToString("X2")
                + int.Parse(roomInfo.r_subcode).ToString("X2")
                + int.Parse(roomInfo.r_subcodedai).ToString("X2");                        
            result = devCommand.WriteCard(CardType.card_Guest, cardnum, validDate, data, data.Length, true);
            if (result == 0)
            {
                CardInfoService.WriteLog(DateTime.Now.ToString() + " : WriteCard Success Code : " + result.ToString());
                return true;
            }
            else
            {
                CardInfoService.WriteLog(DateTime.Now.ToString() + " : WriteCard Error Code : " + result.ToString());
                return false;
            }
        }

        public bool DeleteCard()
        {
            int result = 0;
            result = devCommand.ClearCard(2, true); // clear card data
            CardInfoService.WriteLog(DateTime.Now.ToString() + " Delete card status:" + result.ToString());
            if(result<0)
            {
                return false;
            }           
            return true;
        }
    }

    internal class CardType
    {
        /// <summary>
        /// Anthorizing card
        /// </summary>
        internal const int card_Authorization = 0x00;
        /// <summary>
        /// close card
        /// </summary>
        internal const int card_Lock = 0x00;
        /// <summary>
        /// record card
        /// </summary>
        internal const int card_Record = 0x01;
        /// <summary>
        /// room number card
        /// </summary>
        internal const int card_Room_number = 0x02;
        /// <summary>
        /// Time sync card
        /// </summary>
        internal const int card_Time = 0x03;
        /// <summary>
        /// lost card    lost card for certains cards
        /// </summary>
        internal const int card_Loss_report = 0x04;
        /// <summary>
        /// group number card
        /// </summary>
        internal const int card_Group_number_setting = 0x05;
        /// <summary>
        /// guest card
        /// </summary>
        internal const int card_Guest = 0x06;
        /// <summary>
        /// check out card
        /// </summary>
        internal const int card_Check_out = 0x07;
        /// <summary>
        /// group number card
        /// </summary>
        internal const int card_Group_number = 0x09;
        /// <summary>
        /// emergency card
        /// </summary>
        internal const int card_Emergency = 0x0a;
        /// <summary>
        /// control card
        /// </summary>
        internal const int card_Master_control = 0x0b;
        /// <summary>
        /// floor card
        /// </summary>
        internal const int card_Floor = 0x0c;
        /// <summary>
        /// building card
        /// </summary>
        internal const int card_Building = 0x0d;
    }
}