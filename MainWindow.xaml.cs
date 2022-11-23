using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;



namespace Youtube_sln
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string path = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();
        public MainWindow()
        {
            InitializeComponent();
        }
        string parameters = "";
        string fileName = "youtube-dl.exe";
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
            parameters = "-F " + txt_box.Text;
            string paramaters2 = "--skip-download --get-id --get-title --get-thumbnail --get-duration --get-filename " + txt_box.Text;
            
            ProcessStartInfo procInfoForm = ProcInfoo(fileName, paramaters2);

            using Process procs = Process.Start(procInfoForm);
            {
                procs.Start();
                procs.WaitForExit();
                combo_box.Items.Clear();
                int sg = 0;


                while (!procs.StandardOutput.EndOfStream)
                {
                    string result = procs.StandardOutput.ReadLine();
                    string name = "Name: ";


                    if (sg == 0)
                    {
                        if (result.Length > 50)
                        {
                            name += result.Substring(0, 50);
                            name += ("\n" + result.Substring(50));
                        }
                        else name += result;
                        lbl_name.Content = name;
                    }
                    if (sg == 1)
                    {

                        lbl_name.Content += "\nId: " + result;
                    }
                    if (sg == 2)
                    {
                        BitmapImage nbmp = new BitmapImage();
                        nbmp.BeginInit();
                        nbmp.UriSource = new Uri(result, UriKind.Absolute);
                        nbmp.EndInit();
                        image.Source = nbmp;
                    }
                    if (sg == 3)
                    lbl_name.Content += ("\nFile:" + result);
                    if (sg++ == 4)
                    lbl_name.Content += ("\nTime: " + result);



                }
            }





            ProcessStartInfo procInfo = ProcInfoo(fileName, parameters);

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

                // file info form portion 


                


            }


        }

        private void btn_dowloand(object sender, RoutedEventArgs e)
        { 

            if (combo_box.SelectedItem == null) return;
            string selectedFormat = combo_box.SelectedItem.ToString();

            string code = selectedFormat.Split(' ').ToArray()[0].ToString();

            
            fileName = "youtube-dl.exe";
            parameters = $"-o {path}\\%(title)s.%(ext)s -f {code} {SelectedUrl}";

            ProcessStartInfo proc = ProcInfoo(fileName, parameters);
            using (Process procs = Process.Start(proc))
            {

                procs.WaitForExit();
                list_box.Items.Add(txt_box.Text);
                MessageBox.Show("Dowloand Okey");
            }
           
        }

        private void place_select(object sender, RoutedEventArgs e)
        {

            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = path;
            dialog.IsFolderPicker = true;
            if(dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                path = dialog.FileName;
                path_textbox.Text = path;
            }

        }
    }

   

}
