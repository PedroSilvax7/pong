using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int pontuacaoDoJogador1;

    public int pontuacaoDoJogador2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AumentarPontuacaoDoJogador1(){
        pontuacaoDoJogador1 += 1;

    }

    public void AumentarPontuacaoDoJogador2(){
        pontuacaoDoJogador2 +=1;

    }
}
