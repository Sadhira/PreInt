using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

using System.Threading;

namespace preInt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Thread thread1;
        Thread thread2;
        Thread thread3;
        Thread thread4;

        public static int numSamples = 0;
        public static int numTraces = 0;
        public static int blockSize = 0;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            //setBlockSize
            if (int.TryParse(inNumSamples.Text, out numSamples) && int.TryParse(inNumTraces.Text, out numTraces))
            {
                //MessageBox.Show("NumSamples = " + numSamples);
            }
            else MessageBox.Show("Please enter only integers.");
            
            blockSize = numSamples * numTraces;
            short[] dataBlock = new short[blockSize];
            MessageBox.Show("blockSize = " + blockSize);


        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
        }

        public void TeamA()
        {

        }

        public void TeamB()
        {

        }

        public void Worker1()
        {

        }

        public void Worker2()
        {

        }
    }

}
