using System;
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

        private NetworkList<bool> matrix;

        public bool GetMatrixValue(int x, int y)
        {
            int index = (x * size.x) + y;
            return index >= matrix.Count ? false : matrix[index];
        }

        public event Action OnMatrixChanged;

        private void Awake()
        {
            matrix = new();
        }

        public override void OnNetworkSpawn()
        {
            matrix.OnListChanged += (list) => OnMatrixChanged?.Invoke();
            if (IsServer || IsHost)
            {
                GenerateRandomServerRpc();
            }
            else
            {
                OnMatrixChanged?.Invoke();
            }
        }

        [ServerRpc]
        public void GenerateRandomServerRpc()
        {
            matrix.Clear();
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    matrix.Add(UnityEngine.Random.value >= 0.5f);
                }
            }
        }

        public override void OnDestroy()
        {
            matrix?.Dispose();
        }
    }
}
