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

        private void MovementInputPerformed(InputAction.CallbackContext context)
        {
            isReadInput = true;
        }

        private void MovementInputCanceled(InputAction.CallbackContext context)
        {
            isReadInput = false;
        }

        private void Update()
        {
            if (isReadInput)
            {
                Vector2 direction = movementInput.ReadValue<Vector2>();
                Vector3 moveDelta = speed * Time.deltaTime * (Vector3)direction;
                UpdatePositionServerRpc(moveDelta);
            }
        }

        [ServerRpc]
        private void UpdatePositionServerRpc(Vector3 moveDelta)
        {
            transform.position += moveDelta;
        }
    }
}
