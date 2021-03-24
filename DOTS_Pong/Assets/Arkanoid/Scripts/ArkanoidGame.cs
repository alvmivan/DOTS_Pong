using System;
using Arkanoid.Scripts.DataComponents;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Arkanoid.Scripts
{
    public class ArkanoidGame : MonoBehaviour
    {
        public static ArkanoidGame Main;

        public EntityManager Manager;

        public float blocksScreenAmount = 0.5f;
        public Camera cam;
        public int blocksAmountX = 5;
        public int blocksAmountY = 5;
        public GameObject blockPrefab;
        public GameObject ballPrefab;

        private Entity blockEntityPrefab;
        private EntityManager manager;

        public float ballSpeed = 1;


        private int currentAliveBlocks = 0;

        private Entity ball;

        private void Awake()
        {
            if (Main != null && Main != this)
            {
                Destroy(gameObject);
                return;
            }

            Main = this;

            manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            blockEntityPrefab = blockPrefab.ToEntity();


            currentAliveBlocks = 0;

            SpawnBlocks();
            CreateBall();
            SpawnBall();
        }

        private void SpawnBall()
        {
            Manager.SetComponentData(ball, new Translation
            {
                Value = cam.ViewportToWorldPoint(new Vector2(0.5f, (1 - blocksScreenAmount) * 0.5f)).ToFloat3()
            });

            Manager.SetComponentData(ball, new PhysicsVelocity
            {
                Linear = new float3(0, ballSpeed, 0),
            });
        }

        private void CreateBall()
        {
            var ballEntityPrefab = ballPrefab.ToEntity();
            ball = Manager.Instantiate(ballEntityPrefab);
        }

        /// <summary>
        /// seguir viendo : https://youtu.be/68sUTX7rQiA?t=1028
        /// </summary>
        public void KillOneBlock()
        {
            currentAliveBlocks--;
            if (currentAliveBlocks <= 0)
            {
                Win();
            }
        }

        private void Win()
        {
            SpawnBlocks();
            SpawnBall();
        }


        private void SpawnBlocks()
        {
            for (int x = 0; x < blocksAmountX; x++)
            for (int y = 0; y < blocksAmountY; y++)
            {
                Spawn(x, y);
            }
        }

        private void Spawn(int xPos, int yPos)
        {
            var x = Mathf.InverseLerp(0, blocksAmountX - 1, xPos);
            var y = Mathf.InverseLerp(0, blocksAmountY - 1, yPos);
            var position = cam.ViewportToWorldPoint(new Vector3(x, y, 0));

            var block = manager.Instantiate(blockEntityPrefab);

            var translation = manager.GetComponentData<Translation>(block);
            translation.Value = position.ToFloat3();
            manager.SetComponentData(block, translation);


            var blockData = new BlockData {Health = yPos};
            manager.SetComponentData(block, blockData);


            currentAliveBlocks++;
        }
    }
}