using UnityEngine;

public class Bola : MonoBehaviour
{
    public float velocidadeDaBola;

    public Rigidbody2D oRigidbody2D;

    void Awake()
    {
        // Garante que oRigidbody2D esteja atribuído
        if (oRigidbody2D == null)
            oRigidbody2D = GetComponent<Rigidbody2D>();

        if (oRigidbody2D == null)
            Debug.LogError("Bola: Rigidbody2D não encontrado no GameObject!");
    }

    void Start()
    {
        MoverBola();
    }

    void Update()
    {
        // pode adicionar lógica futura aqui
    }

    private void MoverBola()
    {
        if (oRigidbody2D != null)
        {
            // Define a velocidade inicial da bola
            oRigidbody2D.linearVelocity = new Vector2(velocidadeDaBola, velocidadeDaBola);
        }
    }
}