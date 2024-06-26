﻿using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Tiles
{
    [AddComponentMenu("Scripts/Tiles/Tiles.Spawner")]
    internal class Spawner : NetworkBehaviour
    {
        [SerializeField]
        private Tile tilePrefab;

        [SerializeField]
        private List<Tile> tiles = new();

        [SerializeField]
        private int tileCount;

        [SerializeField]
        private Vector2 bias;

        public override void OnNetworkSpawn()
        {
            if (IsServer || IsHost)
            {
                CreateTiles();
            }
        }

        public void CreateTiles()
        {
            foreach (Tile tile in tiles)
            {
                tile.GetComponent<NetworkObject>().Despawn();
            }
            tiles.Clear();

            for (int i = 0; i < tileCount; i++)
            {
                Vector3 position =
                    transform.position
                    + new Vector3()
                    {
                        x = Random.Range(-bias.x, bias.x),
                        y = Random.Range(-bias.y, bias.y)
                    };
                Tile instance = Instantiate(tilePrefab, position, Quaternion.identity, transform);
                NetworkObject instanceNetworkObject = instance.GetComponent<NetworkObject>();
                instanceNetworkObject.Spawn();
                tiles.Add(instance);
            }
        }
    }
}
