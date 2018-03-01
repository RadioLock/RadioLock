using System;

namespace RadioLock
{
    public class CardInfoRequest1
    {
        public int reservationRoomId { get; set; }
        public int roomId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
    public class CardInfoResponse1
    {
        public int result { get; set; }
        public string cardType { get; set; }
        public string cardNumber { get; set; }
        public string validTime { get; set; }
        public string cardData { get; set; }
        public int flags { get; set; }
        public string response { get; set; }
    }

    public class CardInfoResponse2
    {
        public int result { get; set; }
        public string response { get; set; }
    }
}