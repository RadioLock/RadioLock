using System;

namespace RadioLock
{
    public class CardInfoRequest1
    {
        public int reservationRoomId { get; set; }
        public string roomName { get; set; }
        //public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }

    public class CardInfoResponse1
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public string cardNumber { get; set; }
        public string reservationRoomId { get; set; }
        public string roomName { get; set; }
        public string validTime { get; set; }
    }

    public class RoomInfo
    {
        public string b_code { get; set; }
        public string f_code { get; set; }
        public string r_code { get; set; }
        public string r_subcode { get; set; }
        public string r_subcodedai { get; set; }
    }
}