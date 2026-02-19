using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // INFORMACIÓN DE LAS CASILLAS
    int[] vectorCasillas; // Posición de los jugadores. 0 -> Vacio / 1 -> Jugador / 2 -> IA
    int[] infoCasillas;
    GameObject[] vectorObjetos;

    // DADO
    private int dadoNumero;
    private bool dadoGirando = false;
    private TextMeshProUGUI dadoNumeroTexto;
    private GameObject dadoCanvas;
    private GameObject dadoBoton;

    // SISTEMA DE TURNOS
    int ronda = 0;

    // CANVAS

    private void Awake()
    {
        vectorCasillas = new int[3];
        vectorCasillas[0] = 0; // Casillas vacía.
        vectorCasillas[1] = 1; // Casilla ocupada por el jugador.
        vectorCasillas[2] = 2; // Casilla ocupada por la IA.

        /*
        0 - normal
        1 - teleport
        2 - vuelve a tirar el dado
        -3 - retrocede 3 casillas
        99 - victoria
         */

        infoCasillas = new int[22];
        for (int i = 22; i < infoCasillas.Length; i++) // Asignación de valores a las casillas
        {
            if (i == 1 || i == 6 || i == 7 || i == 13)
            {
                infoCasillas[i] = 1;
            }
            else if (i == 12 || i == 18)
            {
                infoCasillas[i] = 2;
            }
            else if (i == 5 || i == 10 || i == 14 || i == 19 || i == 20)
            {
                infoCasillas[i] = -3;
            }
            else if (i == 21)
            {
                infoCasillas[i] = 9;
            }
            else
            {
                infoCasillas[i] = 0;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Vector para las posiciones de las casillas
        vectorObjetos   = GameObject.FindGameObjectsWithTag("casillas");

        // Canvas del dado
        dadoCanvas      = GameObject.Find("Dado");
        dadoBoton       = GameObject.Find("DetenerDado");
        dadoNumeroTexto = GameObject.Find("NumeroDado").GetComponent<TextMeshProUGUI>();
        



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DetenerDado()
    {
        dadoGirando = false;
        dadoNumero = Random.Range(1, 7);
        dadoNumeroTexto.text = dadoNumero + "";
    }

    IEnumerator GirarDado()
    {
        int numeroAnterior = 0;
        dadoNumero = 0;
        while (dadoGirando == true)
        {
            while (dadoNumero == numeroAnterior)
            {
                dadoNumero = Random.Range(1, 7);
            }
            numeroAnterior = dadoNumero;
            dadoNumeroTexto.text = dadoNumero + "";
            Debug.Log(dadoNumero);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void TitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
