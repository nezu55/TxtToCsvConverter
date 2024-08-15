using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Library.UseCase;

/// <summary>
/// タブモデルのDTO
/// </summary>
[DataContract]
public class TabModelDto
{
    /// <summary>
    /// ヘッダー
    /// </summary>
    [DataMember]
    public string Header { get; set; } = string.Empty;

    /// <summary>
    /// 中身
    /// </summary>
    [DataMember]
    public ObservableCollection<FieldModelDto> Contents { get; set; } = new();
}
