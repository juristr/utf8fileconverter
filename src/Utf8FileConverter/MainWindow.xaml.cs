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

namespace Utf8FileConverter
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
                bytes = bytes.Skip(3).ToArray<byte>();
            }

            File.WriteAllText(filepath, utf8Encoding.GetString(bytes), utf8Encoding);
        }

        private bool StartsWithUTF8Bom(byte[] bytes)
        {
            byte[] utf8BOM = Encoding.UTF8.GetPreamble();
            return bytes[0] == utf8BOM[0] && bytes[1] == utf8BOM[1] && bytes[2] == utf8BOM[2];
        }
    }
}
