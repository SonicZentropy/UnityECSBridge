using TwoStickPureExample.ZenECS;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms2D;

namespace TwoStickPureExample
{
    public class TransformSyncSystem : ComponentSystem
    {
        public struct Data
        {
            public int Length;
            public ComponentDataArray<Position2D> Position;
            public ComponentDataArray<GameObjectComp> GOs;
        }

        [Inject] private Data m_Data;

        protected override void OnUpdate()
        {
            var settings = TwoStickBootstrap.Settings;

            float dt = Time.deltaTime;
            for (int index = 0; index < m_Data.Length; ++index)
            {
                var position = m_Data.Position[index].Value;

                GameObject goRef = ZenComponentReferences.GameObjectCache[m_Data.GOs[index].entityID];
                goRef.transform.position = new Vector3(position.x, position.y, goRef.transform.position.z);
                //Debug.Log(goRef.name + " Pos: " + position.x + " , " + position.y);
            }
        }
    }
}
