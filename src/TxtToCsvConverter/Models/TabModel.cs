using Library.UseCase;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TxtToCsvConverter.Models;

/// <summary>
/// タブモデル
/// </summary>
internal class TabModel : INotifyPropertyChanged
{
    private string header = string.Empty;
    private ObservableCollection<FieldModel> contents = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="TabModel"/> class.
    /// </summary>
    /// <param name="header">ヘッダー</param>
    /// <param name="cotnents">中身</param>
    public TabModel(string header, ObservableCollection<FieldModel> cotnents)
    {
        this.Header = header;
        this.Contents = cotnents;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TabModel"/> class.
    /// </summary>
    /// <param name="dto">タブモデルのDTO</param>
    public TabModel(TabModelDto dto)
    {
        this.Header = dto.Header;
        foreach (var content in dto.Contents)
        {
            var dt = new FieldModel(content.No, content.Name, content.Digit);
            this.Contents.Add(dt);
        }
    }

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// ヘッダー
    /// </summary>
    public string Header
    {
        get => this.header;
        set
        {
            if (this.header != value)
            {
                this.header = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// 中身
    /// </summary>
    public ObservableCollection<FieldModel> Contents
    {
        get => this.contents;
        set
        {
            if (this.contents != value)
            {
                this.contents = value;
                this.OnPropertyChanged();
            }
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
