using System;
using System.Configuration;
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

        /// <summary>
        /// read card  Opening USB or COM depends on you.
        /// </summary>
        /// <param name="retdata">card written data return，string type，divided by”;”，including：card type、card number、valid time、card data。</param>        
        /// <param name="Buzzer">true：device buzzer。false：no buzzer</param>
        /// <returns> return：return value = 0 success，other value means fail，please refer to the error code of other return value。</returns>
        //public int ReadCard(ref string CardData, bool Buzzer);

        public CardInfoResponse1 ReadCard()
        {
            byte cardtype = CardType.card_Room_number;
            string responsedata = "init";
            var cardInfoResponse = new CardInfoResponse1();
            int result = devCommand.ReadCard(out cardtype, ref responsedata, true);
            cardInfoResponse.result = result;
            cardInfoResponse.response = responsedata;
            cardInfoResponse.cardNumber = "0";
            if (result == 0)
            {
                try
                {
                    string[] cardData = responsedata.Split(';');//including：card type、card number、valid time、card data
                    cardInfoResponse.cardNumber = cardData[1];
                    cardInfoResponse.validTime = cardData[2];
                    cardInfoResponse.cardData = cardData[3];
                }
                catch (Exception ex)
                {
                    CardInfoService.WriteLog("Read Card -> split data exception:" + ex.Message);
                }
            }
            CardInfoService.WriteLog("Read Card:" + result.ToString() + ", responsedata:" + responsedata);
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


        public int WriteCard(DateTime validDate, int buildingId, int floorId, int roomId, int subId)
        {
            CardInfoResponse1 card_info = ReadCard();
            //string data = "0F12500100";//0F125001: building = 15(0F)、floor = 18(12)、room = 80(50)、subroom = 1(01)，baseband value = 0，change to HEX string
            int result = 0;
            int cardnum = 0;
            cardnum = int.Parse(card_info.cardNumber);
            byte buildcode, floorcode, roomcode, subcode;
            string datetime = validDate.ToString("yyyyMMddHHmmss");
            buildcode = Convert.ToByte(buildingId);
            floorcode = Convert.ToByte(floorId);
            roomcode = Convert.ToByte(roomId);
            subcode = Convert.ToByte(subId);//range from 0-255  0-255  0-255  0-15
            string data = buildcode.ToString("X2") + floorcode.ToString("X2") + roomcode.ToString("X2") + subcode.ToString("X2");
            result = devCommand.WriteCard(CardType.card_Room_number, cardnum, datetime, data, data.Length, true);
            //CardInfoService.CheckErr(result);
            CardInfoService.WriteLog("WriteCard Response:" + result.ToString());
            return result;
        }

        public int DeleteCard()
        {
            int result = 0;
            result = devCommand.ClearCard(2, true); // clear card data
            //CardInfoService.CheckErr(result);
            CardInfoService.WriteLog("delete card status:" + result.ToString());
            return result;
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