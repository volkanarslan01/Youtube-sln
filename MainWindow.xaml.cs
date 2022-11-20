
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Youtube_sln
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private string SelectedUrl = "";
        private ProcessStartInfo ProcInfoo (string fileName , string parameters)
        {
            ProcessStartInfo procInfo = new ProcessStartInfo();
            procInfo.FileName = fileName;
            procInfo.Arguments = parameters;
            procInfo.RedirectStandardOutput = true;
            procInfo.UseShellExecute = false;
            procInfo.CreateNoWindow = true;


            return procInfo;


        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            string fileName = "youtube-dl.exe";
            string parameters = "-F " + txt_box.Text;
            ProcessStartInfo procInfo = ProcInfoo(fileName, parameters);


            //Process proc =  Process.Start(procInfo);

            using Process proc = Process.Start(procInfo);
            {

                SelectedUrl = txt_box.Text;
                combo_box.Items.Clear();
                proc.WaitForExit();

                int s = 0;

                while (!proc.StandardOutput.EndOfStream)
                {
                    string line = proc.StandardOutput.ReadLine();
                    if (s++ > 2)
                    combo_box.Items.Add(line);


                }
            }

        }

        private void btn_dowloand(object sender, RoutedEventArgs e)
        {
            if (combo_box.SelectedItem == null) return;
            string selectedFormat = combo_box.SelectedItem.ToString();

            string code = selectedFormat.Split(' ').ToArray()[0].ToString();

            string fileName = "youtube-dl.exe";
            string parameters = $"-f {code} {SelectedUrl}";

            ProcessStartInfo proc = ProcInfoo(fileName, parameters);
            using (Process procs = Process.Start(proc))
            {
                procs.WaitForExit();
                MessageBox.Show("Dowloand Okey");
            }
            



        }

        private void txt_box_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
