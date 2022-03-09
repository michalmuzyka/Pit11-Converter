using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Xml;
using System.Xml.Linq;
using Microsoft.Win32;
using Path = System.IO.Path;

namespace Pit11_26_toPit11_27_Converter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ConversionArguments arguments = new ConversionArguments();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadFilesButtonClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Pliki XML|*.xml;|All files (*.*)|*.*";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofd.Multiselect = true;
            var res = ofd.ShowDialog();

            if (res.HasValue && res.Value)
            {
                arguments.Files = ofd.FileNames;
                FileNamesListView.Items.Clear();
                foreach (var path in arguments.Files)
                    FileNamesListView.Items.Add(path);
            }
        }
        private void ChooseDirectoryButtonClicked(object sender, RoutedEventArgs e)
        {
            var ofd = new FolderPicker();
            ofd.InputPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var res = ofd.ShowDialog();

            if (res.HasValue && res.Value)
            {
                arguments.FinalDirectory = ofd.ResultPath;
                DirectoryListView.Items.Clear();
                DirectoryListView.Items.Add(arguments.FinalDirectory);
            }
        }
        private void ConvertButtonClicked(object sender, RoutedEventArgs e)
        {
            if (arguments.Files == null)
                StatusBarInfoTextBlock.Text = "Wybierz pliki!";
            else if (arguments.FinalDirectory == null)
                StatusBarInfoTextBlock.Text = "Wybierz folder docelowy!";
            else
            {
                for (int convertedFiles = 0; convertedFiles < arguments.Files.Length; ++convertedFiles)
                {
                    XDocument document = XDocument.Load(arguments.Files[convertedFiles]);
                    Converter.ConvertDocument(document);

                    string path = Path.Combine(arguments.FinalDirectory, Path.GetFileName(arguments.Files[convertedFiles]));
                    document.Save(path);
                    
                    ConversionProgressBar.Value = 100 * (convertedFiles + 1) / (double)arguments.Files.Length;
                }
            }
        }

    }
}
