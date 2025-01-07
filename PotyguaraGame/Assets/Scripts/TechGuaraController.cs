using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TechGuaraController : MonoBehaviour
{
    public Report report;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if(SceneManager.GetActiveScene().buildIndex == 0)
            {
                report.UpdateTitle("Bem-vindo(a) ao Potyguara Verse!");
                report.UpdateDescription("Voc� acaba de entrar em um mundo onde a cultura e a tecnologia se encontram em uma experi�ncia imersiva �nica. Eu sou a Techyguara, sua guia," +
                    " e juntos vamos explorar esse universo cheio de novidades! No Potyguara Verse, voc� poder� participar de eventos incr�veis, jogar minigames, visitar nossa loja exclusiva e muito mais.");
            }
            if(SceneManager.GetActiveScene().buildIndex == 1)
            {
                report.UpdateTitle("Antes de come�armos, vamos conhecer um pouco mais sobre voc�!");
                report.UpdateDescription("Complete o seu perfil e crie o seu avatar para come�ar a sua jornada!");

                // depois de 15 segundos
                report.UpdateTitle("Crie seu Avatar!");
                report.UpdateDescription("Agora que voc� j� se apresentou, � hora de criar seu avatar! Escolha suas caracter�sticas, como rosto, cabelo, roupas e acess�rios para refletir sua personalidade no " +
                    "Potyguara Verse. Depois, voc� estar� pronto para explorar este mundo como nunca antes!");
            }
            if(SceneManager.GetActiveScene().buildIndex == 2)
            {
                report.UpdateTitle("Praia de Ponta Negra");
                report.UpdateDescription("Voc� est� na famosa Praia de Ponta Negra, uma das mais conhecidas da Cidade do Natal, principalmente por conta do imponente Morro do Careca, com seus 110 metros de altura. " +
                    "Aqui, voc� encontrar� diversos eventos, como shows, exposi��es e o emocionante minigame Hoverbunda. Sinta-se livre para caminhar pela praia, explorar as atra��es e aproveitar os eventos incr�veis!");
            }
            if(SceneManager.GetActiveScene().buildIndex == 3)
            {
                report.UpdateTitle("Forte dos Reis Magos");
                report.UpdateDescription("Agora, vamos � Fortaleza dos Reis Magos, um dos locais mais hist�ricos da cidade de Natal. Este lugar foi palco de batalhas importantes que mudaram o rumo da nossa regi�o." +
                    "Aqui, voc� poder� jogar minigames inspirados em �pocas passadas. Sabia que, durante as invas�es holandesas, a cidade de Natal foi chamada de Nova Amsterd�? Explore os muros de pedra e descubra " +
                    "mais sobre essa fascinante hist�ria enquanto se diverte!");
            }
            if(SceneManager.GetActiveScene().buildIndex == 4)
            {
                report.UpdateTitle("HoverBunda");
                report.UpdateDescription("Prepare-se para a adrenalina no Hoverbunda, uma corrida emocionante onde voc� se lan�a no seu skibunda voador! Compita contra seus amigos e mostre que voc� � o melhor, pois " +
                    "apenas o mais r�pido cruzar� a linha de chegada!");
            }
        }
    }
}
