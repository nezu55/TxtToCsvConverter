using System.Runtime.Serialization;

namespace Library.UseCase;

/// <summary>
/// フィールドモデルのDTO
/// </summary>
[DataContract]
public class FieldModelDto
{
    /// <summary>
    /// 番号
    /// </summary>
    [DataMember]
    public int No { get; set; }

    /// <summary>
    /// フィールド名
    /// </summary>
    [DataMember]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 桁数
    /// </summary>
    [DataMember]
    public int Digit { get; set; }
}
