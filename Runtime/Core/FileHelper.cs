using System.IO;

namespace CipherSphere.Runtime.Core
{
    public static class FileHelper
    {
        public static void CreateDirectoryIfNeed(string path)
        {
            var directory = Path.GetDirectoryName(path);
            if (directory == null)
                throw new DirectoryNotFoundException($"Directory not found. {path}");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
        
        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        public static void Delete(string path)
        {
            if  (File.Exists(path))  
            {
                File.Delete(path);
            }
            else
            {
                throw new FileNotFoundException("File not found.", path);
            }
        }
    }
}