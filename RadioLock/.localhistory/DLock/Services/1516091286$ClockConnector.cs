using System;
using System.Runtime.InteropServices;

namespace RadioLock
{
    internal class ClockConnector
    {
        public static string ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\\EZCode\\ezcloudconnectivity\\ELock\\Document\\Data\\eLock.mdb;Jet OLEDB:Database Password =eLock0618;";

        [DllImport("CLock.dll",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 dv_connect(Int16 beep);

        [DllImport("CLock.dll",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 dv_read_card([In, Out] char[] auth, [In, Out] char[] cardno, [In, Out] char[] building, [In, Out] char[] room, [In, Out] char[] commdoors, [In, Out] char[] arrival, [In, Out] char[] departure);

        [DllImport("CLock.dll",
           CharSet = CharSet.Ansi,
           CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 dv_get_auth_code([In, Out] char[] auth);

        [DllImport("CLock.dll",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 dv_write_card([In, Out] char[] auth, [In, Out] char[] building, [In, Out] char[] room, [In, Out] char[] commdoors, [In, Out] char[] arrival, [In, Out] char[] departure);

        [DllImport("CLock.dll",
           CharSet = CharSet.Ansi,
           CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 dv_delete_card([In, Out] char[] room);

        public static int CreateCard(string room, string cardno, DateTime indate, DateTime outdate)
        {
            while (room.Length < 4)
            {
                room = "0" + room;
            }
            string arrival = string.Format("{0:yyyy-MM-dd HH:mm:ss}", indate);
            string departure = string.Format("{0:yyyy-MM-dd HH:mm:ss}", outdate);
            var result = dv_write_card(Helper.CharFromString(Settings.Default.SystemCode), new char[2], Helper.CharFromString(room), Helper.CharFromString(cardno), Helper.CharFromString(arrival), Helper.CharFromString(departure));
            return result;
        }

        public static int CreateCard(string _floor_name, string _cardno, string _room, DateTime _indate, DateTime _outdate)
        {
            if (_floor_name.Length == 0)
            {
                if (_room.Length > 0) _floor_name = _room.Substring(0, 1);
            }
            string arrival = string.Format("{0:yyyy-MM-dd HH:mm:ss}", _indate);
            string departure = string.Format("{0:yyyy-MM-dd HH:mm:ss}", _outdate);
            var auth = Settings.Default.SystemCode;
            var building = "01";//_floor_name; //KS co mot tang
            var room = _room;
            var commdoors = _cardno;

            // var result = dv_write_card(CharFromString(Settings.Default.SystemCode), new char[2], CharFromString(room), CharFromString(cardno), CharFromString(arrival), CharFromString(departure));
            //dv_write_card([In, Out] char[] auth, [In, Out] char[] building, [In, Out] char[] room, [In, Out] char[] commdoors, [In, Out] char[] arrival, [In, Out] char[] departure);
            var result = dv_write_card(Helper.CharFromString(auth), Helper.CharFromString(building), Helper.CharFromString(room), Helper.CharFromString(commdoors), Helper.CharFromString(arrival), Helper.CharFromString(departure));
            return result;
        }

        public static CardInfo ReadCard()
        {
            var cardno = new char[6];
            var building = new char[200];
            var room = new char[4];
            var commdoors = new char[8];
            var arrival = new char[20];
            var departure = new char[20];
            //[In, Out] char[] auth, [In, Out] char[] cardno, [In, Out] char[] building, [In, Out] char[] room, [In, Out] char[] commdoors, [In, Out] char[] arrival, [In, Out] char[] departure
            int result = dv_read_card(Helper.CharFromString(Settings.Default.SystemCode), cardno, building, room, commdoors, arrival, departure);

            //int result = dv_read_card(CharFromString( Settings.Default.SystemCode), cardno, building, room, commdoors, arrival, departure);
            var retval = new CardInfo { result = result };
            if (result == 0)
            {
                var date = new DateTime();
                var date2 = new DateTime();
                DateTime.TryParse(Helper.CharToString(arrival), out date);
                DateTime.TryParse(Helper.CharToString(departure), out date2);
                retval.arrivalDate = date;
                retval.departureDate = date2;
                retval.cardNo = Helper.CharToString(commdoors);
                retval.room = Helper.CharToString(room);
            }
            return retval;
        }
    }
}