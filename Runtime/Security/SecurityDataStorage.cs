using System.IO;
using CipherSphere.Runtime.Core;
using Newtonsoft.Json;

namespace CipherSphere.Runtime.Security
{
    public class SecurityDataStorageService : IDataStorage
    {
        private readonly string dataFullPath;
        private readonly string password;

        public SecurityDataStorageService(string fileName, string appSalt, string password)
        {
            CryptographyExecutor.Setup(appSalt);
            this.password = password;
            dataFullPath = $"{DataStorageConfig.DataPath}/{Crc32.Compute(fileName, password)}.bin";
        }

        public bool Exists()
        {
            return FileHelper.Exists(dataFullPath);
        }

        public void Save<T>(T data)
        {
            FileHelper.CreateDirectoryIfNeed(dataFullPath);

            var encrypted = CryptographyExecutor.Encrypt(JsonConvert.SerializeObject(data), password);
            using var fileStream = new FileStream(dataFullPath, FileMode.Create, FileAccess.Write);
            fileStream.Write(encrypted, 0, encrypted.Length);
        }

        public T Load<T>()
        {
            if (!FileHelper.Exists(dataFullPath))
                throw new FileNotFoundException("File not found.", dataFullPath);
            
            using var fileStream = new FileStream(dataFullPath, FileMode.Open, FileAccess.Read);
            var bin = new byte[fileStream.Length];
            _ = fileStream.Read(bin, 0, bin.Length);
            
            var data = JsonConvert.DeserializeObject<T>(CryptographyExecutor.Decrypt(bin, password));
            if (data == null)
                throw new InvalidDataException("Failed to load data.");

            return data;
        }

        public void Delete()
        {
            FileHelper.Delete(dataFullPath);
        }
    }
}
