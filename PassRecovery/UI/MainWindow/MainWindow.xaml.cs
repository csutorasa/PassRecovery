using PassRecovery.BLL;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PassRecovery.UI.MainWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        MainViewModel viewModel = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var errors = viewModel.ItemsSelected((sender as ListBox).SelectedItems.Cast<Profile>());
            if(errors.Any())
            {
                MessageBox.Show(string.Join(Environment.NewLine, errors.Select(error => error.Message)));
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            viewModel.OpenExportPopup(true);
        }

        private void ExportAll_Click(object sender, RoutedEventArgs e)
        {
            viewModel.OpenExportPopup(false);
        }

        private void AddProfile_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AddProfile();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Created by: https://github.com/csutorasa", "About");
        }
    }
}
