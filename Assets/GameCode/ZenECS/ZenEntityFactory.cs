using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace TwoStickPureExample.ZenECS {
	public static class ZenEntityFactory {
		
		static ZenEntityFactory() {
			EM = World.Active.GetOrCreateManager<EntityManager>();
		}
		
		public static Dictionary<GameObject, EntityArchetype> ArchetypeCache = new Dictionary<GameObject,EntityArchetype>();
		
		public static EntityManager EM;

		public static Entity CreateLinkedEntity(GameObject Prefab, params ComponentType[] types) {
			EntityArchetype EA;
			bool cachehas = ArchetypeCache.TryGetValue(Prefab, out EA);
			if (!cachehas) {
				EA = EM.CreateArchetype(types);
				ArchetypeCache.Add(Prefab, EA);
			}

			return CreateLinkedEntity_Internal(Prefab, EA);
		}
		
		public static Entity CreateLinkedEntity(GameObject Prefab, EntityArchetype EA) {
			bool cachehas = ArchetypeCache.ContainsKey(Prefab);
			if (cachehas) {
				ArchetypeCache[Prefab] = EA;
			}

			return CreateLinkedEntity_Internal(Prefab, EA);
		}

		private static Entity CreateLinkedEntity_Internal(GameObject Prefab, EntityArchetype EA) {
			Entity newEntity = EM.CreateEntity(EA);

			GameObject newObject = Object.Instantiate(Prefab);
			
			GameObjectComp goComp = EM.GetComponentData<GameObjectComp>(newEntity);
			goComp.entityID = newEntity.Index;
			ZenComponentReferences.GameObjectCache.Add(newEntity.Index, newObject);
			EM.SetComponentData(newEntity, goComp);
			return newEntity;
		}
	}
}