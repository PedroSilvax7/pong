using UnityEngine;
using TMPro; // TMP

public class GameManager : MonoBehaviour
{
    public int pontuacaoDoJogador1;
    public int pontuacaoDoJogador2;

    public TextMeshProUGUI textoDePontuacao; // continua público se quiser arrastar, mas não obrigatório

    void Awake()
    {
        // Tenta achar o TMP automaticamente na cena
        if (textoDePontuacao == null)
        {
            textoDePontuacao = FindObjectOfType<TextMeshProUGUI>();
            if (textoDePontuacao == null)
            {
                Debug.LogError("Não foi encontrado nenhum TextMeshProUGUI na cena!");
            }
        }
    }

    void Start()
    {
        AtualizarTextoDePontuacao(); // mostra 0x0 no início
    }

    public void SetPontuacao(int p1, int p2)
    {
        pontuacaoDoJogador1 = p1;
        pontuacaoDoJogador2 = p2;
        AtualizarTextoDePontuacao();
    }

    public void AumentarPontuacaoDoJogador1()
    {
        pontuacaoDoJogador1 += 1;
        AtualizarTextoDePontuacao();
    }

    public void AumentarPontuacaoDoJogador2()
    {
        pontuacaoDoJogador2 += 1;
        AtualizarTextoDePontuacao();
    }

    public void AtualizarTextoDePontuacao()
    {
        if (textoDePontuacao != null)
        {
            textoDePontuacao.text = $"{pontuacaoDoJogador1} x {pontuacaoDoJogador2}";
        }
    }
}
