using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;

namespace Финансы
{
    public partial class ПоставкиWindow : Window
    {
        private DataTable _skladData; // Хранение данных о складах

        public ПоставкиWindow()
        {
            InitializeComponent();
            LoadSkлады();
            LoadData();
        }

        private void LoadSkлады()
        {
            try
            {
                using (OleDbConnection connection = DatabaseManager.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM Склады";
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
                    _skladData = new DataTable();
                    adapter.Fill(_skladData);

                    // Устанавливаем данные для конвертера
                    SkladConverter.SetSklaidData(_skladData);

                    cbОткуда.ItemsSource = _skladData.DefaultView;
                    cbОткуда.DisplayMemberPath = "Название";
                    cbОткуда.SelectedValuePath = "КодСклада";

                    cbКуда.ItemsSource = _skladData.DefaultView;
                    cbКуда.DisplayMemberPath = "Название";
                    cbКуда.SelectedValuePath = "КодСклада";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке складов: " + ex.Message);
            }
        }

        private void LoadData()
        {
            try
            {
                using (OleDbConnection connection = DatabaseManager.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM Поставки";
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Добавляем вычисляемый номер
                    dt.Columns.Add("Номер", typeof(int));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["Номер"] = i + 1;
                    }

                    dgПоставки.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void AddПоставка_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbОткуда.SelectedValue == null || cbКуда.SelectedValue == null || dpДата.SelectedDate == null)
                {
                    MessageBox.Show("Заполните все поля!");
                    return;
                }

                DateTime chosenDate = dpДата.SelectedDate.Value.Date;

                using (OleDbConnection connection = DatabaseManager.GetConnection())
                {
                    connection.Open();

                    string query = "INSERT INTO Поставки ([Дата], [Откуда], [Куда]) VALUES (?, ?, ?)";

                    using (OleDbCommand cmd = new OleDbCommand(query, connection))
                    {
                        cmd.Parameters.Add("Дата", OleDbType.Date).Value = chosenDate;
                        cmd.Parameters.Add("Откуда", OleDbType.Integer).Value = Convert.ToInt32(cbОткуда.SelectedValue);
                        cmd.Parameters.Add("Куда", OleDbType.Integer).Value = Convert.ToInt32(cbКуда.SelectedValue);

                        cmd.ExecuteNonQuery();
                    }
                }

                LoadData();
                MessageBox.Show("Поставка успешно добавлена");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении поставки: " + ex.Message);
            }
        }

        private void DeletePostavka_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, что в гриде выбрана строка с корректным КодПоставки
            if (dgПоставки.SelectedItem is not DataRowView rowView
                || rowView["КодПоставки"] == DBNull.Value)
            {
                MessageBox.Show("Выберите корректную поставку для удаления");
                return;
            }

            int id = Convert.ToInt32(rowView["КодПоставки"]);

            // Спрашиваем подтверждение
            var confirm = MessageBox.Show(
                $"Вы действительно хотите безвозвратно удалить поставку #{id}?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            try
            {
                using var connection = DatabaseManager.GetConnection();
                connection.Open();

                using var transaction = connection.BeginTransaction();

                // 1) Удаляем записи из СоставПоставки
                using (var cmdDeleteSostav = new OleDbCommand(
                    "DELETE FROM СоставПоставки WHERE КодПоставки = ?", connection, transaction))
                {
                    cmdDeleteSostav.Parameters
                        .Add("КодПоставки", OleDbType.Integer)
                        .Value = id;
                    cmdDeleteSostav.ExecuteNonQuery();
                }

                // 2) Удаляем саму поставку
                int rowsAffected;
                using (var cmdDeletePost = new OleDbCommand(
                    "DELETE FROM Поставки WHERE КодПоставки = ?", connection, transaction))
                {
                    cmdDeletePost.Parameters
                        .Add("КодПоставки", OleDbType.Integer)
                        .Value = id;
                    rowsAffected = cmdDeletePost.ExecuteNonQuery();
                }

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException(
                        $"Поставка с КодПоставки = {id} не найдена в таблице Поставки.");
                }

                transaction.Commit();

                LoadData();

                MessageBox.Show("Поставка успешно удалена");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении поставки: " + ex.Message);
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
