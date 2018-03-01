using System;

namespace RadioLock
{
    public class orbitaCard
    {
        /*
        Value Description
        -1 Interface error
        -2 Connect encoder failed
        -3 Register encoder failed
        -4 Buzzer mute
        -5 Not supported card type
        -6 Wrong card password
        -7 Wrong supplier password
        -8 Wrong card type
        -9 Wrong authorization code
        -10 Find card request failed
        -11 Find card failed
        -12 Load card password failed
        -13 Read device information failed
        -14 Read card failed
        -15 Write card failed
            */

        //auth, cardno, building, room, commdoors, arrival, departure
        private int _status;

        public int status
        {
            get { return _status; }
            set { _status = value; }
        }

        private String _auth;

        public String auth
        {
            get { return _auth; }
            set { _auth = value; }
        }

        private String _cardno;

        public String cardno
        {
            get { return _cardno; }
            set { _cardno = value; }
        }

        private String _building;

        public String building
        {
            get { return _building; }
            set { _building = value; }
        }

        private String _room;

        public String room
        {
            get { return _room; }
            set { _room = value; }
        }

        private String _commdoors;

        public String commdoors
        {
            get { return _commdoors; }
            set { _commdoors = value; }
        }

        private String _arrival;

        public String arrival
        {
            get { return _arrival; }
            set { _arrival = value; }
        }

        private String _departure;

        public String departure
        {
            get { return _departure; }
            set { _departure = value; }
        }
    }
}