using UnityEngine;

public class Gol : MonoBehaviour
{
    public bool golDoJogador1;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (golDoJogador1)
        {
            FindObjectOfType<GameManager>().AumentarPontuacaoDoJogador2();
            other.gameObject.transform.position = Vector2.zero;
        }
        else
        {
            FindObjectOfType<GameManager>().AumentarPontuacaoDoJogador1();
            other.gameObject.transform.position = Vector2.zero;
        }
    }
}

