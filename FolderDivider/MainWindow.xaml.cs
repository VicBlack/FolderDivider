using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace FolderDivider
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Topmost = true;
            _Model = new Model();
            this.DataContext = _Model;
        }

        private void On_SetInputPath_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _Model != null;
        }

        private void On_SetOutPath_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _Model != null;
        }

        private void On_Divide_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _Model != null && _Model.Nums>0 && !String.IsNullOrEmpty(_Model.InputPath) && !String.IsNullOrEmpty(_Model.OutPath);
        }

        private void OnSetInputPath_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                _Model.InputPath = folderBrowserDialog.SelectedPath;
            }
        }

        private void OnSetOutPath_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                _Model.OutPath = folderBrowserDialog.SelectedPath;
            }
        }

        private void OnDivide_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _Model.filelist = new List<string>(_Model.TotalNums);
            _Model.GetFilesName(_Model.InputPath);
            int[] array = new int[_Model.TotalNums];
            double progs;
            _Model.GetRandomByGuid(array);
            string outpath1 = _Model.OutPath + @"\1_" + _Model.Percent.ToString();
            string outpath2 = _Model.OutPath + @"\2_" + (100 - _Model.Percent).ToString();
            Directory.CreateDirectory(outpath1);
            Directory.CreateDirectory(outpath2);
            for (int i = 0; i < _Model.Nums; i++)
            {
                File.Copy(_Model.filelist[i].ToString(), outpath1 + @"\" + System.IO.Path.GetFileName(_Model.filelist[i].ToString()), true);
                progs = i * 100.0 / _Model.TotalNums;
                progressbar.Dispatcher.Invoke(new Action<System.Windows.DependencyProperty, object>(progressbar.SetValue), System.Windows.Threading.DispatcherPriority.Background, ProgressBar.ValueProperty, progs);
            }
            for (int i = _Model.Nums; i < _Model.TotalNums; i++)
            {
                File.Copy(_Model.filelist[i].ToString(), outpath2 + @"\" + System.IO.Path.GetFileName(_Model.filelist[i].ToString()), true);
                progs = i * 100.0 / _Model.TotalNums;
                progressbar.Dispatcher.Invoke(new Action<System.Windows.DependencyProperty, object>(progressbar.SetValue), System.Windows.Threading.DispatcherPriority.Background, ProgressBar.ValueProperty, progs);
            }
            progressbar.Dispatcher.Invoke(new Action<System.Windows.DependencyProperty, object>(progressbar.SetValue), System.Windows.Threading.DispatcherPriority.Background, ProgressBar.ValueProperty, 0.0);
            MessageBox.Show("操作完成", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private Model _Model;
    }
}
