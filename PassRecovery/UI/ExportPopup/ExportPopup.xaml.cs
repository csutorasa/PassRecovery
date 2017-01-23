using PassRecovery.UI.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Shapes;

namespace PassRecovery.UI.ExportPopup
{
    /// <summary>
    /// Interaction logic for ExportPopup.xaml
    /// </summary>
    public partial class ExportPopup : Popup
    {
        private readonly ExportPopupViewModel viewModel;

        public ExportPopup(ExportPopupViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            DataContext = viewModel;
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Export();
            Close();
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            viewModel.OpenFileDialog();
        }

    }
}
