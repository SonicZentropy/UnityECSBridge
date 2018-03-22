using TwoStickPureExample.ZenECS;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Transforms2D;

namespace TwoStickPureExample
{
    using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Collections;
using Unity.Transforms2D;
 

    public sealed class TwoStickBootstrap
    {
        public static EntityArchetype PlayerArchetype;
        public static EntityArchetype BasicEnemyArchetype;
        public static EntityArchetype ShotSpawnArchetype;

        //public static MeshInstanceRenderer PlayerLook;
        public static PrefabSharedData PlayerPrefab;
        public static MeshInstanceRenderer PlayerShotLook;
        public static MeshInstanceRenderer EnemyShotLook;
        public static MeshInstanceRenderer EnemyLook;

        public static TwoStickSettings Settings;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            // This method creates archetypes for entities we will spawn frequently in this game.
            // Archetypes are optional but can speed up entity spawning substantially.

            var entityManager = World.Active.GetOrCreateManager<EntityManager>();
            //var entityManager = new ZenEntityManager();

            // Create player archetype
            PlayerArchetype = entityManager.CreateArchetype(
                typeof(Position2D), typeof(Heading2D), typeof(PlayerInput),
                typeof(Health), typeof(TransformMatrix), typeof(GameObjectComp), typeof(Rigidbody2DComp));

            // Create an archetype for "shot spawn request" entities
            ShotSpawnArchetype = entityManager.CreateArchetype(typeof(ShotSpawnData));

            // Create an archetype for basic enemies.
            BasicEnemyArchetype = entityManager.CreateArchetype(
                typeof(Enemy), typeof(Health), typeof(EnemyShootState),
                typeof(Position2D), typeof(Heading2D),
                typeof(TransformMatrix), typeof(MoveSpeed), typeof(MoveForward), typeof(GameObjectComp));
        }

        // Begin a new game.
        public static void NewGame()
        {
            // Access the ECS entity manager
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            // Create an entity based on the player archetype. It will get default-constructed
            // defaults for all the component types we listed.
            //Entity player = entityManager.CreateEntity(PlayerArchetype);
            var settingsGO = GameObject.Find("Settings");
            Settings = settingsGO?.GetComponent<TwoStickSettings>();
            if (!Settings)
                Debug.Log("Settings fucked up in NewGame");
            Entity player = ZenEntityFactory.CreateLinkedEntity(Settings.PlayerPrefab, PlayerArchetype);

            // We can tweak a few components to make more sense like this.
            entityManager.SetComponentData(player, new Position2D {Value = new float2(0.0f, 0.0f)});
            entityManager.SetComponentData(player, new Heading2D  {Value = new float2(0.0f, 1.0f)});
            entityManager.SetComponentData(player, new Health { Value = Settings.playerInitialHealth });
            
            entityManager.AddSharedComponentData(player, PlayerPrefab);

            // Finally we add a shared component which dictates the rendered look
            //entityManager.AddSharedComponentData(player, PlayerLook);
            
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitializeWithScene()
        {
            var settingsGO = GameObject.Find("Settings");
            Settings = settingsGO?.GetComponent<TwoStickSettings>();
            if (!Settings)
                return;

            //PlayerLook = GetLookFromPrototype("PlayerRenderPrototype");
            PlayerShotLook = GetLookFromPrototype("PlayerShotRenderPrototype");
            EnemyShotLook = GetLookFromPrototype("EnemyShotRenderPrototype");
            //EnemyLook = GetLookFromPrototype("EnemyRenderPrototype");

            var prefab = GetPrefab("Prefabs/PlayerPrefab");
            //PlayerPrefab = GetPrefab("Prefabs/PlayerPrefab");

            EnemySpawnSystem.SetupComponentData(World.Active.GetOrCreateManager<EntityManager>());

            World.Active.GetOrCreateManager<UpdatePlayerHUD>().SetupGameObjects();
        }

        private static GameObject GetPrefab(string PrefabName) {
            //GameObject instance = Instantiate(Resources.Load("enemy", typeof(GameObject))) as GameObject;
            //return Resources.Load()
            return new GameObject();
        }

        private static MeshInstanceRenderer GetLookFromPrototype(string protoName)
        {
            var proto = GameObject.Find(protoName);
            var result = proto.GetComponent<MeshInstanceRendererComponent>().Value;
            Object.Destroy(proto);
            return result;
        }
    }
}
