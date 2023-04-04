using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
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
using Windows.System;
using Windows.UI.Notifications;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            btnok.IsEnabled = false;
            txtRisultato.Text = "Giulio Sorrentino © 2023, some right reserved.This software is under GNU GPL or, in your humble opinion, any later version.https://github.com/numerunix/numsecurity";
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
                if (IsRunAsAdmin())
                {
                    txtRisultato.Text = ex.Message;
                    txtRisultato.Foreground = Brushes.Red;
                    return;
                }
                else
                {
                    try
                    {
                        AdminRelauncher();
                    }
                    catch (Exception exe)
                    {
                        txtRisultato.Text = exe.Message;
                        txtRisultato.Foreground = Brushes.Red;
                        return;

                    }
                }
            }
            txtRisultato.Text = "Restart Windows";
            txtRisultato.Foreground = Brushes.Yellow;
            txtRisultato.Background = Brushes.Green;
            btnok.IsEnabled = false;
        }
        private void AdminRelauncher()
        {
            if (!IsRunAsAdmin())
            {
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.UseShellExecute = true;
                proc.WorkingDirectory = Environment.CurrentDirectory;
                proc.FileName = Assembly.GetEntryAssembly().CodeBase.Replace(".dll", ".exe");
                proc.Verb = "runas";
                new ToastContentBuilder().AddArgument("Administrative priviledges").AddText("Application will be relaunched with administrative priviledges.").AddAudio(new Uri("ms-winsoundevent:Notification.Reminder")).Show();
                Process.Start(proc);
                Application.Current.Shutdown();
            }
        }

        private bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void cbStato_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnok.IsEnabled = true;
            txtRisultato.Text = "";
            txtRisultato.Foreground = Brushes.Black;
        }
    }
}
