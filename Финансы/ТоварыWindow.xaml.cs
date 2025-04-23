using System.Data;
using System.Data.OleDb;
using System.Windows;
using Финансы;  // не забудь подключить пространство имён DatabaseManager

namespace Финансы
{
    public partial class ТоварыWindow : Window
    {
        public ТоварыWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (OleDbConnection connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand("SELECT * FROM Товары", connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGrid.ItemsSource = table.DefaultView;
            }
        }
    }
}