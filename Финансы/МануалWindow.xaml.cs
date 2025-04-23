using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Финансы
{
    public partial class МануалWindow : Window
    {
        private DataTable склады;
        private DataTable товары;

        private string connectionString =
            "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Финансы.accdb";

        public МануалWindow()
        {
            InitializeComponent();
            ЗагрузитьСклады();
            ЗагрузитьТовары();
        }

        #region — Склады —

        private void ЗагрузитьСклады()
        {
            using var conn = new OleDbConnection(connectionString);
            var adapter = new OleDbDataAdapter("SELECT * FROM Склады", conn);
            склады = new DataTable();
            adapter.Fill(склады);
            dgСклады.ItemsSource = склады.DefaultView;
        }

        private void ФильтрСкладов()
        {
            var txt = tbПоискСкладов.Text.Replace("'", "''");
            склады.DefaultView.RowFilter = $"Название LIKE '%{txt}%'";
        }

        private void tbПоискСкладов_TextChanged(object sender, TextChangedEventArgs e)
        {
            ФильтрСкладов();
        }

        private void ДобавитьСклад_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateWarehouseInputs(out var name, out var addr))
                return;

            try
            {
                using var conn = new OleDbConnection(connectionString);
                conn.Open();
                using var cmd = new OleDbCommand(
                    "INSERT INTO Склады (Название, Адрес) VALUES (?, ?)", conn);
                cmd.Parameters.AddWithValue("?", name);
                cmd.Parameters.AddWithValue("?", addr);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Склад добавлен.");
                ЗагрузитьСклады();
                tbНазваниеСклада.Clear();
                tbАдресСклада.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении склада: " + ex.Message);
            }
        }

        private void УдалитьСклад_Click(object sender, RoutedEventArgs e)
{
    if (dgСклады.SelectedItem is not DataRowView row)
    {
        MessageBox.Show("Выберите склад для удаления.");
        return;
    }

    var складId = Convert.ToInt32(row["КодСклада"]);
    var складName = row["Название"];

    // 1) Считаем, сколько поставок ссылаются на этот склад
    int relatedCount;
    using (var conn = new OleDbConnection(connectionString))
    {
        conn.Open();
        using var cmdCount = new OleDbCommand(
            "SELECT COUNT(*) FROM Поставки WHERE [Откуда] = ? OR [Куда] = ?", conn);
        cmdCount.Parameters.AddWithValue("?", складId);
        cmdCount.Parameters.AddWithValue("?", складId);
        relatedCount = Convert.ToInt32(cmdCount.ExecuteScalar());
    }

    // Если есть связанные поставки — спрашиваем, удаляем ли их вместе со складом
    if (relatedCount > 0)
    {
        var cascadeConfirm = MessageBox.Show(
            $"Найдено {relatedCount} поставок, связанных со складом «{складName}».\n" +
            $"Удалить вместе с ними этот склад и все связанные данные?",
            "Каскадное удаление",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (cascadeConfirm != MessageBoxResult.Yes)
            return;
    }
    else
    {
        // обычное подтверждение удаления без каскада
        var confirm = MessageBox.Show(
            $"Удалить склад «{складName}»?",
            "Подтверждение удаления",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
        if (confirm != MessageBoxResult.Yes)
            return;
    }

    // 2) Удаляем в транзакции: сначала состав поставки, затем поставки, затем сам склад
    try
    {
        using var conn = new OleDbConnection(connectionString);
        conn.Open();
        using var transaction = conn.BeginTransaction();

        if (relatedCount > 0)
        {
            // а) получаем список КодПоставки, где участвует этот склад
            var shipmentIds = new List<int>();
            using (var cmdList = new OleDbCommand(
                       "SELECT КодПоставки FROM Поставки WHERE [Откуда] = ? OR [Куда] = ?", conn, transaction))
            {
                cmdList.Parameters.AddWithValue("?", складId);
                cmdList.Parameters.AddWithValue("?", складId);
                using var reader = cmdList.ExecuteReader();
                while (reader.Read())
                    shipmentIds.Add(Convert.ToInt32(reader["КодПоставки"]));
            }

            if (shipmentIds.Count > 0)
            {
                // б) удаляем все строки составов для этих поставок
                using var cmdDelDetails = new OleDbCommand(
                    "DELETE FROM СоставПоставки WHERE КодПоставки = ?", conn, transaction);
                cmdDelDetails.Parameters.Add("?", OleDbType.Integer);
                foreach (var sid in shipmentIds)
                {
                    cmdDelDetails.Parameters[0].Value = sid;
                    cmdDelDetails.ExecuteNonQuery();
                }

                // в) удаляем сами поставки
                using var cmdDelShip = new OleDbCommand(
                    "DELETE FROM Поставки WHERE [Откуда] = ? OR [Куда] = ?", conn, transaction);
                cmdDelShip.Parameters.AddWithValue("?", складId);
                cmdDelShip.Parameters.AddWithValue("?", складId);
                cmdDelShip.ExecuteNonQuery();
            }
        }

        // 3) удаляем склад
        using var cmdDelWh = new OleDbCommand(
            "DELETE FROM Склады WHERE КодСклада = ?", conn, transaction);
        cmdDelWh.Parameters.AddWithValue("?", складId);
        cmdDelWh.ExecuteNonQuery();

        transaction.Commit();

        MessageBox.Show("Склад (и связанные данные) успешно удалены.");
        ЗагрузитьСклады();
    }
    catch (Exception ex)
    {
        MessageBox.Show("Ошибка при удалении склада: " + ex.Message);
    }
}


        // Валидация складов
        private bool ValidateWarehouseInputs(out string name, out string address)
        {
            name = tbНазваниеСклада.Text.Trim();
            address = tbАдресСклада.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите название склада.");
                tbНазваниеСклада.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Введите адрес склада.");
                tbАдресСклада.Focus();
                return false;
            }
            return true;
        }

        #endregion

        #region — Товары —

        private void ЗагрузитьТовары()
        {
            using var conn = new OleDbConnection(connectionString);
            var adapter = new OleDbDataAdapter("SELECT * FROM Товары", conn);
            товары = new DataTable();
            adapter.Fill(товары);
            dgТовары.ItemsSource = товары.DefaultView;
        }

        private void ФильтрТоваров()
        {
            var txt = tbПоискТоваров.Text.Replace("'", "''");
            товары.DefaultView.RowFilter =
                $"Название LIKE '%{txt}%' OR ЕдИзм LIKE '%{txt}%' OR CONVERT(Цена, System.String) LIKE '%{txt}%'";
        }

        private void tbПоискТоваров_TextChanged(object sender, TextChangedEventArgs e)
        {
            ФильтрТоваров();
        }

        private void ДобавитьТовар_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateProductInputs(out var name, out var unit, out var price))
                return;

            try
            {
                using var conn = new OleDbConnection(connectionString);
                conn.Open();
                using var cmd = new OleDbCommand(
                    "INSERT INTO Товары (Название, ЕдИзм, Цена) VALUES (?, ?, ?)", conn);
                cmd.Parameters.AddWithValue("?", name);
                cmd.Parameters.AddWithValue("?", unit);
                cmd.Parameters.AddWithValue("?", price);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Товар добавлен.");
                ЗагрузитьТовары();
                tbНазваниеТовара.Clear();
                tbЕдИзм.Clear();
                tbЦена.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении товара: " + ex.Message);
            }
        }

        private void УдалитьТовар_Click(object sender, RoutedEventArgs e)
        {
            if (dgТовары.SelectedItem is not DataRowView row)
            {
                MessageBox.Show("Выберите товар для удаления.");
                return;
            }

            var id = row["КодТовара"];
            var name = row["Название"];
            var conf = MessageBox.Show(
                $"Удалить товар «{name}»?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (conf != MessageBoxResult.Yes) return;

            try
            {
                using var conn = new OleDbConnection(connectionString);
                conn.Open();
                using var cmd = new OleDbCommand(
                    "DELETE FROM Товары WHERE КодТовара = ?", conn);
                cmd.Parameters.AddWithValue("?", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Товар удалён.");
                ЗагрузитьТовары();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении товара: " + ex.Message);
            }
        }

        private void tbЦена_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ДобавитьТовар_Click(sender, e);
        }

        // Валидация товаров
        private bool ValidateProductInputs(out string name, out string unit, out decimal price)
        {
            name = tbНазваниеТовара.Text.Trim();
            unit = tbЕдИзм.Text.Trim();
            price = 0;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите название товара.");
                tbНазваниеТовара.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(unit))
            {
                MessageBox.Show("Введите единицу измерения.");
                tbЕдИзм.Focus();
                return false;
            }
            if (!decimal.TryParse(tbЦена.Text.Trim(), out price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену (> 0).");
                tbЦена.Focus();
                return false;
            }
            return true;
        }

        #endregion
    }
}
