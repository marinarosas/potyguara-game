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
                report.UpdateDescription("Você acaba de entrar em um mundo onde a cultura e a tecnologia se encontram em uma experiência imersiva única. Eu sou a Techyguara, sua guia," +
                    " e juntos vamos explorar esse universo cheio de novidades! No Potyguara Verse, você poderá participar de eventos incríveis, jogar minigames, visitar nossa loja exclusiva e muito mais.");
            }
            if(SceneManager.GetActiveScene().buildIndex == 1)
            {
                report.UpdateTitle("Antes de começarmos, vamos conhecer um pouco mais sobre você!");
                report.UpdateDescription("Complete o seu perfil e crie o seu avatar para começar a sua jornada!");

                // depois de 15 segundos
                report.UpdateTitle("Crie seu Avatar!");
                report.UpdateDescription("Agora que você já se apresentou, é hora de criar seu avatar! Escolha suas características, como rosto, cabelo, roupas e acessórios para refletir sua personalidade no " +
                    "Potyguara Verse. Depois, você estará pronto para explorar este mundo como nunca antes!");
            }
            if(SceneManager.GetActiveScene().buildIndex == 2)
            {
                report.UpdateTitle("Praia de Ponta Negra");
                report.UpdateDescription("Você está na famosa Praia de Ponta Negra, uma das mais conhecidas da Cidade do Natal, principalmente por conta do imponente Morro do Careca, com seus 110 metros de altura. " +
                    "Aqui, você encontrará diversos eventos, como shows, exposições e o emocionante minigame Hoverbunda. Sinta-se livre para caminhar pela praia, explorar as atrações e aproveitar os eventos incríveis!");
            }
            if(SceneManager.GetActiveScene().buildIndex == 3)
            {
                report.UpdateTitle("Forte dos Reis Magos");
                report.UpdateDescription("Agora, vamos à Fortaleza dos Reis Magos, um dos locais mais históricos da cidade de Natal. Este lugar foi palco de batalhas importantes que mudaram o rumo da nossa região." +
                    "Aqui, você poderá jogar minigames inspirados em épocas passadas. Sabia que, durante as invasões holandesas, a cidade de Natal foi chamada de Nova Amsterdã? Explore os muros de pedra e descubra " +
                    "mais sobre essa fascinante história enquanto se diverte!");
            }
            if(SceneManager.GetActiveScene().buildIndex == 4)
            {
                report.UpdateTitle("HoverBunda");
                report.UpdateDescription("Prepare-se para a adrenalina no Hoverbunda, uma corrida emocionante onde você se lança no seu skibunda voador! Compita contra seus amigos e mostre que você é o melhor, pois " +
                    "apenas o mais rápido cruzará a linha de chegada!");
            }
        }
    }
}
