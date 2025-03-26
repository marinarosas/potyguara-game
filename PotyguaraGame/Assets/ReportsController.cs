using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportsController : MonoBehaviour
{
    public void StartShowReport()
    {
        FindFirstObjectByType<TechGuaraController>().CreateReport("Shows", "Ol� jogador(a)!! Para assistir a um show, voc� deve comprar o ingresso na loja do jogo. " +
            "At� mesmo o show de abertura do jogo, que � GRATUITO, voc� deve resgat�-lo na loja para poder ter acesso ao deck e assistir ao show!!!", 12f, new Vector3(148.55f, 12.17f, 6.88f), 180f);
    }

    public void StartSaleReport()
    {
        FindFirstObjectByType<TechGuaraController>().CreateReport("Loja do Jogo", "Ol� jogador(a)!! Na loja do Potyguara Verse voc� poder� comprar potycoins, as moedas usadas " +
            "para jogar os minigames; Ingressos, usados para assistir aos shows transmitidos na plataforma; Aulas de Medita��o, para poder relaxar na sala de medita��o com o " +
            "auxilio da profissional Andrea Rosas; e por fim, skins para poder customizar o seu avatar!!!", 16f, new Vector3(148.55f, 12.17f, 6.88f), 180f);
    }
    public void StartGamesReport()
    {
        FindFirstObjectByType<TechGuaraController>().CreateReport("Minigames do Potyguara Verse", "Ol� jogador(a)!! No Potyguara Verse voc� poder� usar seus potycoins para jogar " +
            "minigames, sendo cada minimage 10 potycoins.  Com o Hoverbunda, voc� experienciar� a descida no famso Morro do Careca; Na Batalha do Forte, defender� o Forte dos Reis " +
            "Magos dos navios invasores; e, por fim, na Batalha do Forte (Modo Zombie) defender� o Forte dos Reis Magos dos invasores zumbis!!!", 16f, new Vector3(148.55f, 12.17f, 6.88f), 180f);
    }
    public void StartWeatherAndGameModeReport()
    {
        FindFirstObjectByType<TechGuaraController>().CreateReport("Clima e Modos de Jogo", "Ol� jogador(a)!! No Potyguara Verse h� 2 modos de jogo: Modo Tutorial e Modo Normal. No modo tutorial, a " +
            "techguara te acompanhar� em cada area de intera��o do jogo, enquanto que no modo normal voc� n�o ter� orienta��es. Al�m disso, h� um sistema de clima de acordo com " +
            "o clinma do mundo real e tanto o clima quanto o modo de jogo podem ser alterados no menu do Jogo (Pressione Y do controle esquerdo para abri-lo)!!!", 16f, new Vector3(148.55f, 12.17f, 6.88f), 180f);
    }
}