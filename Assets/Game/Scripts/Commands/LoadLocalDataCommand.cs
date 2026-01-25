using Common;
using FPS;
using UnityEngine;
using VContainer;

namespace Commands
{
	public class LoadLocalDataCommand : SyncCommand
	{
		private readonly GameProgress _gameProgress;

		[Inject]
		public LoadLocalDataCommand(GameProgress gameProgress)
		{
			_gameProgress = gameProgress;
		}

		public override void Do()
		{
			//local save
			bool hasSave = PlayerPrefs.HasKey(Constants.ProgressPrefsKey);
			if (hasSave)
			{
				var encodedData = PlayerPrefs.GetString(Constants.ProgressPrefsKey);
				_gameProgress.Deserialize(GZip.Decode(encodedData));
			}
			else
			{
				// _user.SetDefaults(_dtoStorage.GetSingle<UserDTO>());
			}

			Status = CommandStatus.Success;
		}
	}
}