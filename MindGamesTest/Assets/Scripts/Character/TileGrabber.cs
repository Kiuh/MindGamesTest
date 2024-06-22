using Gameplay;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    [AddComponentMenu("Scripts/Character/Character.TileGrabber")]
    internal class TileGrabber : NetworkBehaviour
    {
        [SerializeField]
        private InputAction grabAction;

        [SerializeField]
        private Tile grabbedTile;

        [SerializeField]
        private Transform bufferParent;

        public override void OnNetworkSpawn()
        {
            if ((IsServer || !IsOwner) && !IsHost)
            {
                return;
            }

            grabAction.Enable();
            grabAction.performed += GrabActionPerformed;
        }

        private void GrabActionPerformed(InputAction.CallbackContext obj)
        {
            GrabActionServerRpc();
        }

        [ServerRpc]
        private void GrabActionServerRpc()
        {
            if (grabbedTile == null)
            {
                Collider2D collider = Physics2D.OverlapPoint(transform.position);
                if (collider != null && collider.GetComponent<Tile>() != null)
                {
                    grabbedTile = collider.GetComponent<Tile>();
                    bufferParent = grabbedTile.transform.parent;
                    grabbedTile.transform.parent = transform;
                }
            }
            else
            {
                grabbedTile.transform.parent = bufferParent;
                grabbedTile = null;
                bufferParent = null;
            }
        }
    }
}
