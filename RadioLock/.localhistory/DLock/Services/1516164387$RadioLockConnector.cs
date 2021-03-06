﻿using System;
using System.Runtime.InteropServices;
using System.Text;

namespace RadioLock
{
    internal class LLockConnector
    {
        public static string ConnectionString = "Data Source=.\\SQLHotelLock;Initial Catalog=RadioLock;Integrated Security=False;User Id=sa;Password=123;MultipleActiveResultSets=True";

        internal static Dev_C_Sharp.Dev_C_Sharp devCommand = Dev_C_Sharp.Dev_C_Sharp.Instance;
        internal static int WriteGuestCard()
        {
            return devCommand.WriteCard(0, 0, "", "", 0, false);
        }

        //public int connecToLock(Int16 LockType)
        //{
        //    int result = 0;
        //    result = TP_Configuration(LockType);
        //    return result;
        //}

        public CardInfo1 ReadCard()
        {
            //byte cardtype = 255;
            //string carddata = "";
            //int result = devCommand.ReadCard(out cardtype, ref carddata, true);

            StringBuilder lockinfo = new StringBuilder(2048);
            StringBuilder recstr = new StringBuilder(7680);
            int result = devCommand.ReadCardS70(lockinfo, recstr, true);

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

            result = devCommand.WriteCard(CardType.card_Room_number, room_no, checkin_time, checkout_time, iflags);
            //CardInfoService.CheckErr(result);
            CardInfoService.WriteLog("WriteCard:" + result.ToString() + ", room_no:" + room_no + ",checkin_time:" + checkin_time + ",checkout_time:" + checkout_time + "iflags:" + iflags);
            if (result == 1) result = 0;
            return result;
        }

        public int DeleteCard()
        {
            //StringBuilder card_snr = ReadCardBeforeWrite();
            int result = 0;
            result = devCommand.ClearCard(2, true); // clear card data
            //CardInfoService.CheckErr(result);
            //CardInfoService.WriteLog("Delete Card card_snr:" + card_snr + ",status:" + result.ToString());
            if (result == 1) result = 0;
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