using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using Финансы;

namespace Финансы
{
    public partial class ДокументыWindow : Window
    {
        public ДокументыWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using (var connection = DatabaseManager.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM Документы";
                    var adapter = new OleDbDataAdapter(query, connection);
                    var dt = new DataTable();
                    adapter.Fill(dt);

                    dt.Columns.Add("№", typeof(int));
                    for (int i = 0; i < dt.Rows.Count; i++)
                        dt.Rows[i]["№"] = i + 1;

                    dgДокументы.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки документов: " + ex.Message);
            }
        }

        private void AddDocument_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbНомер.Text) || dpДата.SelectedDate == null || string.IsNullOrWhiteSpace(tbСумма.Text))
                {
                    MessageBox.Show("Заполните обязательные поля: Номер, Дата, Сумма");
                    return;
                }

                using (var connection = DatabaseManager.GetConnection())
                {
                    connection.Open();

                    string query = "INSERT INTO Документы (Номер, Дата, Сумма, ТипДокумента, Описание, Статус) VALUES (?, ?, ?, ?, ?, ?)";

                    using var cmd = new OleDbCommand(query, connection);
                    cmd.Parameters.AddWithValue("Номер", tbНомер.Text.Trim());
                    cmd.Parameters.AddWithValue("Дата", dpДата.SelectedDate.Value);
                    cmd.Parameters.AddWithValue("Сумма", Convert.ToDecimal(tbСумма.Text));
                    cmd.Parameters.AddWithValue("ТипДокумента", cbТип.Text);
                    cmd.Parameters.AddWithValue("Описание", tbОписание.Text.Trim());
                    cmd.Parameters.AddWithValue("Статус", cbСтатус.Text);

                    cmd.ExecuteNonQuery();
                }

                LoadData();
                MessageBox.Show("Документ успешно добавлен");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении документа: " + ex.Message);
            }
        }

        private void DeleteDocument_Click(object sender, RoutedEventArgs e)
        {
            if (dgДокументы.SelectedItem is not DataRowView selectedRow ||
                selectedRow["КодДокумента"] == DBNull.Value)
            {
                MessageBox.Show("Выберите документ для удаления");
                return;
            }

            int id = Convert.ToInt32(selectedRow["КодДокумента"]);

            var confirm = MessageBox.Show(
                $"Удалить документ с кодом {id}?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            try
            {
                using var connection = DatabaseManager.GetConnection();
                connection.Open();

                using var cmd = new OleDbCommand(
                    "DELETE FROM Документы WHERE КодДокумента = ?", connection);
                cmd.Parameters.AddWithValue("?", id);
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    MessageBox.Show("Документ не найден или уже удалён.");
                }
                else
                {
                    MessageBox.Show("Документ удалён");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении документа: " + ex.Message);
            }
        }


        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
