using System;

namespace ELock
{
    public class CardInfo
    {
        public int result { get; set; }
        public string cardNo { get; set; }
        public string building { get; set; }
        public string room { get; set; }
        public DateTime arrivalDate { get; set; }
        public DateTime departureDate { get; set; }
        public string arrivalDate2 { get; set; }
        public string departureDate2 { get; set; }
    }

    public class CardInfo1
    {
        public int result { get; set; }
        public string cardNo { get; set; }
        public string room { get; set; }
        public DateTime arrivalDate { get; set; }
        public DateTime departureDate { get; set; }
        public int flags { get; set; }
    }

    public class CardInfo2
    {
        public string RoomName { get; set; }
        public string TravellerName { get; set; }
        public string TravellerId { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public bool OverrideOldCards { get; set; }
    }

    public class CardInfoResponse2
    {
        public int Result { get; set; } //0:success;1:fail
        public string RoomName { get; set; }
        public string TravellerName { get; set; }
        public string TravellerId { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public int cardNumber { get; set; }
    }
}