using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Galileo;

namespace SatelliteDataProcessing
{
  
    public partial class MainWindow : Window
    {
        LinkedList<double> sensorA = new LinkedList<double>();
        LinkedList<double> sensorB = new LinkedList<double>();
        
        public MainWindow()
        {
            InitializeComponent();
        }

        
        // Methods

        // Load Method
        
        private void LoadData()
        {
            
            sensorA.Clear();
            sensorB.Clear();
;           ReadData readData = new ReadData();
            int maxDataSize = 400;

            double sigma = (double)upDownSigma.Value;
            double mu = (double)upDownMu.Value;

            for(int i = 0; i < maxDataSize; i++)
            {
                sensorA.AddFirst(readData.SensorA(sigma, mu));
                sensorB.AddFirst(readData.SensorB(sigma, mu));

            }
           
 
        }

        private void ShowSensorData()
        {
            lvSensorData.Items.Clear();
            
            var dataList = sensorA.Zip(sensorB, (a, b) => new { sensorA = a, sensorB = b }).ToList();

            foreach(var data in dataList)
            {
                lvSensorData.Items.Add(data);
            }
        }
       
        private void NumberOfNodes(LinkedList<double> sensor)
        {
            int nodeAmount = sensor.Count();
        }

        private void DisplayListBoxData(LinkedList<double> sensor, ListBox listBoxName)
        {

            foreach(var data in sensor)
            {
                listBoxName.Items.Add(data);
            }
          
        }
        // Buttons
       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            ShowSensorData();
            DisplayListBoxData(sensorA, lstboxSensorA);
            DisplayListBoxData(sensorB, lstboxSensorB);
            NumberOfNodes(sensorB);
        }
    }
}
