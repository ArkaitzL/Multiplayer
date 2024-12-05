using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(CharacterController))]
public class Movimiento : NetworkBehaviour
{
    private CharacterController controller;
    private Vector3 playerV;
    private bool enSuelo;

    [SerializeField] float velocidad = 2.0f;
    [SerializeField] float salto = 1.0f;
    [SerializeField] float gravityValue = -9.81f;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        // Solo el propietario puede controlarlo
        if (!IsOwner) return;

        // Verificar si el jugador está en el suelo
        enSuelo = controller.isGrounded;

        // Mantiene el jugador en el suelo
        if (enSuelo && playerV.y < 0)
        {
            playerV.y = -2f;
        }

        // Movimiento en XZ
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(x, 0, y);
        controller.Move(move * Time.deltaTime * velocidad);

        // Rotar hacia la dirección del movimiento
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Salto
        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            Saltar(salto);
        }

        // Aplicar gravedad
        playerV.y += gravityValue * Time.deltaTime;

        // Movimiento vertical
        controller.Move(playerV * Time.deltaTime);
    }

    public void Saltar(float fuerza) {
        playerV.y = Mathf.Sqrt(fuerza * -2.0f * gravityValue);
    }
}
