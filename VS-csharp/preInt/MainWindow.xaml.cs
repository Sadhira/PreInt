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
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Threading;

namespace preInt
{
    /// <summary>
    /// 
    /// Sadhira Wagiswara - Pre-Interview Task
    /// 
    /// Highly functional backend for Threads 1 and 2, except that the UI list is not updated.
    /// For Threads 3 and 4, please note that there is pseudocode written in the main Thread 3 function (worker1_DoWork).
    /// 
    /// Thank you.
    /// 
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        BackgroundWorker teamA = new BackgroundWorker();
        BackgroundWorker teamB = new BackgroundWorker();
        BackgroundWorker worker1 = new BackgroundWorker();
        BackgroundWorker worker2 = new BackgroundWorker();

        
       
        //
        public static int numWorkers = 2;
        public static int numSamples = 3;
        public static int numTraces = 5;
        public static int blockSize = 0;

        Interface dataInterface = new Interface(numSamples, blockSize);

       

        TraceList traceList;  
        List<string> timeList = new List<string>();
       


        private int increment = 0;

        private void dtTicker(object sender, EventArgs e)
        {
            increment++;
            debugLabel.Content = increment.ToString();
        }

        public MainWindow()
        {
            InitializeComponent();
           
            teamA.WorkerSupportsCancellation = true;
            teamA.WorkerReportsProgress = true;
            teamA.DoWork += teamA_DoWork;
            teamA.ProgressChanged += teamA_ProgressChanged;
            teamA.RunWorkerCompleted += teamA_RunWorkerCompleted;

            teamB.WorkerSupportsCancellation = true;
            teamB.WorkerReportsProgress = true;
            teamB.DoWork += teamB_DoWork;
            teamB.ProgressChanged += teamB_ProgressChanged;
            teamB.RunWorkerCompleted += teamB_RunWorkerCompleted;


            worker1.WorkerSupportsCancellation = true;
            worker1.WorkerReportsProgress = true;
            worker1.DoWork += worker1_DoWork;
            worker1.ProgressChanged += worker1_ProgressChanged;
            worker1.RunWorkerCompleted += worker1_RunWorkerCompleted;


            // commented out to reduce clutter, see pseudocode under worker1 for more +++++++++++++++++++
            worker2.WorkerSupportsCancellation = true;
            worker2.WorkerReportsProgress = true;
            worker2.DoWork += worker2_DoWork;
            worker2.ProgressChanged += worker2_ProgressChanged;
            worker2.RunWorkerCompleted += worker2_RunWorkerCompleted;
          
        }
        

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            //setBlockSize
            if (int.TryParse(inNumSamples.Text, out numSamples) && int.TryParse(inNumTraces.Text, out numTraces)) {
        
            }
            else MessageBox.Show("Please enter only integers.");
            
            blockSize = numSamples * numTraces;
            //ushort[] dataBlock = new ushort[blockSize];
           
            TraceList traceList = new TraceList();
            dataInterface = new Interface(numSamples, blockSize);

            Debug.WriteLine("blockSize = " + blockSize + ", numSumples = " + numSamples);
            Debug.WriteLine("BlockVal = " + dataInterface.BlockValue[0]);
            Debug.WriteLine("NewTrace = " + dataInterface.NewTrace[0]);
            Debug.WriteLine("NewTrace length = " + dataInterface.NewTrace.Length);


