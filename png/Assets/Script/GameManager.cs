using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int pontuacaoDoJogador1;

    public int pontuacaoDoJogador2;

    public Text textoDePontuacao;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pontuacaoDoJogador1 = 0;
        pontuacaoDoJogador2 = 0;
        textoDePontuacao.text = pontuacaoDoJogador1 + " X " + pontuacaoDoJogador2;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            ReiniciarPartida();
        }
    }

    public void AumentarPontuacaoDoJogador1(){
        pontuacaoDoJogador1 += 1;
        AtualizarTextoDePontuaçao();

    }

    public void AumentarPontuacaoDoJogador2(){
        pontuacaoDoJogador2 +=1;
        AtualizarTextoDePontuaçao();

    }

    public void AtualizarTextoDePontuaçao(){
        textoDePontuacao.text = pontuacaoDoJogador1 + " X " + pontuacaoDoJogador2;
    }

    private void ReiniciarPartida(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    private void SairDoJogo(){
        Application.Quit();
    }
}
