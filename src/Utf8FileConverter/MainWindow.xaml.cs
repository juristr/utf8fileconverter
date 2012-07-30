using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using System.Collections.ObjectModel;

namespace Utf8FileConverter
{
    class MainWindowViewModel
    {
        public string SourcePath { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MainWindowViewModel();

            this.DataContext = ViewModel;
        }

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    FolderBrowserDialog dialog = new FolderBrowserDialog();
        //    dialog.ShowDialog();
        //}

        private void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

        private bool StartsWithUTF8Bom(byte[] bytes)
        {
            byte[] utf8BOM = Encoding.UTF8.GetPreamble();
            return bytes[0] == utf8BOM[0] && bytes[1] == utf8BOM[1] && bytes[2] == utf8BOM[2];
        }

        private void buttonConvert_Click(object sender, RoutedEventArgs e)
        {
            var filepath = ViewModel.SourcePath;

            /*
             *  TODOS:
             *      - check whether it is a real path
             *      - If it is a path, then traverse recursively, if a file, just process that file
             */

            var utf8Encoding = new UTF8Encoding(false);

            //for now *.js files only
            var files = Directory.EnumerateFiles(filepath, "*.js", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                try
                {
                    byte[] bytes = File.ReadAllBytes(filepath);


                    if (StartsWithUTF8Bom(bytes))
                    {
                        bytes = bytes.Skip(3).ToArray<byte>(); //skip the BOM bytes
                    }

                    File.WriteAllText(filepath, utf8Encoding.GetString(bytes), utf8Encoding);
                }
                catch (UnauthorizedAccessException ex)
                {
                    System.Windows.MessageBox.Show("Unauthorized to read/write " + file + ". Make sure they're not used within another tool or blocked on your VCS system");
                    break;
                }
            }
        }
    }
}
