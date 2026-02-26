using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // INFORMACIÓN DE LAS CASILLAS
    public int[] vectorCasillas; // Posición de los jugadores. 0 -> Vacio / 1 -> Jugador / 2 -> IA.
    public int[] infoCasillas; // Info de las casillas.
    public GameObject[] vectorObjetos; // Posición de las casillas.

    // DADO
    private int dadoNumero;
    private bool dadoGirando = false; // true = girando; false = parado.
    private TextMeshProUGUI dadoNumeroTexto;
    private GameObject dadoCanvas;
    private GameObject dadoBoton;

    // SISTEMA DE TURNOS
    private int  ronda = 0;
    private bool game  = true;
    private bool turno = false; // false = IA, true = jugador.

    // CANVAS
    private TextMeshProUGUI textoRonda;
    private TextMeshProUGUI textoTurno;
    private string turnoJugadorTexto = "Turno del jugador";
    private string turnoIaTexto = "Turno de\nla IA";
    private TextMeshProUGUI textoNarrador;

    private void Awake()
    {
        vectorCasillas = new int[22];

        /*
        0 - normal
        1 - teleport
        2 - vuelve a tirar el dado
        -3 - retrocede 3 casillas
        99 - victoria                  */

        infoCasillas = new int[22];
        for (int i = 22; i < infoCasillas.Length; i++) // Asignación de valores a las casillas
        {
            if (i == 1 || i == 6 || i == 7 || i == 13)  // 1 - TELEPORT
            {
                infoCasillas[i] = 1; 
            }
            else if (i == 12 || i == 18) // 2 - VUELVE A TIRAR EL DADO
            {
                infoCasillas[i] = 2;
            }
            else if (i == 5 || i == 10 || i == 14 || i == 19 || i == 20) // RETROCEDE -3 CASILLAS
            {
                infoCasillas[i] = -3;
            }
            else if (i == 21) // VICTORIA
            {
                infoCasillas[i] = 99;
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

        // Canvas
        textoRonda    = GameObject.Find("Ronda").GetComponent<TextMeshProUGUI>();
        textoTurno    = GameObject.Find("Turno").GetComponent<TextMeshProUGUI>();
        textoNarrador = GameObject.Find("Narrador").GetComponent<TextMeshProUGUI>();

        // Canvas del dado
        dadoCanvas      = GameObject.Find("Dado");
        dadoBoton       = GameObject.Find("DetenerDado");
        dadoNumeroTexto = GameObject.Find("NumeroDado").GetComponent<TextMeshProUGUI>();

        dadoCanvas.SetActive(false);
        dadoBoton.SetActive(false);
        StartCoroutine(GooseGame());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator GooseGame()
    {
        textoNarrador.text = "Bienvenido al juego de la Oca.";
        yield return new WaitForSeconds(2f);
        textoNarrador.text = "¡Comencemos! Empiezas tú, jugador/a.";
        yield return new WaitForSeconds(2f);


        while (game == true)
        {
            // Ronda
            ronda++;
            textoRonda.text = "Ronda: " + ronda;
            // Turno jugador.
            textoTurno.text = turnoJugadorTexto;
            textoNarrador.text = "¡Tira el dado!";
            // Activo el canvas del dado.
            dadoCanvas.SetActive(true);
            dadoBoton.SetActive(true);
            dadoGirando = true;
            while (turno == true)
            {
                yield return null;
            }
        }



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
}
