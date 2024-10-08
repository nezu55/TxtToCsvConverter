# テキストファイルをCSVファイルに変換するツール

このツールは、テキストファイル(拡張子は何でも良い)をCSVファイルに変換します。

## サンプル

C:\sample.txtというファイルが存在し、以下の内容が記載されていると仮定します。

```
1サンプル太郎　　　　20000101
2サンプル次郎　　　　20010102
```

このツールを用いてC:\に`sample.csv`というファイルを以下の内容で作成するとします。

```
番号,名前,生年月日
1,サンプル太郎　　　　,20000101
2,サンプル次郎　　　　,20010102
```

## 使用方法

CSVファイルの出力先をC:\とするとき、ツールに以下のように入力します。

- **ソース元ファイル**: `C:\sample.txt`
- **出力先パス**: `C:\`
- **ラベル名**: 任意の文字列

| 番号 | フィールド名 | 桁数 |
| ---- | ------------ | ---- |
| 1    | 番号         | 1    |
| 2    | 名前         | 10   |
| 3    | 生年月日     | 8    |

CSV出力ボタンを押下して、CSVファイル(sample.csv)を出力します。

## その他

新規登録：新たにタブを追加します。<br><br>
更新：入力内容をxmlファイルに保存します。次回起動時に入力したタブ内容を表示します。保存場所はsettingsフォルダです。<br><br>
削除：入力内容をタブごと削除します。xmlファイルに入力内容が保存されている場合、ファイルも削除され、次回起動時に削除された項目が表示されなくなります。<br><br>
行の追加：表の一番下に新たな行を追加します。<br><br>
上に挿入：表の選択されている行の上に新たな行を追加します。右クリックでのコンテキストメニューで選択可能です。<br><br>
下に挿入：表の選択されている行の下に新たな行を追加します。<br><br>
削除(コンテキストメニュー)：選択されている行を削除します。複数行の削除が可能です。

