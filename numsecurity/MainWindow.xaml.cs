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
        int retarded=0;
        public MainWindow()
        {
            InitializeComponent();
            key = hklm.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\USBSTOR", false);
            enabled = (int) key.GetValue("Start");
            if (key.GetValue("DelayedAutostart")!=null)
            retarded = (int)key.GetValue("DelayedAutostart");
            switch (enabled)
            {
                case 2: if (retarded == 0)
                        cbStato.SelectedIndex = 1;
                    else
                        cbStato.SelectedIndex = 0; break;
                case 3: cbStato.SelectedIndex = 2; break;
                case 4: cbStato.SelectedIndex = 3; break;
            }
        }

        private void OnOk_Clicked(object sender, RoutedEventArgs e)
        {
            retarded = 0;
            switch (cbStato.SelectedIndex)
            {
                case 0:
                    enabled = 2;
                    retarded = 1;
                    break;
                case 1:
                    enabled = 2;
                    break;
                case 2:
                    enabled = 3;
                    break;
                case 3:
                    enabled = 4;
                    break;
            }

            try
            {
                key = hklm.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\USBSTOR", true);
                key.SetValue("Start", enabled);
                key.SetValue("DelayedAutostart", retarded);
            }
            catch (Exception ex)
            {
                txtRisultato.Text = ex.Message;
                txtRisultato.Foreground = Brushes.Red;
                return;
            }
            txtRisultato.Text = "Riavvia il pc";
            txtRisultato.Foreground = Brushes.Yellow;
        }
    }
}
