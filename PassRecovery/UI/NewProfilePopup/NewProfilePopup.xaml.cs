using PassRecovery.BLL;
using PassRecovery.UI.Components;
using System;
using System.Collections.Generic;
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

namespace PassRecovery.UI.NewProfilePopup
{
    /// <summary>
    /// Interaction logic for NewProfilePopup.xaml
    /// </summary>
    public partial class NewProfilePopup : Popup
    {
        private readonly NewProfileViewModel viewModel;
        private Profile profile;
        public Profile Profile { get { return profile; } }
        public NewProfilePopup(NewProfileViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            DataContext = viewModel;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            profile = viewModel.Add();
            Close();
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            viewModel.OpenFileDialog();
        }

    }
}
