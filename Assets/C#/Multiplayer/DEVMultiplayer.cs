using UnityEngine;

public class DEVMultiplayer : MonoBehaviour
{
    [SerializeField] int width = 1280, height = 720; 

    void Start()
    {
        // Configura la resolución y modo ventana
        width = 1280; // Ancho de la ventana
        height = 720; // Alto de la ventana
        bool fullscreen = false; // Define si inicia en modo ventana

        Screen.SetResolution(width, height, fullscreen);
    }
}
