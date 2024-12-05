using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Networking.Transport.Relay;
using Unity.Netcode.Transports.UTP;

public class UIMultiplayer : NetworkBehaviour
{
    [Header("Principal")]
    [SerializeField] Button copiar;
    [SerializeField] TextMeshProUGUI jugadoresTxt;
    [SerializeField] GameObject menuGO, codigoGO;

    [Header("Menu")]
    [SerializeField] TextMeshProUGUI codigoHost;
    [SerializeField] TMP_InputField codigoCliente;
    [SerializeField] Button host, cliente;

    private NetworkVariable<int> jugadores = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);

    async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        host.onClick.AddListener(Host);
        cliente.onClick.AddListener(Cliente);

        copiar.onClick.AddListener(() => {
            GUIUtility.systemCopyBuffer = codigoHost.text;
        });
    }

    private void Update()
    {
        jugadoresTxt.text = $"Jugadores: {jugadores.Value}";

        if (!IsServer) return;
        jugadores.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }

    private async void Host() 
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
        string codigo = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        codigoHost.text = codigo;

        RelayServerData data = new RelayServerData(allocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(data);
        NetworkManager.Singleton.StartHost();

        GUIUtility.systemCopyBuffer = codigo;

        menuGO.SetActive(false);
        codigoGO.SetActive(true);

    }

    private async void Cliente()
    {
        var allocation = await RelayService.Instance.JoinAllocationAsync(
            codigoCliente.text
        );

        RelayServerData data = new RelayServerData(allocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(data);
        NetworkManager.Singleton.StartClient();

        menuGO.SetActive(false);
    }
}

