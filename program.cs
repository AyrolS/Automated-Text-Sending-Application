using System;
using SqlConnect;
using MySql.Data.MySqlClient;
using System.IO.Ports;
using THESIS;

class Program
{
    static SerialPort _serialPort;

    static void Main(string[] args)
    {
        while (true)
        {
            // Prompt user to select COM port
            Console.WriteLine("Please enter the COM port number you want to read from (e.g. COM3):");
            string comPort = Console.ReadLine();

            // Initialize the SerialPort object with user-selected COM port
            _serialPort = new SerialPort(comPort, 9600, Parity.None, 8, StopBits.One);

            try
            {
                _serialPort.Open();
                Console.WriteLine("Serial port connection established successfully.");
                _serialPort.DataReceived += SerialPort_DataReceived;

                // Loop to send AT commands
                while (true)
                {
                    Console.WriteLine("Do you want to start the program? (type y/n) ");
                    string input = Console.ReadLine();

                    if (input == "y" || input == "Y")
                    {
                        // Call read and send functions
                        _serialPort.WriteLine("AT");
                        Thread.Sleep(500);
                        GSMsms gsmSms = new GSMsms();
                        gsmSms.serialPort = _serialPort;
                        gsmSms.Read();

                    }
                    else if (input == "n" || input == "N")
                    {
                        // Exit the loop for AT command selection and go back to COM port selection
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to establish serial port connection: " + ex.Message);
                Console.WriteLine("Please try again.");
            }
            finally
            {
                // Close the SerialPort object if it was opened
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    _serialPort.Close();
                }
            }
        }

    }

    static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        // Read the data from the SerialPort
        string data = _serialPort.ReadLine();
        // Console.WriteLine(data);

        // Check if the response contains "OK" or "ERROR"
        if (data.Contains("OK"))
        {
            Console.WriteLine("GSM: OK");
        }
        else if (data.Contains("ERROR"))
        {
            Console.WriteLine("GSM: ERROR");
        }
    }
}
