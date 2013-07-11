using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public delegate void AddDataDelegate(String myString);
        public delegate void AddDataDelegate2(String myString2);
        public AddDataDelegate myDelegate;
        public AddDataDelegate2 myDelegate2;
        

        private void button1_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == false)
            {
                serialPort1.PortName = comboBox1.SelectedItem.ToString();
                serialPort1.Open();
                button1.Text = "disconnect";
            }
            else
            {
                serialPort1.Close();
                button1.Text = "connect";
            }
               
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = SerialPort.GetPortNames();
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
            this.myDelegate = new AddDataDelegate(AddDataMethod);
            this.myDelegate2 = new AddDataDelegate2(AddDataMethod2);
            System.IO.StreamWriter file = new System.IO.StreamWriter("C:\\Users\\Florian\\Test\\Temperaturauswertung.txt", true);
            file.WriteLine("Datum" + "      " + "Uhrzeit" + "   " + "Sensor1" + "  " + "Sensor2");
            file.Close();
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {           
            SerialPort serialPort1 = (SerialPort)sender;
            string alles = serialPort1.ReadExisting();
            string[] allessplit = alles.Split(new Char [] { ' ' });
            int count = 1;
            foreach (string einzel in allessplit)
            {
                System.Threading.Thread.Sleep(500);
                string filter = einzel;
                if (filter.Length == 5)
                {                    
                     if (count == 1)
                     {
                         textBox1.Invoke(this.myDelegate, new Object[] { filter });
                         count = 2;
                     }
                     else
                     {
                         textBox2.Invoke(this.myDelegate2, new Object[] { filter });
                         count = 1;
                     }
                 }
            }

            System.IO.StreamWriter file = new System.IO.StreamWriter("C:\\Users\\Florian\\Test\\Temperaturauswertung.txt",true);
            //file.WriteLine("Sensor1"+"  " + "Sensor2");
            file.WriteLine(DateTime.Now +"  " + textBox1.Text + "    " + textBox2.Text);
            file.Close();
         }

        private void displayTime()
        {
          DateTime.Now.ToShortTimeString();
        }
            
         public void AddDataMethod(String myString)
        {
            textBox1.Text = myString;
        }

         public void AddDataMethod2(String myString2)
         {         
             textBox2.Text = myString2;
         }   
    }
}
