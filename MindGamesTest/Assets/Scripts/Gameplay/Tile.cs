using System;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay
{
    [AddComponentMenu("Scripts/Gameplay/Gameplay.Tile")]
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
