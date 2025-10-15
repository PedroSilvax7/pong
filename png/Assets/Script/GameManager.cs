using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int pontuacaoDoJogador1;
    public int pontuacaoDoJogador2;

    // Você pode arrastar um destes no Inspector, ou nenhum — o script tenta achar automaticamente.
    public TextMeshProUGUI textoTMP;
    public Text textoLegacy;

    void Awake()
    {
        // Se nada foi arrastado, tenta achar automaticamente na cena (prioriza TMP)
        if (textoTMP == null)
            textoTMP = FindObjectOfType<TextMeshProUGUI>();

        if (textoTMP == null && textoLegacy == null)
            textoLegacy = FindObjectOfType<Text>();
    }

    void Start()
    {
        AtualizarTextoDePontuacao();
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
        string s = $"{pontuacaoDoJogador1} x {pontuacaoDoJogador2}";

        if (textoTMP != null)
        {
            textoTMP.text = s;
            return;
        }

        if (textoLegacy != null)
        {
            textoLegacy.text = s;
            return;
        }

        // Nenhum componente de UI disponível — não quebra, apenas loga
        Debug.LogWarning("GameManager: nenhum TextMeshProUGUI ou Text encontrado para mostrar a pontuação. Mensagem: " + s);
    }
}

