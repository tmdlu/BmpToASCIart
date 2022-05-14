using BmpToASCIIart.Application;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace BmpToASCIIart
{

    



    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

  
        string name = null;
        public int readSize(byte val1, byte val2)
        {

            int result = val2 << 8 | val1;
            return result;
        }

        public int test(string x)
        {
            int width;
            byte[] buff = null;
            System.IO.FileStream file = new FileStream(x, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(file);
            long numBytes = new FileInfo(x).Length;
            buff = reader.ReadBytes((int)numBytes);
            width = readSize(buff[18], buff[19]);
            return width;
        }


        public MainWindow()
        {
            InitializeComponent();
           


            
            slValue.Value = Environment.ProcessorCount;
            


        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "BMP files (*.bmp)|*.bmp";

            if(openFileDialog.ShowDialog()== true)
            {
                name = openFileDialog.FileName;
                pic.Source = new BitmapImage(new Uri(name));
                
                    
                
                
            }
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            
            if (name!=null && !(asm.IsChecked==false && cpp.IsChecked==false) )
            {
                string choice;
                if (asm.IsChecked == true)
                    choice = "asm";
                else
                    choice = "cpp";

                ascii.Text = "";
                string com = "";
                BMP bmp = new BMP(name, Convert.ToInt32( slValue.Value));
                string[] result = bmp.readBMP(ref com, choice);
                for (int i = 0; i < result.Length; i++)
                {
                    ascii.Text += result[i];
                    ascii.Text += "\n";
                }
                output.Text = com;
                time.Text = bmp.getTime().ToString();
               
                
            }
        }

       

      

       
    }
}
