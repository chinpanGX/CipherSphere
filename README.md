# CipherSphere
Unityのセーブデータを管理するサービス。

JSON形式および共通鍵暗号方式を用いた暗号化されたバイナリ形式をサポートしています。

# 導入手順
>[!NOTE]
> Newtonsoft Jsonの導入が必須です。

PackageManager > Add package from git URL...から追加
```
https://github.com/chinpanGX/CipherSphere.git
```

実装方法は、Tests以下のソースコードを参考にしてください、

セーブデータの保存先は```DataStorageConfig``で、設定しています。__"PersistentDataPath/Data/"__ 以下がデフォルトです。

変更する場合は、アプリ初期化などで```DataStorageConfig.SetDataPath("任意のパス")```を呼び出してください。

```
namespace CipherSphere.Runtime.Core
{
    public static class DataStorageConfig
    {
        public static string DataPath { get; private set; } = $"{Application.persistentDataPath}/Data";

        /// <summary>
        /// アプリ側でデータパスを上書き設定する場合に使用します。
        /// </summary>
        /// <param name="path"></param>
        public static void SetDataPath(string path)
        {
            DataPath = path;
        }
    }
}
```

暗号化するセーブデータのファイル名は、```CRC32アルゴリズム```を, 利用してファイル名のハッシュ化を行います。
```
public class SecurityDataStorageService : IDataStorage
{
    private readonly string dataFullPath;
    private readonly string password;

    public SecurityDataStorageService(string fileName, string appSalt, string password)
    {
        CryptographyExecutor.Setup(appSalt);
        this.password = password;
        dataFullPath = $"{DataStorageConfig.DataPath}/{Crc32.Compute($"{fileName}_{password}")}.bin";
    }
}
```

# ライセンス
本ソフトウェアは, MITライセンスで公開しています。

https://github.com/chinpanGX/CipherSphere/blob/main/LICENSE
