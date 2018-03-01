using System;
using System.Runtime.InteropServices;
using System.Text;

namespace RadioLock
{
    internal class LLockConnector
    {
        public static string ConnectionString = "Data Source=.\\SQLHotelLock;Initial Catalog=RadioLock;Integrated Security=False;User Id=sa;Password=123;MultipleActiveResultSets=True";

        #region Radio Lock 

        [DllImport("LockSDK.dll", EntryPoint = "TP_Configuration")]
        public static extern int TP_Configuration(Int16 LockType);

        [DllImport("LockSDK.dll", EntryPoint = "TP_MakeGuestCard")]
        public static extern int TP_MakeGuestCard(StringBuilder card_snr, string room_no, string checkin_time, string checkout_time, Int16 iflags);

        [DllImport("LockSDK.dll", EntryPoint = "TP_ReadGuestCard")]
        public static extern int TP_ReadGuestCard(StringBuilder card_snr, StringBuilder room_no, StringBuilder checkin_time, StringBuilder checkout_time);

        [DllImport("LockSDK.dll", EntryPoint = "TP_ReadGuestCardEx")]
        public static extern int TP_ReadGuestCardEx(StringBuilder card_snr, StringBuilder room_no, StringBuilder checkin_time, StringBuilder checkout_time, ref int iflags);

        [DllImport("LockSDK.dll", EntryPoint = "TP_CancelCard")]
        public static extern int TP_CancelCard(StringBuilder card_snr);

        [DllImport("LockSDK.dll", EntryPoint = "TP_GetCardSnr")]
        public static extern int TP_GetCardSnr(StringBuilder card_snr);

        #endregion Radio Lock

        internal static Dev_C_Sharp.Dev_C_Sharp devCommand = Dev_C_Sharp.Dev_C_Sharp.Instance;

        public int connecToLock(Int16 LockType)
        {
            int result = 0;
            result = TP_Configuration(LockType);
            return result;
        }

        public CardInfo1 ReadCard1()
        {
            StringBuilder card_snr = new StringBuilder(100);
            StringBuilder roomno = new StringBuilder(100);
            StringBuilder intime = new StringBuilder(100);
            StringBuilder outtime = new StringBuilder(100);
            //string strMsg = "";
            int iflags = 0;
            int result = TP_ReadGuestCardEx(card_snr, roomno, intime, outtime, ref iflags);
            if (result == 1)
            {
                //strMsg = Language.g_LoadString_Ex("IDS_STRING_CARDNO") + card_snr.ToString() + "\n";
                //strMsg += Language.g_LoadString_Ex("IDS_STRING_LOCKNO") + roomno.ToString() + "\n";
                //strMsg += Language.g_LoadString_Ex("IDS_STRING_INTIME") + intime.ToString() + "\n";
                //strMsg += Language.g_LoadString_Ex("IDS_STRING_OUTTIME") + outtime.ToString() + "\n";
                //strMsg += Language.g_LoadString_Ex("IDS_STRING_FLAGS") + "0x" + iflags.ToString("X1") + "\n";
                //MessageBox.Show(strMsg, Language.g_LoadString_Ex("IDS_STRING_MSG"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                result = 0;
            }
            //else
            //{
            //    CardInfoService.CheckErr(result);
            //}
            var retval = new CardInfo1 { result = result };
            if (result == 0)
            {
                var date = new DateTime();
                var date2 = new DateTime();
                DateTime.TryParse(intime.ToString(), out date);
                DateTime.TryParse(outtime.ToString(), out date2);
                retval.arrivalDate = date;
                retval.departureDate = date2;
                retval.cardNo = card_snr.ToString();
                retval.room = roomno.ToString();
                retval.flags = iflags;
            }

            CardInfoService.WriteLog("ReadCard: " + "Result:" + retval.result + "Room-" + retval.room + "" + "CardNo-" + retval.cardNo + "ArrivalDate-" + retval.arrivalDate +
                "DepartureDate-" + retval.departureDate);
            return retval;
        }

        public StringBuilder ReadCardBeforeWrite()
        {
            StringBuilder card_snr = new StringBuilder(100);
            StringBuilder roomno = new StringBuilder(100);
            StringBuilder intime = new StringBuilder(100);
            StringBuilder outtime = new StringBuilder(100);
            int result = TP_ReadGuestCard(card_snr, roomno, intime, outtime);
            StringBuilder cardSnr = new StringBuilder(100);
            if (result == 1)
            {
                cardSnr = card_snr;
            }

            //CardInfoService.CheckErr(result);
            CardInfoService.WriteLog("ReadCardBeforeWrite:" + result.ToString() + ", cardSnr:" + cardSnr);

            return cardSnr;
        }

        public int WriteCard(string room_no, string checkin_time, string checkout_time, Int16 iflags)
        {
            StringBuilder card_snr = ReadCardBeforeWrite();
            int result = 0;
            result = TP_MakeGuestCard(card_snr, room_no, checkin_time, checkout_time, iflags);
            //CardInfoService.CheckErr(result);
            CardInfoService.WriteLog("WriteCard:" + result.ToString() + ", room_no:" + room_no + ",checkin_time:" + checkin_time + ",checkout_time:" + checkout_time + "iflags:" + iflags);
            if (result == 1) result = 0;
            return result;
        }

        public int DeleteCard()
        {
            StringBuilder card_snr = ReadCardBeforeWrite();
            int result = 0;
            result = TP_CancelCard(card_snr);
            //CardInfoService.CheckErr(result);
            CardInfoService.WriteLog("Delete Card card_snr:" + card_snr + ",status:" + result.ToString());
            if (result == 1) result = 0;
            return result;
        }
    }
}