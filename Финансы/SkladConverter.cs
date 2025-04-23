using System;
using System.Data;
using System.Globalization;
using System.Windows.Data;

namespace Финансы
{
    public class SkladConverter : IValueConverter
    {
        private static DataTable _skladData;

        // Устанавливаем данные о складах
        public static void SetSklaidData(DataTable skladData)
        {
            _skladData = skladData;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (_skladData == null || value == null)
                return string.Empty;

            var rows = _skladData.Select($"КодСклада = {value}");

            if (rows.Length > 0)
                return rows[0]["Название"];

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}