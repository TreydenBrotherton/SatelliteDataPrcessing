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
            lstboxSensorA.Items.Clear();
            lstboxSensorB.Items.Clear();
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
            listBoxName.Items.Clear();
            foreach(var data in sensor)
            {
                listBoxName.Items.Add(data);
            }
          
        }

        private void SelectionSort(LinkedList<double> sensor, ListBox listBox)
        {

            for (int i = 0; i < NumberOfNodes(sensor) - 1; i++)
            {
                int min = i;

                for (int j = i + 1; j < NumberOfNodes(sensor); j++)
                {
                    if (sensor.ElementAt(j) < sensor.ElementAt(min))
                    {
                        min = j;
                    }
                }
                LinkedListNode<double> currentMin = sensor.Find(sensor.ElementAt(min));
                LinkedListNode<double> currentI = sensor.Find(sensor.ElementAt(i));

                double temp = currentMin.Value;
                currentMin.Value = currentI.Value;
                currentI.Value = temp;
            }

            // Displays sorted list
            DisplayListBoxData(sensor, listBox);
        }

        private void InsertionSort(LinkedList<double> sensor, ListBox listBox)
        {
            for (int i = 0; i < NumberOfNodes(sensor) - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    if (sensor.ElementAt(j - 1) > sensor.ElementAt(j))
                    {
                        LinkedListNode<double> current = sensor.Find(sensor.ElementAt(j));
                        LinkedListNode<double> previous = sensor.Find(sensor.ElementAt(j - 1));

                        sensor.Remove(current);
                        sensor.AddBefore(previous, current.Value);
                    }
                }
            }
            // Display the sorted list
            DisplayListBoxData(sensor, listBox);
        }

        private int BinarySearchIterative(LinkedList<double> sensor, double searchValue)
        {
            int minimum = 0;
            int maximum = sensor.Count - 1;

            while (minimum <= maximum)
            {
                int middle = minimum + (maximum - minimum) / 2;

                var middleNode = GetNodeAt(sensor, middle);

                if (searchValue == middleNode.Value)
                {
                    return middle + 1; // Index is 1-based
                }
                else if (searchValue < middleNode.Value)
                {
                    maximum = middle - 1;
                }
                else
                {
                    minimum = middle + 1;
                }
            }
            return minimum;

            LinkedListNode<double> GetNodeAt(LinkedList<double> sensor, int index)
            {
                if (index < 0 || index >= sensor.Count)
                {
                    throw new ArgumentOutOfRangeException("index out of range");
                }

                LinkedListNode<double> current = sensor.First;
                for (int i = 0; i < index; i++)
                {
                    current = current.Next;

                }
                return current;
            }
        }
        // Buttons
       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            ShowSensorData();
            DisplayListBoxData(sensorA, lstboxSensorA);
            DisplayListBoxData(sensorB, lstboxSensorB);
        }

        private void btnSensorASelectionSort_Click(object sender, RoutedEventArgs e)
        {
            SelectionSort(sensorA, lstboxSensorA);
        }

        private void btnSensorAInsertionSort_Click(object sender, RoutedEventArgs e)
        {
            InsertionSort(sensorA, lstboxSensorA);
        }

        private void btnSensorABinaryI_Click(object sender, RoutedEventArgs e)
        {
            SelectionSort(sensorA, lstboxSensorA);
            double searchValue;
            double.TryParse(txtBoxASearchTarget.Text, out searchValue);
            int position = BinarySearchIterative(sensorA, searchValue);

            if (position > 0 && position <= sensorA.Count)
            {
                MessageBox.Show($"Search value {searchValue}, Position was found {position}");
            }    
           

            
            
        }
    }
}
