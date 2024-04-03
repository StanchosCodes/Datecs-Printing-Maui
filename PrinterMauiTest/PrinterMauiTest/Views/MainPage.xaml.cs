using System.IO.Ports;
using System.Runtime.InteropServices;

namespace PrinterMauiTest.Views
{
    public partial class MainPage : ContentPage
    {
        static SerialPort serialPort;

        [ComVisible(true)]
        [DllImport("Printing.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int PrnOpen(int port, int speed, bool hardware);

        [ComVisible(true)]
        [DllImport("Printing.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int PrnClose();

        [ComVisible(true)]
        [DllImport("Printing.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int PrnText(string printtext);

        [ComVisible(true)]
        [DllImport("Printing.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int PrnCodepage(int codepage);

        string textToPrint = string.Empty;

        public MainPage()
        {
            InitializeComponent();

            serialPort = new SerialPort();

            foreach (string name in SerialPort.GetPortNames())
            {
                portsDropdown.Items.Add(name);
            }

            portsDropdown.SelectedIndexChanged += OnSelectionDropdownChanged!;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            string oldText = e.OldTextValue;
            string newText = e.NewTextValue;
            string currentText = entry.Text;

            textToPrint = currentText;
        }

        private void OnEntryCompleted(object sender, EventArgs e)
        {
            string text = ((Entry)sender).Text;

            textToPrint = text;
        }

        private void OnPrintBtnClicked(object sender, EventArgs e)
        {
            string? selectedPort = portsDropdown.SelectedItem.ToString();
            int selectedPortNum = default;

            if (selectedPort != null)
            {
                selectedPortNum = int.Parse(selectedPort.Substring(3));
            }

            serialPort.BaudRate = 57600;
            int codePageNumber = 1251; // cyrillic code table

            int prnOpenresponse = PrnOpen(selectedPortNum, serialPort.BaudRate, false);

            int codePageResponse = PrnCodepage(codePageNumber);
            int prnTextResponse = PrnText(textToPrint);

            PrnClose();

            entry.Text = string.Empty;
        }

        private void OnSelectionDropdownChanged(object sender, EventArgs e)
        {
            string? selectedPort = portsDropdown.SelectedItem.ToString();
        }
    }

}
