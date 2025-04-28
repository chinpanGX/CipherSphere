using System.IO;
using NUnit.Framework;
using UnityEngine;

namespace CipherSphere.Tests
{
    public class TestsDataStorageService
    {
        public void CreatePlayerData()
        {
            var playerData = TestPlayerData.CreateNew();
            Assert.IsNotNull(playerData);
            Assert.IsNotNull(playerData.PlayerId);
            Assert.IsNotNull(playerData.Name);
            Debug.Log($"Player data created successfully: {playerData.Name}");
        }
        
        [TestCase(true)]
        [TestCase(false)]
        public void CreateSaveData(bool development)
        {
            var repository = new TestPlayerRepository(development);
            var playerData = TestPlayerData.CreateNew();
            repository.Save(playerData);

            Assert.AreEqual(repository.Load().PlayerId, playerData.PlayerId);
            
        }
        
        [TestCase(true)]
        [TestCase(false)]
        public void UpdateSaveData(bool development)
        {
            var repository = new TestPlayerRepository(development);
            var playerData = repository.Load();
            var newName = "NewPlayerName";
            var updatedPlayerData = playerData.UpdateName(newName);
            repository.Save(updatedPlayerData);
            
            var loadedPlayerData = repository.Load();
            Assert.AreEqual(newName, loadedPlayerData.Name);
            Debug.Log($"Updated PlayerName: {loadedPlayerData.Name}");
        }
        
        [TestCase(true)]
        [TestCase(false)]
        public void DeleteSaveData(bool development)
        {
            var repository = new TestPlayerRepository(development);
            repository.Delete();
            Assert.IsFalse(repository.Exists());
            Debug.Log("Save data deleted successfully.");
        }
        
        [Test]
        public void PlayerNameExceedsMaxLengthThrowsExceptionOnCreation()
        {
            Assert.Throws<TestPlayerException>(() => new TestPlayerData("123", new string('A', 21)));
        }
        
        [Test]
        public void PlayerNameContainsSpacesThrowsExceptionOnCreation()
        {
            Assert.Throws<TestPlayerException>(() => new TestPlayerData("123", "Player Name"));
        }
        
        [TearDown]
        public void TearDown()
        {
            var repository = new TestPlayerRepository(true);
            repository.Delete();
            
            repository = new TestPlayerRepository(false);
            repository.Delete();
        }
    }
}