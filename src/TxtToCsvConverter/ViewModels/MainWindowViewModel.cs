using Library.UseCase;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using TxtToCsvConverter.Models;

namespace TxtToCsvConverter.ViewModels;

/// <summary>
/// メインウィンドウのビューモデル
/// </summary>
internal class MainWindowViewModel : INotifyPropertyChanged
{
    private const string TempFilePath = @"..\..\..\..\..\temp\input.txt";
    private const string SettingFilePath = @"..\..\..\..\..\Settings\";
    private string srcFilePath = @"..\..\..\..\..\input/sample.txt";
    private string dstFilePath = @"..\..\..\..\..\output";
    private ObservableCollection<TabModel> tabItems = [];
    private int selectedIndex = 0;

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// [ja-JP] ソース元ファイルパス
    /// </summary>
    public string SrcFilePath
    {
        get => this.srcFilePath;
        set
        {
            if (this.srcFilePath != value)
            {
                this.srcFilePath = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// 出力先ファイルパス
    /// </summary>
    public string DstFilePath
    {
        get => this.dstFilePath;
        set
        {
            if (this.dstFilePath != value)
            {
                this.dstFilePath = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// 送信元ファイルのエンコード
    /// </summary>
    public int SelectedEncode { get; set; }

    /// <summary>
    /// タブのインデックス
    /// </summary>
    public int SelectedIndex
    {
        get => this.selectedIndex;
        set
        {
            if (this.selectedIndex != value)
            {
                this.selectedIndex = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// タブ項目
    /// </summary>
    public ObservableCollection<TabModel> TabItems
    {
        get => this.tabItems;
        set
        {
            if (this.tabItems != value)
            {
                this.tabItems = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// タブの追加
    /// </summary>
    public void AddTab()
    {
        this.tabItems.Add(new TabModel("カスタム", [new(1, string.Empty, 0)]));

        if (this.tabItems.Count <= 1)
        {
            this.SelectedIndex = 0;
        }
    }

    /// <summary>
    /// 行の追加
    /// </summary>
    public void AddRow()
    {
        var count = this.tabItems[this.SelectedIndex].Contents.Count();
        this.InsertRowToBelow(this.tabItems[this.SelectedIndex].Contents[count - 1].No);
    }

    /// <summary>
    /// 上に行の挿入
    /// </summary>
    /// <param name="no">選択行の番号</param>
    public void InsertRowToAbobe(int no)
    {
        var index = no - 1;
        this.TabItems[this.SelectedIndex].Contents.Insert(index, new(no, string.Empty, 0));

        this.UpdateNo();
    }

    /// <summary>
    /// 下に行の挿入
    /// </summary>
    /// <param name="no">選択行の番号</param>
    public void InsertRowToBelow(int no)
    {
        var index = no;
        this.TabItems[this.SelectedIndex].Contents.Insert(index, new(no, string.Empty, 0));

        this.UpdateNo();
    }

    /// <summary>
    /// 行の削除
    /// </summary>
    /// <param name="selectedItems">選択行</param>
    public void DeleteRow(List<FieldModel> selectedItems)
    {
        foreach (var selectedItem in selectedItems)
        {
            this.TabItems[this.SelectedIndex].Contents.Remove(selectedItem);
        }

        this.UpdateNo();
    }

    /// <summary>
    /// 登録処理
    /// </summary>
    /// <param name="selectedIndex">選択されたタブのインデックス</param>
    /// <returns>結果</returns>
    public bool OnRegist(int selectedIndex)
    {
        var fileName = SettingFilePath + this.tabItems[selectedIndex].Header + ".xml";

        TabModelDto result = new();

        ObservableCollection<FieldModelDto> contents = [];

        foreach (var content in this.tabItems[selectedIndex].Contents)
        {
            var dto = new FieldModelDto()
            {
                No = content.No,
                Name = content.Name,
                Digit = content.Digit,
            };
            contents.Add(dto);
        }

        result = new TabModelDto()
        {
            Header = this.tabItems[selectedIndex].Header,
            Contents = contents,
        };

        XmlSerializer se = new(typeof(TabModelDto));
        using StreamWriter sw = new(fileName, false, new UTF8Encoding(false));
        se.Serialize(sw, result);

        return true;
    }

    /// <summary>
    /// [ja-JP] タブの削除
    /// </summary>
    public bool DeleteTab()
    {
        var fileName = SettingFilePath + this.tabItems[this.selectedIndex].Header + ".xml";
        int temp = this.SelectedIndex;
        var lastIndex = this.tabItems.Count - 1;

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        // タブの削除
        this.Sort();  // 最後の項目を削除しないとバインドエラーが発生するためソートしています。
        this.TabItems.RemoveAt(lastIndex);

        lastIndex = this.tabItems.Count - 1;
        if (this.tabItems.Count <= 0)
        {
            this.SelectedIndex = 0;
        }
        else
        {
            this.SelectedIndex = Math.Min(temp, lastIndex);
        }

        return true;
    }

    /// <summary>
    /// ロード処理
    /// </summary>
    public void OnLoaded()
    {
        DirectoryInfo directoryInfo = new(SettingFilePath);

        var allFiles = directoryInfo.GetFiles();
        if (allFiles.Length <= 0)
        {
            return;
        }

        foreach (var file in allFiles)
        {
            var fileName = SettingFilePath + file.Name;

            XmlSerializer serializer = new(typeof(TabModelDto));
            using StreamReader sr = new(fileName, new UTF8Encoding(false));

            TabModelDto? result = (TabModelDto?)serializer.Deserialize(sr);

            if (result is not null)
            {
                this.TabItems.Add(new(result));
            }
        }

        this.SelectedIndex = 0;
    }

    /// <summary>
    /// CSVファイルの作成
    /// </summary>
    /// <param name="selectedIndex">タブのインデックス</param>
    /// <returns>結果</returns>
    public string MakeCsvFile(int selectedIndex)
    {
        if (!File.Exists(this.srcFilePath))
        {
            return "ソース元ファイルが存在しません。";
        }

        if (!Directory.Exists(this.dstFilePath))
        {
            return "出力先パスが存在しません。";
        }

        string srcFileName = this.GetSrcFileName();
        string dstFileName = this.GetDstFileName();

        // エンコードが UTF-8 でない場合、UTF-8で保存された一時ファイルを作成する
        if (!this.PrepareTempFile(srcFileName))
        {
            return "一時ファイルの作成に失敗しました。";
        }

        // 出力先ファイルの削除
        if (File.Exists(dstFileName))
        {
            File.Delete(dstFileName);
        }

        // 入力内容を取得し、フィールド名とデータの書き込みを行う
        var keyValuePairs = this.GetFieldDefinitions(selectedIndex);

        if (!this.WriteCsvFile(srcFileName, dstFileName, keyValuePairs))
        {
            this.CleanupTempFile(srcFileName);
            File.Delete(dstFileName);
            return "入力された総桁数とファイルの行の長さが一致しません。";
        }

        // 登録
        this.OnRegist(this.selectedIndex);

        // 一時ファイルの削除
        this.CleanupTempFile(srcFileName);

        return string.Empty;
    }

    /// <summary>
    /// ソート
    /// </summary>
    private void Sort()
    {
        for (int i = this.selectedIndex; i < this.tabItems.Count() - 1; i++)
        {
            var temp = this.tabItems[i];
            this.tabItems[i] = this.tabItems[i + 1];
            this.tabItems[i + 1] = temp;
        }
    }

    private void UpdateNo()
    {
        int newNo = 1;
        foreach (var item in this.tabItems[this.SelectedIndex].Contents)
        {
            item.No = newNo;
            newNo++;
        }
    }

    private string GetSrcFileName()
    {
        return this.SelectedEncode == 0 ? this.srcFilePath : TempFilePath;
    }

    private string GetDstFileName()
    {
        var srcFileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.srcFilePath);

        var dstFileName = this.dstFilePath;
        if (this.dstFilePath.Substring(this.dstFilePath.Length - 1, 1) != @"\")
        {
            dstFileName += @"\" + srcFileNameWithoutExtension + ".csv";
        }
        else
        {
            dstFileName += srcFileNameWithoutExtension + ".csv";
        }

        return dstFileName;
    }

    private bool PrepareTempFile(string srcFileName)
    {
        if (this.SelectedEncode != 1)
        {
            return true; // 一時ファイルの準備は不要
        }

        return this.CreateTempFile(srcFileName);
    }

    private List<KeyValuePair<string, int>> GetFieldDefinitions(int selectedIndex)
    {
        var keyValuePairs = new List<KeyValuePair<string, int>>();
        foreach (var content in this.tabItems[selectedIndex].Contents)
        {
            keyValuePairs.Add(new KeyValuePair<string, int>(content.Name, content.Digit));
        }

        return keyValuePairs;
    }

    private bool WriteCsvFile(string srcFileName, string dstFileName, List<KeyValuePair<string, int>> keyValuePairs)
    {
        // 総桁数
        int totalDigit = keyValuePairs.Sum(kvp => kvp.Value);

        using StreamWriter sw = new(dstFileName, true, new UTF8Encoding(true));

        // フィールド名の書き込み
        if (!keyValuePairs.All(x => x.Key == string.Empty))
        {
            sw.WriteLine(string.Join(",", keyValuePairs.Select(kvp => kvp.Key)));
        }

        using StreamReader sr = new(srcFileName, new UTF8Encoding(false));
        string? line;

        while ((line = sr.ReadLine()) != null)
        {
            if (line == string.Empty)
            {
                continue;
            }

            if (totalDigit != line.Length)
            {
                // 桁数が違う場合
                return false;
            }

            string formattedLine = this.FormatLine(line, keyValuePairs);
            sw.WriteLine(formattedLine);
        }

        return true;
    }

    private string FormatLine(string line, List<KeyValuePair<string, int>> keyValuePairs)
    {
        var newLine = new StringBuilder();
        int startIndex = 0;

        foreach (var kvp in keyValuePairs)
        {
            newLine.Append(line.Substring(startIndex, kvp.Value)).Append(",");
            startIndex += kvp.Value;
        }

        // 最後の ","を取り除く
        return newLine.ToString(0, newLine.Length - 1);
    }

    private void CleanupTempFile(string srcFileName)
    {
        if (srcFileName != this.srcFilePath)
        {
            File.Delete(srcFileName);
        }
    }

    private bool CreateTempFile(string srcFileName)
    {
        try
        {
            EncodingProvider provider = CodePagesEncodingProvider.Instance;
            var encoding = provider.GetEncoding("shift-jis");
            if (encoding is null)
            {
                return false;
            }

            string fileContent;
            using (StreamReader sr = new(this.srcFilePath, encoding))
            {
                fileContent = sr.ReadToEnd();
            }

            using (StreamWriter sw = new(srcFileName, false, new UTF8Encoding(false)))
            {
                sw.Write(fileContent);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
