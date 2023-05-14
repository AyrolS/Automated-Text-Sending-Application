using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using SqlConnect;
using MySql.Data.MySqlClient;

namespace Automated-Text-Sending-Application
{
    class GSMsms
    {
        public SerialPort serialPort;
        
        public void Read()
        {

            GSMsms sms = new GSMsms();
            sms.serialPort = serialPort;

            // Set message format to text mode
            string AT = $"AT+CMGF=1\r\n"; // Set SMS message format to text mode
            serialPort.Write(AT);
            System.Threading.Thread.Sleep(500); 

            // Set message storage to SIM card
            AT = $"AT+CPMS=\"SM\"\r\n"; // Set message storage to SIM card
            serialPort.Write(AT);
            System.Threading.Thread.Sleep(500); 

            AT = $"AT+CSCS=\"GSM\"\r\n"; 
            serialPort.Write(AT);
            System.Threading.Thread.Sleep(500); 

            // Enable unsolicited message indications
            AT = $"AT+CNMI=2,2,0,0,0\r\n"; // Enable unsolicited message indications
            serialPort.Write(AT);
            System.Threading.Thread.Sleep(500); 

            Console.WriteLine("Waiting for incoming message...");
            
            
            while (true)
            {
                // Read response from GSM module
                string response = serialPort.ReadLine();
                DbConnector db = new DbConnector();

                if (!string.IsNullOrEmpty(response))
                {
                    Console.WriteLine(response);
                    string message = response;

                    //distance 4
                    if (message.IndexOf("distance 4", StringComparison.OrdinalIgnoreCase) >= 0){
                        db.Open();
                        string query = $"INSERT INTO table_x (column_1, column_2) VALUES ('x feet away from sensor', NOW())";
                
                        MySqlCommand command = new MySqlCommand(query1, db.Connection);
                        int rowsAffected = command.ExecuteNonQuery();

                        if(rowsAffected > 0){
                            Console.WriteLine($"{rowsAffected} rows affected.");
                            sms.send_alert1();
                            db.Close();
                        }

                        AT = $"AT+CMGD=1,4\r\n"; //deletes all messages for a fresh inbox everytime
                        serialPort.Write(AT);
                        System.Threading.Thread.Sleep(250);

                        continue;
                    }
                    //distance 3 
                    else if(message.IndexOf("distance 3", StringComparison.OrdinalIgnoreCase) >= 0){
                        db.Open();
                         string query = $"INSERT INTO table_x (column_1, column_2) VALUES ('x feet away from sensor', NOW())";
                
                        MySqlCommand command = new MySqlCommand(query2, db.Connection);
                        int rowsAffected = command.ExecuteNonQuery();
                        if(rowsAffected > 0){
                            Console.WriteLine($"{rowsAffected} rows affected.");
                            sms.send_alert2();
                            db.Close();
                        }

                        AT = $"AT+CMGD=1,4\r\n"; //deletes all messages for a fresh inbox everytime
                        serialPort.Write(AT);
                        System.Threading.Thread.Sleep(250);

                        continue;
                    }
                    //distance 2 
                    else if(message.IndexOf("distance 2", StringComparison.OrdinalIgnoreCase) >= 0){
                        db.Open();
                       string query = $"INSERT INTO table_x (column_1, column_2) VALUES ('x feet away from sensor', NOW())";;
                
                        MySqlCommand command = new MySqlCommand(query3, db.Connection);
                        int rowsAffected = command.ExecuteNonQuery();
                        if(rowsAffected > 0){
                            Console.WriteLine($"{rowsAffected} rows affected.");
                            sms.send_alert3();
                            db.Close();
                        }

                        AT = $"AT+CMGD=1,4\r\n"; //deletes all messages for a fresh inbox everytime
                        serialPort.Write(AT);
                        System.Threading.Thread.Sleep(250);

                        continue;
                    }
                    //distance 1 
                    else if (message.IndexOf("distance 1", StringComparison.OrdinalIgnoreCase) >= 0){
                        db.Open();
                        string query = $"INSERT INTO table_x (column_1, column_2) VALUES ('x feet away from sensor', NOW())";;
                
                        MySqlCommand command = new MySqlCommand(query2, db.Connection);
                        int rowsAffected = command.ExecuteNonQuery();
                        if(rowsAffected > 0){
                            Console.WriteLine($"{rowsAffected} rows affected.");
                            sms.send_alert4();
                            db.Close();
                        }
                        
                        AT = $"AT+CMGD=1,4\r\n"; //deletes all messages for a fresh inbox everytime
                        serialPort.Write(AT);
                        System.Threading.Thread.Sleep(250);

                        continue;
                    }
                    else{
                        continue;
                    }
                }
            }
        }

