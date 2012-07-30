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
    public class MainViewModel
    {
        public ObservableCollection<FileInfo> ResultList { get; set; }

        public MainViewModel()
        {
            var fileInfoList = new ObservableCollection<FileInfo>
            {
                new FileInfo(){ Path = "C:\\test.txt", Encoding = "UTF-8"},
                new FileInfo(){ Path = "C:\\test2.txt", Encoding = "ANSI"}
            };

            ResultList = fileInfoList;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; set; }

        public IEnumerable<FileInfo> ResultList
        {
            get
            {
                return ViewModel.ResultList;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MainViewModel();
            DataContext = this;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
        }

        private void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var utf8Encoding = new UTF8Encoding(false);
            var filepath = @"C:\projects\applications\IAM\Main\BackOffice\frontend\accounts\models\account.js";

            byte[] bytes = File.ReadAllBytes(filepath);

            if (StartsWithUTF8Bom(bytes))
            {
                bytes = bytes.Skip(3).ToArray<byte>(); //skip the BOM bytes
            }

            File.WriteAllText(filepath, utf8Encoding.GetString(bytes), utf8Encoding);
        }

        private bool StartsWithUTF8Bom(byte[] bytes)
        {
            byte[] utf8BOM = Encoding.UTF8.GetPreamble();
            return bytes[0] == utf8BOM[0] && bytes[1] == utf8BOM[1] && bytes[2] == utf8BOM[2];
        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
