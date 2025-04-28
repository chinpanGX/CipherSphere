using System.Collections.Generic;
using System.Linq;
using CipherSphere.Runtime.Core;
using CipherSphere.Runtime.Json;
using CipherSphere.Runtime.Security;

namespace CipherSphere.Tests
{
    internal interface IPlayerRepository
    {
        void Save(TestPlayerData playerData);
        TestPlayerData Load();
        void Delete();
        bool Exists();
    }
    
    internal class TestPlayerRepository : IPlayerRepository
    {
        private readonly string fileName = "TestPlayerData";
        private readonly IDataStorage dataStorage;

        public TestPlayerRepository(bool development)
        {
            if (development)
            {
                dataStorage = new JsonDataStorage(fileName);    
            }
            else
            {
                var appSalt = "7kA2pzQ4!M@v9wF5xLtR&0Yn#cXsOg$Hd3Ub3XpQa!Z5TZxP4t9$wQa!m3oL8#";
                dataStorage = new SecurityDataStorageService(fileName, appSalt, "password1234");
            }
            

            if (!dataStorage.Exists())
            {
                Create();
            }
        }

        private void Create()
        {
            var playerData = TestPlayerData.CreateNew();
            dataStorage.Save(playerData);
        }

        public void Save(TestPlayerData playerData)
        {
            if (playerData == null)
                throw new TestPlayerException("Player data cannot be null.");
            if (string.IsNullOrEmpty(playerData.PlayerId))
                throw new TestPlayerException("Player ID cannot be null or empty.");
            if (string.IsNullOrEmpty(playerData.Name))
                throw new TestPlayerException("Player name cannot be null or empty.");
            dataStorage.Save(playerData);
        }

        public TestPlayerData Load()
        {
            return dataStorage.Load<TestPlayerData>();
        }
        
        public void Delete()
        {
            if (dataStorage.Exists())
            {
                dataStorage.Delete();
            }
        }
        
        public bool Exists() => dataStorage.Exists();
    }
}