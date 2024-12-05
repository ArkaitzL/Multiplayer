using UnityEngine;
using Unity.Netcode;

public class Saltadera : NetworkBehaviour
{
    [SerializeField] float fuerza = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return; // Solo se ejecuta en el servidor

        Movimiento mv = other.GetComponent<Movimiento>();
        if (mv == null) return;

        mv.Saltar(fuerza); // Aplica el salto en el servidor
    }
}
