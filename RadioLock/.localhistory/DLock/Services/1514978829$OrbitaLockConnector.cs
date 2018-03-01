using System;

namespace ELock
{
    internal class GLockConnector
    {
        public GLockConnector()
        {
        }

        /*
      Kiem tra ket noi voi thiet bi
           0 Ket noi thanh cong
          -2 Connect encoder failed

          */

        public int orbita_Connect()
        {
            int _return;
            try
            {
                _return = ClockConnector.dv_connect(1000);
            }
            catch (Exception e)
            {
                throw;
            }

            return _return;
        }

        /*
             Doc the
             0 Ket noi thanh cong
            -2 Connect encoder failed

            */

        public orbitaCard orbita_Read_card()
        {
            orbitaCard objCard = new orbitaCard();

            string _auth = "";//ConfigurationManager.AppSettings["Author"].ToString();
            try
            {
                var auth = _auth.ToCharArray();//  new char[6] { '5', '7', '5', '4', '2', '0' };
                var cardno = new char[6];
                var building = new char[2];
                var room = new char[4];
                var commdoors = new char[8];
                var arrival = new char[20];
                var departure = new char[20];

                int _return = ClockConnector.dv_read_card(auth, cardno, building, room, commdoors, arrival, departure);

                objCard.status = _return;
                objCard.auth = _auth;
                objCard.building = new string(building);
                objCard.cardno = new string(cardno);
                objCard.room = new string(room);
                objCard.commdoors = new string(commdoors);
                objCard.arrival = new string(arrival);
                objCard.departure = new string(departure);
                //Console.WriteLine("{0} {1} {2} {3} {4} {5}", new string(cardno), new string(building), new string(room), new string(commdoors), new string(arrival), new string(departure));
            }
            catch (Exception ex)
            {
                throw;
            }

            return objCard;
        }

        /*
             Tien hanh tao the

             0 Ket noi thanh cong
            -15 Write card failed

            */

        public int orbita_Write_card(string _building, string _room, string _commdoors, string _arrival, string _departure)
        {
            int _return;
            string _auth = "";// CharFromString(Settings.Default.SystemCode);//ConfigurationManager.AppSettings["Author"].ToString();
            try
            {
                var auth = _auth.ToCharArray(); //new char[6] { '5', '7', '5', '4', '2', '0' };
                var building = _building.ToCharArray();
                var room = _room.ToCharArray();
                var commdoors = _commdoors.ToCharArray();
                var arrival = _arrival.ToCharArray();
                var departure = _departure.ToCharArray();

                _return = ClockConnector.dv_write_card(auth, building, room, commdoors, arrival, departure);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _return;
        }

        /*
             Tien hanh tao the

             0 Ket noi thanh cong
            -15 Write card failed

            */

        public orbitaCard orbita_delete_card(string _room)
        {
            int _return;
            orbitaCard objCard = new orbitaCard();
            string _auth = "";// ConfigurationManager.AppSettings["Author"].ToString();
            try
            {
                var auth = _auth.ToCharArray(); //new char[6] { '5', '7', '5', '4', '2', '0' };
                var room = _room.ToCharArray();
                _return = ClockConnector.dv_delete_card(room);

                objCard.status = _return;
                objCard.auth = _auth;
                objCard.room = new string(room);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objCard;
        }

        /*
             Get Authori

             0 Ket noi thanh cong
            -15 Write card failed

            */

        public orbitaCard orbita_get_auth_code()
        {
            int _return;
            orbitaCard objCard = new orbitaCard();
            try
            {
                var auth = new char[6];

                _return = ClockConnector.dv_get_auth_code(auth);

                objCard.status = _return;
                objCard.auth = new string(auth);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objCard;
        }
    }
}