using UnityEngine;

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