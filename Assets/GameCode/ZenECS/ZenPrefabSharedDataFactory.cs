using UnityEngine;

namespace TwoStickPureExample.ZenECS {
	public class ZenPrefabSharedDataFactory {
		private static PrefabSharedData _playerPrefabData;
		private static bool _playerPrefabDataInitialized = false;

		public static PrefabSharedData PlayerPrefabData {
			get {
				if (_playerPrefabDataInitialized == false) {
					_playerPrefabData.Prefab = Resources.Load("Prefabs/PlayerPrefab", typeof(GameObject)) as GameObject;
					_playerPrefabDataInitialized = true;
				}

				return _playerPrefabData;
			}
		}
	}
}