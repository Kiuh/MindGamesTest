using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Unity.Netcode;
using UnityEngine;

namespace PatternViewer
{
    [AddComponentMenu("Scripts/PatternViewer/PatternViewer.Model")]
    internal class Model : NetworkBehaviour
    {
        [SerializeField]
        private Vector2Int size;
        public Vector2Int Size => size;

        private NetworkList<GridCell> grid;
        public NetworkList<GridCell> Grid => grid;

        public event Action OnGridChanged;

        private void Awake()
        {
            grid = new();
        }

        public override void OnNetworkSpawn()
        {
            grid.OnListChanged += (l) => OnGridChanged?.Invoke();
            if (IsServer || IsHost)
            {
                GenerateRandomGrid();
            }
            else
            {
                OnGridChanged?.Invoke();
            }
        }

        public void GenerateRandomGrid()
        {
            grid.Clear();
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    GridCell cell =
                        new() { Busy = UnityEngine.Random.value >= 0.5f, Position = new(i, j) };
                    grid.Add(cell);
                }
            }
        }

        public bool IsGridEqual(List<GridCell> otherGrid)
        {
            foreach (GridCell cell in Grid)
            {
                if (!otherGrid.Any(x => x.Equals(cell)))
                {
                    return false;
                }
            }
            return true;
        }

        public override void OnDestroy()
        {
            grid?.Dispose();
        }
    }
}
