using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TxtToCsvConverter.Models;

/// <summary>
/// フィールドモデル
/// </summary>
internal class FieldModel : INotifyPropertyChanged
{
    private int no;
    private string name;
    private int digit;

    /// <summary>
    /// Initializes a new instance of the <see cref="FieldModel"/> class.
    /// </summary>
    /// <param name="no">番号</param>
    /// <param name="name">フィールド名</param>
    /// <param name="digit">桁数</param>
    public FieldModel(int no, string name, int digit)
    {
        this.no = no;
        this.name = name;
        this.digit = digit;
    }

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// 番号
    /// </summary>
    public int No
    {
        get => this.no;
        set
        {
            if (this.no != value)
            {
                this.no = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// フィールド名
    /// </summary>
    public string Name
    {
        get => this.name;
        set
        {
            if (this.name != value)
            {
                this.name = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// 桁数
    /// </summary>
    public int Digit
    {
        get => this.digit;
        set
        {
            if (this.digit != value)
            {
                this.digit = value;
                this.OnPropertyChanged();
            }
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
