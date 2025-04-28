using System;
using Newtonsoft.Json;

namespace CipherSphere.Tests
{
    [JsonObject]
    internal class TestPlayerData
    {
        [JsonProperty] public string PlayerId { get; private set; }
        
        [JsonProperty] public string Name { get; private set; }
        
        public static TestPlayerData CreateNew()
        {
            var id = Guid.NewGuid().ToString();
            var name = "Player";
            return new TestPlayerData(id, name);
        }
        
        public TestPlayerData(string playerId, string name)
        {
            PlayerId = playerId;
            if (string.IsNullOrEmpty(name))
                throw new TestPlayerException("Player name cannot be null or empty.");
            if (name.Length > 20)
                throw new TestPlayerException("Player name cannot exceed 20 characters.");
            if (name.Contains(" "))
                throw new TestPlayerException("Player name cannot contain spaces.");
            Name = name;
        }
        
        public TestPlayerData UpdateName(string name)
        {
            return new TestPlayerData(PlayerId, name);
        }
    }
    
    internal class TestPlayerException : Exception
    {
        public TestPlayerException(string message) : base(message)
        {
        }
        
        public TestPlayerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}