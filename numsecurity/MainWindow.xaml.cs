using Microsoft.Win32;
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

namespace numsecurity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
        RegistryKey key;
        int enabled;
        public MainWindow()
        {
            InitializeComponent();
            key = hklm.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\USBSTOR", true);
            enabled = (int) key.GetValue("Start");
            if (enabled == 3)
                usb.IsChecked = true;
            else
                usb.IsChecked = false;
        }

        private void OnOk_Clicked(object sender, RoutedEventArgs e)
        {
            if (usb.IsChecked == true)
                enabled = 3;
            else
                enabled = 4;
            try
            {
               key.SetValue("Start", enabled);
            }
            catch (Exception ex)
            {
                txtRisultato.Text = ex.Message;
                txtRisultato.Foreground = Brushes.Red;
                return;
            }
            txtRisultato.Text = "Riavviare il computer";
            txtRisultato.Foreground = Brushes.Yellow;
        }
    }
}
