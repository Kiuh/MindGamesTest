using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Unity.Netcode;
using UnityEngine;

namespace Tiles
{
    [AddComponentMenu("Scripts/Tiles/Tiles.Matcher")]
    internal class Matcher : NetworkBehaviour
    {
        [Serializable]
        private class PlaceZone
        {
            public bool IsBusy;
            public Bounds Bounds;
            public Vector2Int Position;
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
        private Spawner spawner;

        private List<PlaceZone> placeZones = new();

        public override void OnNetworkSpawn()
        {
            CreatePlaceZones();
            foreach (PlaceZone zone in placeZones)
            {
                _ = Instantiate(placePrefab, zone.Bounds.center, Quaternion.identity, transform);
            }
        }

        private void CreatePlaceZones()
        {
            placeZones.Clear();
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    Vector3 position =
                        transform.position + new Vector3(shift.x * i, shift.y * j, 0);
                    PlaceZone zone =
                        new()
                        {
                            IsBusy = false,
                            Bounds = new Bounds(position, bounds),
                            Position = new(i, j)
                        };
                    placeZones.Add(zone);
                }
            }
        }

        public void TryPlaceTile(Tile tile)
        {
            foreach (PlaceZone zone in placeZones)
            {
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

            List<GridCell> grid = placeZones
                .Select(x => new GridCell() { Busy = x.IsBusy, Position = x.Position })
                .ToList();

            if (patternViewer.IsGridEqual(grid))
            {
                scoreModel.Score++;
                spawner.CreateTiles();
                patternViewer.GenerateRandomGrid();
                CreatePlaceZones();
            }
        }
    }
}
