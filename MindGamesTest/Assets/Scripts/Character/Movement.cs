using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    [AddComponentMenu("Scripts/Character/Character.Movement")]
    internal class Movement : NetworkBehaviour
    {
        [SerializeField]
        private InputAction movementInput;

        [SerializeField]
        private float speed;

        [SerializeField]
        private bool isReadInput = false;

        [SerializeField]
        private NetworkVariable<Vector3> netPosition = new();

        public override void OnNetworkSpawn()
        {
            if ((IsServer || !IsOwner) && !IsHost)
            {
                return;
            }

            movementInput.Enable();
            movementInput.performed += MovementInputPerformed;
            movementInput.canceled += MovementInputCanceled;
        }

        private void MovementInputCanceled(InputAction.CallbackContext context)
        {
            isReadInput = false;
        }

        private void MovementInputPerformed(InputAction.CallbackContext context)
        {
            isReadInput = true;
        }

        private void Update()
        {
            if (isReadInput)
            {
                Vector2 direction = movementInput.ReadValue<Vector2>();
                Vector3 moveDelta = speed * Time.deltaTime * (Vector3)direction;
                MoveServerRpc(moveDelta);
            }

            transform.position = netPosition.Value;
        }

        [ServerRpc]
        private void MoveServerRpc(Vector3 moveDelta)
        {
            netPosition.Value += moveDelta;
        }
    }
}
