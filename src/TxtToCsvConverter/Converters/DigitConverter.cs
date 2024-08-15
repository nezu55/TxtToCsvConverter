using System.Globalization;
using System.Windows.Data;

namespace TxtToCsvConverter.Converters;

/// <summary>
/// 数値コンバーター
/// </summary>
public class DigitConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // 入力値が0以上の数値かどうかを確認
        if (int.TryParse(value.ToString(), out int result) && result >= 0)
        {
            return result;
        }
        else
        {
            // 数値に変換できない場合は0を返す
            return 0;
        }
    }
}
