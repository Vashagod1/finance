using System.Windows;

namespace Финансы
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenPostavki_Click(object sender, RoutedEventArgs e)
        {
            var window = new ПоставкиWindow();
            window.Show();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void OpenManual_Click(object sender, RoutedEventArgs e)
        {
            var окно = new МануалWindow();
            окно.Show();
        }
        private void OpenDocuments_Click(object sender, RoutedEventArgs e)
        {
            var window = new ДокументыWindow();
            window.Show(); 
        }


    }
}