using System;
using Unity.Netcode;
using UnityEngine;

namespace Tiles
{
    [AddComponentMenu("Scripts/Tiles/Tiles.Tile")]
    internal class Tile : NetworkBehaviour
    {
        public event Action OnTileGrabbed;

        public void Grab(Transform newParent)
        {
            transform.parent = newParent;
            OnTileGrabbed?.Invoke();
        }

        public void UnGrab()
        {
            transform.parent = null;
        }
    }
}
