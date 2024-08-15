using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;
using TxtToCsvConverter.ViewModels;
using System.Windows.Controls;
using TxtToCsvConverter.Models;

namespace TxtToCsvConverter.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private FieldModel? selectedItem;
    private List<FieldModel>? selectedItems;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        this.InitializeComponent();
        this.CommandBindings.Add(new CommandBinding(NewRegistCommand, this.NewRegistCommand_Executed));
        this.CommandBindings.Add(new CommandBinding(MakeCommand, this.MakeCommand_Executed, this.MakeCommand_CanExecute));
        this.CommandBindings.Add(new CommandBinding(UpdateCommand, this.UpdateCommand_Executed, this.UpdateCommand_CanExecute));
        this.CommandBindings.Add(new CommandBinding(DeleteCommand, this.DeleteCommand_Executed, this.DeleteCommand_CanExecute));
        this.CommandBindings.Add(new CommandBinding(OpenSrcFileCommand, this.OpenSrcFileCommand_Executed));
        this.CommandBindings.Add(new CommandBinding(OpenDstFileCommand, this.OpenDstFileCommand_Executed));
        this.CommandBindings.Add(new CommandBinding(AddRowCommand, this.AddCommand_Executed));
        this.CommandBindings.Add(new CommandBinding(InsertAbobeCommand, this.InsertAbobeCommand_Executed, this.InsertCommand_CanExecute));
        this.CommandBindings.Add(new CommandBinding(InsertBelowCommand, this.InsertBelowCommand_Executed, this.InsertCommand_CanExecute));
        this.CommandBindings.Add(new CommandBinding(DeleteRowCommand, this.DeleteRowCommand_Executed, this.DeleteRowCommand_CanExecute));

        this.Loaded += this.MainWindow_Loaded;
    }

    /// <summary>
    /// 新規登録コマンド
    /// </summary>
    public static RoutedCommand NewRegistCommand { get; } = new();

    /// <summary>
    /// CSV出力コマンド
    /// </summary>
    public static RoutedCommand MakeCommand { get; } = new();

    /// <summary>
    /// 更新コマンド
    /// </summary>
    public static RoutedCommand UpdateCommand { get; } = new();

    /// <summary>
    /// 削除コマンド
    /// </summary>
    public static RoutedCommand DeleteCommand { get; } = new();

    /// <summary>
    /// ソース元のファイルダイアログオープンコマンド
    /// </summary>
    public static RoutedCommand OpenSrcFileCommand { get; } = new();

    /// <summary>
    /// 出力先パスのディレクトリオープンダイアログコマンド
    /// </summary>
    public static RoutedCommand OpenDstFileCommand { get; } = new();

    /// <summary>
    /// 追加コマンド
    /// </summary>
    public static RoutedCommand AddRowCommand { get; } = new();

    /// <summary>
    /// 上へ挿入コマンド
    /// </summary>
    public static RoutedCommand InsertAbobeCommand { get; } = new();

    /// <summary>
    /// 下へ挿入コマンド
    /// </summary>
    public static RoutedCommand InsertBelowCommand { get; } = new();

    /// <summary>
    /// 削除コマンド
    /// </summary>
    public static RoutedCommand DeleteRowCommand { get; } = new();

    private void NewRegistCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (this.DataContext is MainWindowViewModel viewModel)
        {
            viewModel.AddTab();
        }
    }

    private void MakeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (this.DataContext is MainWindowViewModel viewModel)
        {
            var result = viewModel.MakeCsvFile(this.tabControl.SelectedIndex);

            if (result == string.Empty)
            {
                MessageBox.Show("ファイルの作成に成功しました。");
            }
            else
            {
                MessageBox.Show($"ファイルの作成に失敗しました。\n({result})");
            }
        }
    }

    private void MakeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        if (this.DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        if (viewModel.TabItems.Count >= 1)
        {
            e.CanExecute = true;
        }
    }

    private void UpdateCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (this.DataContext is MainWindowViewModel viewModel)
        {
            var result = viewModel.OnRegist(viewModel.SelectedIndex);

            if (result)
            {
                MessageBox.Show("更新しました。");
            }
            else
            {
                MessageBox.Show("更新に失敗しました。");
            }
        }
    }

    private void UpdateCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        if (this.DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        if (viewModel.TabItems.Count >= 1)
        {
            e.CanExecute = true;
        }
    }

    private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (this.DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        var ret = MessageBox.Show("対象項目を削除します。よろしいですか。", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (ret is MessageBoxResult.Yes)
        {
            var result = viewModel.DeleteTab();

            if (!result)
            {
                MessageBox.Show("削除に失敗しました。");
            }
        }
    }

    private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        if (this.DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        if (viewModel.TabItems.Count >= 2)
        {
            e.CanExecute = true;
        }
    }

    private void OpenSrcFileCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (this.DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        OpenFileDialog openFileDialog = new()
        {
            // ファイルの種類をフィルターとして設定
            Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
        };

        if (openFileDialog.ShowDialog() == true)
        {
            // 選択されたファイルのパスをテキストボックスに表示
            viewModel.SrcFilePath = openFileDialog.FileName;
        }
    }

    private void OpenDstFileCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (this.DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        OpenFolderDialog openFolderDialog = new();

        if (openFolderDialog.ShowDialog() is true)
        {
            // 選択されたファイルのパスをテキストボックスに表示
            viewModel.DstFilePath = openFolderDialog.FolderName;
        }
    }

    private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (this.DataContext is MainWindowViewModel viewModel)
        {
            viewModel.AddRow();
        }
    }

    private void InsertAbobeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (this.DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        if (this.selectedItem is not null)
        {
            viewModel.InsertRowToAbobe(this.selectedItem.No);
        }
    }

    private void InsertBelowCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (this.DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        if (this.selectedItem is not null)
        {
            viewModel.InsertRowToBelow(this.selectedItem.No);
        }
    }

    private void InsertCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        if (this.selectedItems is null || this.selectedItems.Count > 1)
        {
            return;
        }

        if (this.selectedItem is not null)
        {
            e.CanExecute = true;
        }
    }

    private void DeleteRowCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (this.DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        if (this.selectedItems is not null)
        {
            viewModel.DeleteRow(this.selectedItems);
        }
    }

    private void DeleteRowCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        if (this.selectedItems is not null)
        {
            e.CanExecute = true;
        }
    }

    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not DataGrid dataGrid)
        {
            return;
        }

        if (dataGrid.SelectedItem is FieldModel fieldModel)
        {
            this.selectedItem = fieldModel;
        }

        this.selectedItems = dataGrid.SelectedItems.Cast<FieldModel>().ToList();
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is MainWindowViewModel viewModel)
        {
            viewModel.OnLoaded();
        }
    }

    private void FieldDataGrid_CurrentCellChanged(object sender, EventArgs e)
    {
        if (sender is not DataGrid dataGrid)
        {
            return;
        }

        dataGrid.CommitEdit();
        dataGrid.CommitEdit();
    }
}