using System.Runtime.Serialization;
using Unity.Entities;
using UnityEngine;

namespace TwoStickPureExample {
	
	public struct Rigidbody2DComp : IComponentData {
		public int entityID;
	}

	public struct GameObjectComp : IComponentData {
		public int entityID;
	}
}