        public void send_alert1()
        {
            // Set message format to text mode
            string AT = $"AT+CMGF=1\r\n"; // Set SMS message format to text mode
            serialPort.Write(AT);
            System.Threading.Thread.Sleep(500); // Wait for command to be sent

            Console.WriteLine("Sending...");

            DbConnector db = new DbConnector();
            db.Open();

            string query = "SELECT co_number FROM account WHERE LENGTH(co_number) = 13";
            MySqlDataReader reader = db.ExecuteQuery(query);

            string message = "ALERT LEVEL 1";

            while (reader.Read())
            {
                string coNumber = reader.GetString("co_number");

                bool sent = false;
                int tries = 0;
                while (!sent && tries < 3)
                {
                    serialPort.WriteLine($"AT+CMGS=\"{coNumber}\"");
                    Thread.Sleep(1000);
                    serialPort.WriteLine(message + char.ConvertFromUtf32(26));
                    Thread.Sleep(1000);

                    string response = serialPort.ReadExisting();
                    Console.WriteLine(response);

                    if (response.Contains("OK"))
                    {
                        sent = true;
                    }
                    else
                    {
                        tries++;
                    }
                }
            }

            reader.Close();
        }

        public void send_alert2()
        {
            // Set message format to text mode
            string AT = $"AT+CMGF=1\r\n"; // Set SMS message format to text mode
            serialPort.Write(AT);
            System.Threading.Thread.Sleep(500); // Wait for command to be sent

            Console.WriteLine("Sending...");

            DbConnector db = new DbConnector();
            db.Open();

            string query = "SELECT co_number FROM account WHERE LENGTH(co_number) = 13";
            MySqlDataReader reader = db.ExecuteQuery(query);

            string message = "ALERT LEVEL 2";

            while (reader.Read())
            {
                string coNumber = reader.GetString("co_number");

                bool sent = false;
                int tries = 0;
                while (!sent && tries < 3)
                {
                    serialPort.WriteLine($"AT+CMGS=\"{coNumber}\"");
                    Thread.Sleep(1000);
                    serialPort.WriteLine(message + char.ConvertFromUtf32(26));
                    Thread.Sleep(1000);

                    string response = serialPort.ReadExisting();
                    Console.WriteLine(response);

                    if (response.Contains("OK"))
                    {
                        sent = true;
                    }
                    else
                    {
                        tries++;
                    }
                }
            }

            reader.Close();
        }

        public void send_alert3()
        {
            // Set message format to text mode
            string AT = $"AT+CMGF=1\r\n"; // Set SMS message format to text mode
            serialPort.Write(AT);
            System.Threading.Thread.Sleep(500); // Wait for command to be sent

            Console.WriteLine("Sending...");

            DbConnector db = new DbConnector();
            db.Open();

            string query = "SELECT co_number FROM account WHERE LENGTH(co_number) = 13";
            MySqlDataReader reader = db.ExecuteQuery(query);

            string message = "ALERT LEVEL 3";

            while (reader.Read())
            {
                string coNumber = reader.GetString("co_number");

                bool sent = false;
                int tries = 0;
                while (!sent && tries < 3)
                {
                    serialPort.WriteLine($"AT+CMGS=\"{coNumber}\"");
                    Thread.Sleep(1000);
                    serialPort.WriteLine(message + char.ConvertFromUtf32(26));
                    Thread.Sleep(1000);

                    string response = serialPort.ReadExisting();
                    Console.WriteLine(response);

                    if (response.Contains("OK"))
                    {
                        sent = true;
                    }
                    else
                    {
                        tries++;
                    }
                }
            }

            reader.Close();
        }

        public void send_alert4()
        {
            // Set message format to text mode
            string AT = $"AT+CMGF=1\r\n"; // Set SMS message format to text mode
            serialPort.Write(AT);
            System.Threading.Thread.Sleep(500); // Wait for command to be sent

            Console.WriteLine("Sending...");

            DbConnector db = new DbConnector();
            db.Open();

            string query = "SELECT co_number FROM account WHERE LENGTH(co_number) = 13";
            MySqlDataReader reader = db.ExecuteQuery(query);

            string message = "ALERT LEVEL 4";

            while (reader.Read())
            {
                string coNumber = reader.GetString("co_number");

                bool sent = false;
                int tries = 0;
                while (!sent && tries < 3)
                {
                    serialPort.WriteLine($"AT+CMGS=\"{coNumber}\"");
                    Thread.Sleep(1000);
                    serialPort.WriteLine(message + char.ConvertFromUtf32(26));
                    Thread.Sleep(1000);

                    string response = serialPort.ReadExisting();
                    Console.WriteLine(response);

                    if (response.Contains("OK"))
                    {
                        sent = true;
                    }
                    else
                    {
                        tries++;
                    }
                }
            }

            reader.Close();
        }


    }
}
