using UnityEngine;
using Unity.Netcode;

public class Movimiento : NetworkBehaviour
{
    public float speed = 5f; // Velocidad de movimiento
    public float jumpHeight = 2f; // Altura de salto
    public float gravity = -9.81f; // Gravedad
    private Vector3 velocity; // Controla el movimiento vertical
    private CharacterController controller; // Referencia al CharacterController

    void Start()
    {
        // Obtenemos el CharacterController del objeto
        controller = GetComponent<CharacterController>();
    }


    void Update()
    {
        if (!IsOwner) return;
        // Movimiento horizontal
        float horizontal = Input.GetAxis("Horizontal"); // A/D o flechas izquierda/derecha
        float vertical = Input.GetAxis("Vertical"); // W/S o flechas arriba/abajo
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // Aplicamos movimiento
        controller.Move(move * speed * Time.deltaTime);

        // Comprobamos si estamos en el suelo
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Mantenemos al personaje pegado al suelo
        }

        // Salto
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Aplicamos gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
