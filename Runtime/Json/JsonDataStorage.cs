using System.IO;
using System.Text;
using CipherSphere.Runtime.Core;
using Newtonsoft.Json;

namespace CipherSphere.Runtime.Json
{
    public class JsonDataStorage : IDataStorage
    {
        private readonly string dataFullPath;
        
        public JsonDataStorage(string fileName)
        {
            dataFullPath = $"{DataStorageConfig.DataPath}/{fileName}.json";
        }
        
        public bool Exists() => FileHelper.Exists(dataFullPath);
        
        public void Save<T>(T data)
        {
            FileHelper.CreateDirectoryIfNeed(dataFullPath);
            
            var jsonData = JsonConvert.SerializeObject(data);
            File.WriteAllText(dataFullPath, jsonData, Encoding.UTF8);
        }
        
        public T Load<T>()
        {
            if (!FileHelper.Exists(dataFullPath))
                throw new FileNotFoundException("File not found.", dataFullPath);
            
            var jsonData = File.ReadAllText(dataFullPath, Encoding.UTF8);
            var data = JsonConvert.DeserializeObject<T>(jsonData);
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