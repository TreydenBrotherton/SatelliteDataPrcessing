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
            // anonymous type, gridview in listview
;          ReadData readData = new ReadData();
            int maxDataSize = 400;

            for(int i = 0; i < maxDataSize; i++)
            {
                sensorA.AddFirst(readData.SensorA(10, 50));
            }
           
 
        }

        private void ShowSensorData()
        {
            lvSensorData.Items.Clear();
            
            for(int i = 0;i < sensorA.Count;i++)
            {
                
            }
        }
        private void DisplayListBoxData()
        {
            
        }

        // Buttons
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            ShowSensorData();
        }
    }
}
