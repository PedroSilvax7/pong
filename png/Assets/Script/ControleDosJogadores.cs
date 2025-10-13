using UnityEngine;

public class ControleDosJogadores : MonoBehaviour
{
    public float velocidadeDoJogador;
    public bool jogador1;
    public float yMinimo;
    public float yMaximo;

    void Update()
    {
        if (jogador1)
            MoverJogador1();
        else
            MoverJogador2();
    }

    private void MoverJogador1()
    {
        float movimento = 0f;

        if (Input.GetKey(KeyCode.W))
            movimento = 1f;
        else if (Input.GetKey(KeyCode.S))
            movimento = -1f;

        transform.Translate(Vector2.up * movimento * velocidadeDoJogador * Time.deltaTime);

        // Limitar posição
        transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, yMinimo, yMaximo));
    }

    private void MoverJogador2()
    {
        float movimento = 0f;

        if (Input.GetKey(KeyCode.UpArrow))
            movimento = 1f;
        else if (Input.GetKey(KeyCode.DownArrow))
            movimento = -1f;

        transform.Translate(Vector2.up * movimento * velocidadeDoJogador * Time.deltaTime);

        // Limitar posição
        transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, yMinimo, yMaximo));
    }
}

