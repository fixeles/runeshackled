using System;
using System.Collections.Generic;
using Converters;
using Newtonsoft.Json;
using Utils;

namespace Common
{
	public class GameProgress : ISerializable
	{
		public float Playtime;
		[JsonProperty] public readonly Inventory Inventory = new();
		[JsonProperty] public Dictionary<string, TimeSpan> Progress = new();
		[JsonProperty] public string Id { get; private set; }
		[JsonProperty] public int CurrentLevel { get; private set; }
		public bool IsTutorialComplete = true;

		public string Serialize() => JsonConvert.SerializeObject(this, new InventoryConverter());

		public void SetDefaults()
		{
			Id = Guid.NewGuid().ToString();
			CurrentLevel = 1;

			// Inventory.Copy();
		}

		public void Deserialize(string decodedJson)
		{
			var deserializedUser = JsonConvert.DeserializeObject<GameProgress>(decodedJson, new InventoryConverter());
			Id = deserializedUser.Id;
			CurrentLevel = deserializedUser.CurrentLevel;
			Playtime = deserializedUser.Playtime;
			Inventory.Copy(deserializedUser.Inventory);
			deserializedUser.Progress.ReplaceAll(Progress);
		}
	}
}