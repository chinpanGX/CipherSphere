using UnityEditor;
using UnityEngine;

namespace CipherSphere.Editor
{
    internal static class CipherSphereTools
    {
        [MenuItem("Development Tools/CipherSphere/Open Save Data Folder", priority = 1)]
        internal static void OpenExplorer()
        {
            // WindowsEditorの場合は、パスの区切り文字を\\に置き換える
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                var path = $"{Application.persistentDataPath}".Replace("/", "\\");
                System.Diagnostics.Process.Start(path);
            }
            // それ以外の環境（Macとか）は、パスの区切り文字を/のまま
            else
            {
                var path = $"{Application.persistentDataPath}";
                System.Diagnostics.Process.Start(path);
            }
        }
        
        [MenuItem("Development Tools/CipherSphere/Delete Save Data", priority = 100)]
        internal static void DeleteSaveData()
        {
            var path = $"{Application.persistentDataPath}".Replace("/", "\\");
            if (System.IO.Directory.Exists(path))
            {
                if (EditorUtility.DisplayDialog("Confirm", "Are you sure you want to delete the save data?",
                        "Yes", "No"))
                {
                    System.IO.Directory.Delete(path, true);
                    EditorUtility.DisplayDialog("Success", "Save data deleted successfully.", "OK");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Error", $"The {path} could not be found.", "OK");
            }
        }
    }
}