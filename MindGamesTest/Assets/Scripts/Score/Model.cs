using System;
using Unity.Netcode;
using UnityEngine;

namespace Score
{
    [AddComponentMenu("Scripts/Score/Score.Model")]
    internal class Model : NetworkBehaviour
    {
        [SerializeField]
        private NetworkVariable<int> score;

        public event Action<int> OnScoreChanged;

        public int Score
        {
            get => score.Value;
            set => score.Value = value;
        }

        public override void OnNetworkSpawn()
        {
            score.OnValueChanged += (old, newV) => OnScoreChanged?.Invoke(newV);
        }
    }
}
