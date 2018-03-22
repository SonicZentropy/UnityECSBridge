using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public struct PrefabSharedData : ISharedComponentData {
	public GameObject Prefab;
}