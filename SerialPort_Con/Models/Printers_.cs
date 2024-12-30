using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPort_Con.Models
{
    public  class Printers_ : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private string portName;
        public string PortName
        {
            get => portName;
            set
            {
                portName = value;
                OnPropertyChanged(nameof(PortName));
            }
        }

        private int baudRate;
        public int BaudRate
        {
            get => baudRate;
            set
            {
                baudRate = value;
                OnPropertyChanged(nameof(BaudRate));
            }
        }

        private int dataSize;
        public int DataSize
        {
            get => dataSize;
            set
            {
                dataSize = value;
                OnPropertyChanged(nameof(DataSize));
            }
        }

        private Parity parity;
        public Parity Parity
        {
            get => parity;
            set
            {
                parity = value;
                OnPropertyChanged(nameof(Parity));
            }
        }

        private Handshake handshake;
        public Handshake Handshake
        {
            get => handshake;
            set
            {
                handshake = value;
                OnPropertyChanged(nameof(Handshake));
            }
        }

        private string mode;
        public string Mode
        {
            get => mode;
            set
            {
                mode = value;
                OnPropertyChanged(nameof(Mode));
            }
        }

    }
}
