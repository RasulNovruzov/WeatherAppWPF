using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Weather.ViewModel;

namespace Weather
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels();            
        }

        private void LB_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox != null && listBox.HasItems) listBox.SelectedIndex = 0;
        }

    }
}
