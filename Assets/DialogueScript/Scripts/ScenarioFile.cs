using UnityEngine;
using System.IO;
using System.Text;

namespace DialogueScript
{
    public class ScenarioFile
    {
        // シナリオファイルの中身です。
        public readonly string scenario = "";

        // シナリオファイルの拡張子の情報です。
        public enum Format
        {
            txt,
            csv,
        }
        // 現在のシナリオファイルの拡張子です。
        public Format ScenarioFormat { get; private set; } = Format.txt;

        // ファイルネームから自動的に拡張子を判別し中身をscenarioに入れます。
        public ScenarioFile(string scenarioFileName)
        {
            if (string.IsNullOrEmpty(scenarioFileName))
            {
                throw new System.Exception("シナリオファイルのファイル名を指定してください。");
            }

            // ファイルパスを作成します。
            string scenarioFilePath = Path.Combine(Application.streamingAssetsPath, scenarioFileName);

            if (!File.Exists(scenarioFilePath))
            {
                throw new System.Exception("指定されたファイルが見つかりませんでした。\nファイル名 : " + scenarioFileName + "\nファイルパス : " + scenarioFilePath);
            }

            // ファイルネームから拡張子を判別して設定します。
            string extension = Path.GetExtension(scenarioFileName);
            switch (extension)
            {
                case ".txt":
                    ScenarioFormat = Format.txt;
                    break;
                case ".csv":
                    ScenarioFormat = Format.csv;
                    break;
                default:
                    throw new System.Exception("シナリオファイルは拡張子が.txtか.csvのものを使用してください。");
            }

            // シナリオファイルの中身をバイナリデータとして読み込みます。
            byte[] scenarioFileBytes = File.ReadAllBytes(scenarioFilePath);
            using (MemoryStream stream = new MemoryStream(scenarioFileBytes))
            {
                // ファイルのバイト数からエンコーディングを判別します。
                Encoding encoding;
                using (StreamReader reader = new StreamReader(stream, detectEncodingFromByteOrderMarks: true))
                {
                    // StreamReaderのCurrentEncodingプロパティでエンコーディングを取得
                    encoding = reader.CurrentEncoding;
                }

                // バイナリデータをエンコーディングでデコードし、文字列として格納します。
                this.scenario = encoding.GetString(scenarioFileBytes);
            }
        }
    }
}