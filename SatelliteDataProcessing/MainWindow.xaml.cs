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
       
        private int NumberOfNodes(LinkedList<double> sensor)
        {
            return sensor.Count;
        }

        private void DisplayListBoxData(LinkedList<double> sensor, ListBox listBoxName)
        {

            foreach(var data in sensor)
            {
                listBoxName.Items.Add(data);
            }
          
        }

        private bool SelectionSort(LinkedList<double> sensor)
        {
            // need to fix this 
            int min = 0;
            int max = NumberOfNodes(sensor);
            for (int i = 0; min < max; max--)
            {
                min = i;
                for (int j = i; j < max; max++)
                {
                    if (sensor.ElementAt(j) < sensor.ElementAt(min))
                    {
                        min = j;
                    }
                }

                LinkedListNode<double> currentMin = sensor.Find(sensor.ElementAt(min));
                LinkedListNode<double> currentI = sensor.Find(sensor.ElementAt(i));

                var temp = currentMin.Value;
                currentMin.Value = currentI.Value;
                currentI.Value = temp;
                
            }
            return true;
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

        private void btnSensorASelectionSort_Click(object sender, RoutedEventArgs e)
        {
            SelectionSort(sensorA);
        }
    }
}
