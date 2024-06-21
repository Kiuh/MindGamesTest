using Unity.Netcode;
using UnityEngine;

namespace Common
{
    [AddComponentMenu("Scripts/Common/Common.NetworkManagerHud")]
    internal class NetworkManagerHud : MonoBehaviour
    {
        [SerializeField]
        private NetworkManager networkManager;

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!networkManager.IsClient && !networkManager.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();
            }

            GUILayout.EndArea();
        }

        private void StartButtons()
        {
            if (GUILayout.Button("Host"))
            {
                _ = networkManager.StartHost();
            }

            if (GUILayout.Button("Client"))
            {
                _ = networkManager.StartClient();
            }

            if (GUILayout.Button("Server"))
            {
                _ = networkManager.StartServer();
            }
        }

        private void StatusLabels()
        {
            string mode = networkManager.IsHost
                ? "Host"
                : networkManager.IsServer
                    ? "Server"
                    : "Client";

            GUILayout.Label(
                "Transport: " + networkManager.NetworkConfig.NetworkTransport.GetType().Name
            );
            GUILayout.Label("Mode: " + mode);
        }
    }
}
