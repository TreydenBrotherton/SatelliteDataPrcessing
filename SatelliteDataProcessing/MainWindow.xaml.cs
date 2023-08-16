using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
            // Clears both sensors and list boxes
            sensorA.Clear();
            sensorB.Clear();
            lstboxSensorA.Items.Clear();
            lstboxSensorB.Items.Clear();

            // intialzation
;           ReadData readData = new ReadData();
            int maxDataSize = 400;

            // get value from the integer updown controls
            double sigma = (double)upDownSigma.Value;
            double mu = (double)upDownMu.Value;

            // Adds data to both sensors
            for(int i = 0; i < maxDataSize; i++)
            {
                sensorA.AddFirst(readData.SensorA(sigma, mu));
                sensorB.AddFirst(readData.SensorB(sigma, mu));

            }
           
        }

        private void ShowSensorData()
        {
            lvSensorData.Items.Clear();
            // Splits data into its section of the listview
            var dataList = sensorA.Zip(sensorB, (a, b) => new { sensorA = a, sensorB = b }).ToList();

            foreach(var data in dataList)
            {
                lvSensorData.Items.Add(data);
            }
        }
       
        // Gets the number of nodes in the sensor
        private int NumberOfNodes(LinkedList<double> sensor)
        {
            return sensor.Count;
        }

        // Displays list box data
        private void DisplayListBoxData(LinkedList<double> sensor, ListBox listBoxName)
        {
            listBoxName.Items.Clear();
            foreach(var data in sensor)
            {
                listBoxName.Items.Add(data);
            }
          
        }

        // Selection Sort Algorithm 
        private bool SelectionSort(LinkedList<double> sensor)
        {
            if (sensor == null)
            {
                return false; // returns false if the sensor is invalid
            }

            if (NumberOfNodes(sensor) <= 1)
            {
                return false; // returns false if sorting is not needed
            }

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

            return true; // indicates that the sort was successful
        }

        // Insertion Sort Algorithm 
        private bool InsertionSort(LinkedList<double> sensor)
        {
            if (sensor == null)
            {
                return false; // returns false if the sensor is invalid
            }

            if (NumberOfNodes(sensor) <= 1)
            {
                return false; // returns false if sorting is not needed
            }
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
            return true; // indicates that the sort was successful
        }

        // Binary Search Iterative Algorithm 
        private int BinarySearchIterative(LinkedList<double> sensor, int searchValue)
        {
            int minimum = 0;
            int maximum = sensor.Count - 1;

            // check if list is sorted
            if (isSorted(sensor) == false)
            {
                MessageBox.Show("Lists are not sorted");
                return -1;
            }
            else
            {
                // performs search
                while (minimum <= maximum - 1)
                {
                    int middle = (minimum + maximum) / 2;
                    if (searchValue == sensor.ElementAt(middle))
                    {
                        return ++middle;
                    }
                    else if (searchValue < sensor.ElementAt(middle))
                    {
                        maximum = middle - 1;
                    }
                    else
                    {
                        minimum = middle + 1;
                    }
                }
                return minimum;
            }
        }



        // Binary Search Recursive Algorithm 
        private int BinarySearchRecursive(LinkedList<double> sensor, int searchValue, int minimum, int maximum)
        {
            if (minimum <= maximum - 1)
            {
                int middle = (minimum + maximum) / 2;

                if (searchValue == sensor.ElementAt(middle))
                {
                    return middle;
                }
                else if (searchValue < sensor.ElementAt(middle))
                {
                    return BinarySearchRecursive(sensor, searchValue, minimum, middle - 1);
                }
                else
                {
                    return BinarySearchRecursive(sensor, searchValue, middle + 1, maximum);
                }
            }
            return minimum;
        }
        // Stop watch method
        private double TimeStopWatch(bool isStarted)
        {
            Stopwatch sw = Stopwatch.StartNew();
            TimeSpan ts = sw.Elapsed;
            double elapsedTime = sw.ElapsedTicks;
            if (isStarted)
            {
                sw.Start();
            }
            else 
            { 
                sw.Stop();
            }
            return elapsedTime;
        }

        // Highlights items for searches
        private void HighlightItemsInListBox(LinkedList<double> sensor, ListBox listBox, int selectedIndex)
        {
            listBox.Items.Clear();

            for (int i = 0; i < sensor.Count; i++)
            {
                
                ListBoxItem item = new ListBoxItem();
                item.Content = sensor.ElementAt(i);

                if (i == selectedIndex || i == selectedIndex - 1 || i == selectedIndex + 1)
                {
                    item.Background = Brushes.LightBlue; // Highlight the items
                }

                listBox.Items.Add(item);
            }
            listBox.ScrollIntoView(listBox.Items[selectedIndex]);
        }

        private bool isSorted(LinkedList<double> sensor)
        {
            if (sensor.Count < 2)
            {
                return true;
            }

            double previousValue = sensor.First.Value;
            foreach(double value in sensor)
            {
                if (value < previousValue)
                {
                    return false;
                }
                previousValue = value;
            }
            return true;
        }

        // Buttons
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            ShowSensorData();
            
        }

        private void btnSensorASelectionSort_Click(object sender, RoutedEventArgs e)
        {
            SelectionSort(sensorA);
            DisplayListBoxData(sensorA, lstboxSensorA);
        }

        private void btnSensorAInsertionSort_Click(object sender, RoutedEventArgs e)
        {
            InsertionSort(sensorA);
            DisplayListBoxData(sensorA, lstboxSensorA);
        }

        private void btnSensorABinaryI_Click(object sender, RoutedEventArgs e)
        {
           
                TimeStopWatch(true); // starts stopwatch

                int searchValue;
                int.TryParse(txtBoxASearchTarget.Text, out searchValue);
                int position = BinarySearchIterative(sensorA, searchValue);

                if (position > 0 && position <= sensorA.Count)
                {
                    TimeStopWatch(false); // stops the stopwatch
                    HighlightItemsInListBox(sensorA, lstboxSensorA, position);
                }
                stopWatchSensorAIterative.Text = $"{TimeStopWatch(false)} ticks"; // displays elapsed time from stopwatch
        }

        private void btnSensorABinaryR_Click(object sender, RoutedEventArgs e)
        {
            TimeStopWatch(true); // starts stop watch

            int searchValue;
            int.TryParse(txtBoxASearchTarget.Text, out searchValue);
            int minimum = 0;
            int maximum = sensorA.Count - 1;
            
            int result = BinarySearchRecursive(sensorA, searchValue, minimum, maximum);
            HighlightItemsInListBox(sensorA, lstboxSensorA, result);

            TimeStopWatch(false); // stops the stopwatch
           
            stopWatchSensorARecursive.Text = $"{TimeStopWatch(false)} ticks"; // displays the ticks for the stopwatch
        }
    }
}