            if (teamA.IsBusy != true && teamB.IsBusy != true && worker1.IsBusy != true && worker2.IsBusy != true) 
            { 
                teamA.RunWorkerAsync();
                Thread.Sleep(250);
                teamB.RunWorkerAsync();
                worker1.RunWorkerAsync();
                worker2.RunWorkerAsync();
                
                statusLabel.Content = "Running.";
                noteText.Text = "Please see debugging output for evidence of the functionality of Threads 1 and 2.";
                //debugLabel.Content = "test";
                Debug.WriteLine("debug running");
            }
            else Debug.WriteLine("already running");
           
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            teamA.CancelAsync();
            teamB.CancelAsync();
            worker1.CancelAsync();
            worker2.CancelAsync();
            statusLabel.Content = "Stopping.";
            Debug.WriteLine("stopping");
        }

        private static readonly Random random = new Random();
        private static readonly object syncLock = new object(); 

        void teamA_DoWork(object sender, DoWorkEventArgs e) 
        {
            Debug.WriteLine("teamA running");
            while (teamA.CancellationPending == false) 
            {

                if (dataInterface.isNew == false)
                {
                    Thread.Sleep(10);
                    Debug.WriteLine("blockSize = " + blockSize + ", numSumples = " + numSamples);

                                    
                    for (int i = 0; i < blockSize; i++)
                    {
                      
                        dataInterface.samples[i] = (ushort)(random.Next(0, 65535));   //up to unsigned short max value
                    }
                   
                    dataInterface.BlockValue = dataInterface.samples;
                    //debugLabel.Content = "test";
                    dataInterface.isNew = true;
                    Debug.WriteLine("BlockValue [0-2] = " + dataInterface.BlockValue[0] + dataInterface.BlockValue[1] + dataInterface.BlockValue[2]);
                    Debug.WriteLine("A-isNew = " + dataInterface.isNew);
                    teamA.ReportProgress(250);

                }
            }

            
            {
                e.Cancel = true;
                return;
            
            }
        }

        string timeNow;

        void teamB_DoWork(object sender, DoWorkEventArgs e)
        {
          
            while (teamB.CancellationPending == false) 
            {

                if (dataInterface.isNew == true)
                {

                    if (dataInterface.isNew == true)
                    {
                        // date --------------------------------------------------------------------------------
                       
                        timeNow = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss.ff tt");
                        Debug.WriteLine("timeNow = " + timeNow);

                        timeList.Add(timeNow);
                        // timesList.Add(new TimeRecieved() {TimeVal = timeNow});

                        // call UI thread to output into list here


                        // splitting ----------------------------------------------------------------------------
                        Debug.WriteLine("blockSize = " + blockSize + ", numSumples = " + numSamples);

                        for (int iter = 0; iter < (numTraces); iter++)
                        {
                            for (int j = 0; j < numSamples; j++)
                            {

                                dataInterface.NewTrace[j] = dataInterface.BlockValue[(iter * numSamples) + j];
                                // show samples
                                Debug.WriteLine("sample index = " + ((iter * numSamples) + j) + ", sample = " + dataInterface.NewTrace[j]);
                       
                        // list ----------------------------------------------------------------------------------
                                                                
                                
                                //traceList.Add(dataInterface.newTrace);
                                //Debug.WriteLine("newTrace = " + dataInterface.newTrace);

                            }
                            // show traces
                            Debug.WriteLine("trace [0-2] = " + dataInterface.NewTrace[0] + dataInterface.NewTrace[1] + dataInterface.NewTrace[2]);
                            
                        }

                        dataInterface.isNew = false;
                        Debug.WriteLine("B-isNew = " + dataInterface.isNew);
                    }
                }
                
            }
            {
                    e.Cancel = true;
                    return;
                }

              
          
        }


        public class Interface
        {
            public ushort[] samples;    // container to hold datablock sample values during generation
            private ushort[] blockVal;   // datablock as passed from A to B
            private ushort[] newTrace;  // container to hold newest trace when splitting
            public bool isNew;                          // flag for new datablock



            public Interface(int numSamples, int blockSize)
            {
                samples = new ushort[blockSize];    // container to hold datablock sample values during generation
                blockVal = new ushort[blockSize];   // datablock as passed from A to B
                //ushort[] newTrace = new ushort[numSamples];  // container to hold newest trace when splitting
                newTrace = new ushort[numSamples];  // container to hold newest trace when splitting
                bool isNew = false;
            }

            public ushort[] BlockValue
            {
                get { return blockVal; }
                set { blockVal = value; }
            }

            public ushort[] NewTrace
            {
                get { return newTrace; }
                set { newTrace = value; }
            }

        }
        

        public class TraceList                                               // class holder with arrays instead of lists for more efficient indexing and copying
        {
            //public ushort[] traceData = new ushort[numSamples + 1];
            public ushort[] traceData;
            public List<ushort[]> heldTraces = new List<ushort[]>();
            public ushort[] traceToBuffer;
            public int numDeleted = 0;
            public ushort[] newTrace;

            public TraceList()
            {
                traceData = new ushort[numSamples + 1];
            }

            public void AddMyData(ushort[] newTrace)
            {
                traceData[0] = (ushort)numWorkers;
                newTrace.CopyTo(traceData, 1);
                heldTraces.Add(traceData);
            }

            public ushort[] GetData(int indRead)
            {
                //traceToBuffer = heldTraces[indRead];
                Array.Copy(heldTraces[indRead], 1, traceToBuffer, 0, numSamples);

                heldTraces[indRead][0] = (ushort)(heldTraces[indRead][0] - 1);

                if (heldTraces[indRead][0] <= 0)
                {
                    heldTraces.RemoveAt(indRead);
                    numDeleted++;
                }
                return traceToBuffer;
            }
        }

       
    
        void worker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ushort[] buffer1 = new ushort[numSamples];
            int trackDel1 = 0;

            //dummy values
            while (worker1.CancellationPending == false) 
            {

                for (int i = 0; i <= 1000; i=i+100)
                {
                    //Debug.WriteLine("barVal = " + i);
                    worker1.ReportProgress(i);
                    //i = i + 100;
                    Thread.Sleep(100);
                }


                
                //buffer1 = getTrace();  // trace fetched from list populated by Team B
               
                //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                // FURTHER PSEUDO CODE FOR WORKERS (Threads 3 and 4) ++++++++++++++++++++++++++++++++++++++++

                // public int numDeleted = 0
                // class WorkData for the below new variables, initialised to 0 for each worker:

                // if numCalc < 1000

                    // as worker # (1 or 2) copies a trace into buffer,
                        // numCopied++ to track total traces recieved
                        // indRead++ to move on to next trace 
                        // trace's numToCopy-- (number of workers left that have to copy it)
                        // if numToCopy == 0
                            // delete trace
                            // trackDeleted++;
                                      
                        // if trackDeleted < numDeleted;
                            // then a different worker must have deleted the most recent trace
                            // therefore indRead-- to calibrate the trace reader index to the loss of a trace
                            // so as not to skip the next trace

                        // for worker1, calculate trace average
                        // for worker2, calculate trace minima and maxima
                        // numCalc ++;

                //else set numCalc = 0 and repeat from the beginning
                

                // to calculate trace average (float):
                // traceAv = traceInBuffer.Sum() / numSamples;

                // to calculate trace average (ushort):
                // initialise both minVal and maxVal to first sample in trace;
                // iterate through samples
                    // if newSample < nowSample, minVal = newSample
                    // if newSample > nowSample, maxVal = newSample

                // END PSEUDOCODE +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


             }
        }

        void worker2_DoWork(object sender, DoWorkEventArgs e)
        {


            //dummy values
            while (worker2.CancellationPending == false)
            {

                for (int k = 0; k <= 1075; k = k + 75)
                {
                    //Debug.WriteLine("barVal = " + i);
                    worker2.ReportProgress(k);
                   
                    Thread.Sleep(100);
                }
            }
        }


        void getTrace(){
            //copies trace from trace list
        }

        void teamA_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //lblStatus.Text = "Working... (" + e.ProgressPercentage + "%)";
        }

        void teamB_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        void worker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            bar1.Value = e.ProgressPercentage;
        }

        void worker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            bar2.Value = e.ProgressPercentage;
        }

        void teamA_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Cancelled)
            {
                statusLabel.Content = "Cancelled.";
            }
            else
            {
                statusLabel.Content = "A Completed.";
            }
        }

        void teamB_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                statusLabel.Content = "Cancelled.";
            }
            else
            {
                statusLabel.Content = "B Completed.";
            }
        }

        void worker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                statusLabel.Content = "Cancelled.";
            }
            else
            {
                statusLabel.Content = "w1 Completed.";
            }
        }

        void worker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                statusLabel.Content = "Cancelled.";
            }
            else
            {
                //statusLabel.Content = "w2 Completed.";
                statusLabel.Content = "Threads Completed.";
            }
        }

        // possibly useful code segments below ++++++++++++++++++++++++++++++++++++++++++++++++++++++

        //private ObservableCollection<TimeRecieved> uiList = new ObservableCollection<TimeRecieved>();

        //public class TimeRecieved : INotifyPropertyChanged
        //{
        //    private string timeVal;
        //    public string TimeVal
        //    {
        //        get { return this.timeVal; }
        //        set
        //        {
        //            if (this.timeVal != value)
        //            {
        //                this.timeVal = value;
        //                this.NotifyPropertyChanged("Name");
        //            }
        //        }
        //    }
        //    public event PropertyChangedEventHandler PropertyChanged;

        //    public void NotifyPropertyChanged(string propTime)
        //    {
        //        if (this.PropertyChanged != null)
        //            this.PropertyChanged(this, new PropertyChangedEventArgs(propTime));
        //    }
        //}
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    }

}
