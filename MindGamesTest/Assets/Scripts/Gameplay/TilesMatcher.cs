using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay
{
    [AddComponentMenu("Scripts/Gameplay/Gameplay.TilesMatcher")]
    internal class TilesMatcher : NetworkBehaviour
    {
        [Serializable]
        private class PlaceZone
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

        [SerializeField]
        private PatternViewer.Model patternViewer;

        [SerializeField]
        private Score.Model scoreModel;

        [SerializeField]
        private TilesSpawner spawner;

        private List<List<PlaceZone>> placeZones = new();

        public override void OnNetworkSpawn()
        {
            CreatePlaceZones();
            foreach (List<PlaceZone> zones in placeZones)
            {
                foreach (PlaceZone zone in zones)
                {
                    _ = Instantiate(
                        placePrefab,
                        zone.Bounds.center,
                        Quaternion.identity,
                        transform
                    );
                }
            }
        }

        private void CreatePlaceZones()
        {
            placeZones.Clear();
            for (int i = 0; i < size.x; i++)
            {
                placeZones.Add(new());
                for (int j = 0; j < size.y; j++)
                {
                    Vector3 position =
                        transform.position + new Vector3(shift.x * i, shift.y * j, 0);
                    PlaceZone zone =
                        new() { IsBusy = false, Bounds = new Bounds(position, bounds) };
                    placeZones[i].Add(zone);
                }
            }
        }

        public void TryPlaceTile(Tile tile)
        {
            for (int i = 0; i < placeZones.Count; i++)
            {
                for (int j = 0; j < placeZones[i].Count; j++)
                {
                    PlaceZone zone = placeZones[i][j];
                    if (!zone.IsBusy && zone.Bounds.Contains(tile.transform.position))
                    {
                        zone.IsBusy = true;
                        tile.transform.position = zone.Bounds.center;
                        void action()
                        {
                            zone.IsBusy = false;
                            tile.OnTileGrabbed -= action;
                        }
                        tile.OnTileGrabbed += action;
                    }
                }
            }

            List<List<bool>> bools = placeZones
                .Select(x => x.Select(x => x.IsBusy).ToList())
                .ToList();

            if (patternViewer.IsMatrixEqual(bools))
            {
                scoreModel.Score++;
                spawner.CreateTiles();
                patternViewer.GenerateRandom();
                CreatePlaceZones();
            }
        }
    }
}
