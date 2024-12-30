using SerialPort_Con.Commands;
using SerialPort_Con.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SerialPort_Con.ViewModels
{
    public class PrinterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        private SerialPort serialPort;
        public ICommand ConnectCommand => new RelayCommand(async () => await ConnectToSerialPort());
        public ICommand SendCommand => new RelayCommand(SendDataToSerialPort);


        private Printers_ printerModel;
        public Printers_ PrinterModel
        {
            get => printerModel;
            set
            {
                printerModel = value;
                OnPropertyChanged(nameof(PrinterModel));
            }
        }

        private ObservableCollection<string> receivedData;
        public ObservableCollection<string> ReceivedData
        {
            get => receivedData;
            set
            {
                receivedData = value;
                OnPropertyChanged(nameof(ReceivedData));
            }
        }


        private string sentData;
        public string SentData
        {
            get => sentData;
            set
            {
                sentData = value;
                OnPropertyChanged(nameof(SentData));
            }
        }

        private string connectionStatus;
        public string ConnectionStatus
        {
            get => connectionStatus;
            set
            {
                connectionStatus = value;
                OnPropertyChanged(nameof(ConnectionStatus));
            }
        }

        public PrinterViewModel()
        {
            printerModel = new Printers_();
            serialPort = new SerialPort();
            ConnectionStatus = "Disconnected";
        }

        private async Task ConnectToSerialPort()
        {
            if (!serialPort.IsOpen)
            {
                try
                {
                    if (PrinterModel.BaudRate <= 0)
                    {
                        throw new ArgumentOutOfRangeException("BaudRate", "BaudRate must be a positive integer.");
                    }

                    serialPort.PortName = PrinterModel.PortName;
                    serialPort.BaudRate = PrinterModel.BaudRate;
                    serialPort.DataBits = PrinterModel.DataSize;
                    serialPort.Parity = PrinterModel.Parity;
                    serialPort.Handshake = PrinterModel.Handshake;
                    
                    // Subscribe to data received event
                    serialPort.DataReceived += SerialPort_DataReceived;

                    // Open the serial port
                    serialPort.Open();

                    ConnectionStatus = "Connected";
                    // Optionally, you can show a message or change UI state once connected
                    Console.WriteLine($"Connected to {serialPort.PortName} at {serialPort.BaudRate} Baud.");
                }
                catch (Exception ex)
                {
                    // Handle connection error
                    Console.WriteLine($"Error connecting to serial port: {ex.Message}");
                }
            }
        }

        // Method to send data to the serial port
        private void SendDataToSerialPort()
        {
            if (serialPort.IsOpen && !string.IsNullOrEmpty(SentData))
            {
                try
                {
                    serialPort.WriteLine(SentData);
                    // Optionally, clear the SendData text box after sending
                    SentData = string.Empty;
                    Console.WriteLine($"Sent: {SentData}");
                }
                catch (Exception ex)
                {
                    // Handle error while sending data
                    Console.WriteLine($"Error sending data: {ex.Message}");
                }
            }
        }

        // Method to handle data received from the serial port
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = serialPort.ReadExisting();
            App.Current.Dispatcher.Invoke(() =>
            {
                ReceivedData.Add(data);
            });
        }
    }
}
