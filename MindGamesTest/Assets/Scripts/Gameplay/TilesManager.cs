using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay
{
    [AddComponentMenu("Scripts/Gameplay/Gameplay.TilesManager")]
    internal class TilesManager : NetworkBehaviour
    {
        [SerializeField]
        private List<Tile> tiles;

        public void CreateTiles() { }
    }
}
