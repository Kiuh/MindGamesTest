using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay
{
    [AddComponentMenu("Scripts/Gameplay/Gameplay.TilesMatcher")]
    internal class TilesMatcher : NetworkBehaviour
    {
        [Serializable]
        private struct PlaceZone
        {
            public bool IsBusy;
            public Bounds Bounds;
        }

        [SerializeField]
        private Vector2Int size;

        [SerializeField]
        private Vector2 shift;

        [SerializeField]
        private Vector3 bounds;

        [SerializeField]
        private GameObject placePrefab;

        private List<List<PlaceZone>> placeZones = new();

        public override void OnNetworkSpawn()
        {
            for (int i = 0; i < size.x; i++)
            {
                placeZones.Add(new());
                for (int j = 0; j < size.y; j++)
                {
                    Vector3 position =
                        transform.position + new Vector3(shift.x * i, shift.y * j, 0);
                    _ = Instantiate(placePrefab, position, Quaternion.identity, transform);

                    PlaceZone zone =
                        new() { IsBusy = false, Bounds = new Bounds(position, bounds) };
                    placeZones[i].Add(zone);
                }
            }
        }

        public void TryPlaceTile(Tile tile)
        {
            foreach (List<PlaceZone> list in placeZones)
            {
                foreach (PlaceZone zone in list)
                {
                    if (!zone.IsBusy && zone.Bounds.Contains(tile.transform.position))
                    {
                        tile.transform.position = zone.Bounds.center;
                    }
                }
            }
        }
    }
}
