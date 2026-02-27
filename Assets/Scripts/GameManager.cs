using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // INFORMACIÓN DE LAS CASILLAS
    public int[] infoCasillas; // Info de las casillas.
    public int[] vectorCasillas; // Posición de los jugadores. 0 -> Vacio / 1 -> Jugador / 2 -> IA.
    public GameObject[] vectorObjetos; // Posición de las casillas. (Usar  componente "RectTransform" cuyo component es "anchoredPosition").

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

    // BOTONES EN CASO DE QUE UNA CASILLA ESTÉ OCUPADA
    private GameObject casillasAdyacentesCanvas;

    //JUGADORES
    private GameObject jugador;
    private int jugadorCasilla;
    private GameObject IA;
    private int iaCasilla;

    private void Awake()
    {
        vectorCasillas = new int[22];
        vectorObjetos = new GameObject[22];

        /*
        0 - normal
        1 - teleport
        2 - vuelve a tirar el dado
        -3 - retrocede 3 casillas
        99 - victoria                  */

        infoCasillas = new int[22];
        for (int i = 22; i < infoCasillas.Length; i++) // Asignación de valores a las casillas
        {
            if (i == 1 || i == 7 )  // 1 - TELEPORT ( 1 a 6, 7 a 13)
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
        // Vector para las posiciones de las casillas y jugdores.
        for (int i = 0; i < vectorObjetos.Length; i++)
        {
            vectorObjetos[i] = GameObject.Find("casilla" + i);
        }

        // Los jugadores
        jugador = GameObject.Find("Jugador");
        IA = GameObject.Find("IA");

        // Canvas
        textoRonda    = GameObject.Find("Ronda").GetComponent<TextMeshProUGUI>();
        textoTurno    = GameObject.Find("Turno").GetComponent<TextMeshProUGUI>();
        textoNarrador = GameObject.Find("Narrador").GetComponent<TextMeshProUGUI>();
        casillasAdyacentesCanvas = GameObject.Find("EligeAdyacente");

        // Canvas del dado
        dadoCanvas      = GameObject.Find("Dado");
        dadoBoton       = GameObject.Find("DetenerDado");
        dadoNumeroTexto = GameObject.Find("NumeroDado").GetComponent<TextMeshProUGUI>();

        dadoCanvas.SetActive(false);
        dadoBoton.SetActive(false);
        casillasAdyacentesCanvas.SetActive(false);
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
            // Ronda.
            ronda++;
            textoRonda.text = "Ronda: " + ronda;
            // Turno jugador. -------------------------------------------
            textoTurno.text = turnoJugadorTexto;
            textoNarrador.text = "¡Tira el dado, jugador!";
            // Activo el canvas del dado.
            dadoCanvas.SetActive(true);
            dadoBoton.SetActive(true);
            dadoGirando = true;
            StartCoroutine(GirarDado());
            turno = true;
            while (turno == true)
            {
                yield return null;
            }
            // Moviendo el icono del jugador...
            textoNarrador.text = "Moviendo ficha...";
            yield return new WaitForSeconds(1f);
            vectorCasillas[jugadorCasilla] = 0; // El jugador abandona la casilla.
            jugadorCasilla = jugadorCasilla + dadoNumero;
            if (jugadorCasilla > 21)
            {
                int rebote = jugadorCasilla - 21;
                jugadorCasilla = 21 - rebote;
            } // Rebotar si el jugador sobrepasa el número.
            // Evitar la misma casilla
            if (vectorCasillas[jugadorCasilla] != 0)
            {
                textoNarrador.text = "La casilla está ocupada por la IA. Elige una opción.";
                yield return new WaitForSeconds(1f);
                casillasAdyacentesCanvas.SetActive(true);
                turno = true;
                while (turno == true)
                {
                    yield return null;
                }
            }
            // EFECTOS DE CASILLAS //////////////////////////////
            if (jugadorCasilla == 1)  // 1 - TELEPORT
            {
                jugador.GetComponent<RectTransform>().anchoredPosition = vectorObjetos[jugadorCasilla].GetComponent<RectTransform>().anchoredPosition;
                textoNarrador.text = "TELEPORT";
                yield return new WaitForSeconds(1f);
                jugadorCasilla = 6;
                if (vectorCasillas[jugadorCasilla] != 0)
                {
                    textoNarrador.text = "La casilla está ocupada por la IA. Elige una opción.";
                    yield return new WaitForSeconds(1f);
                    casillasAdyacentesCanvas.SetActive(true);
                    turno = true;
                    while (turno == true)
                    {
                        yield return null;
                    }
                }// Evitar la misma casilla
                jugador.GetComponent<RectTransform>().anchoredPosition = vectorObjetos[jugadorCasilla].GetComponent<RectTransform>().anchoredPosition;
                vectorCasillas[jugadorCasilla] = 1;
            } // TELEPORT DE 1 A 6
            else if (jugadorCasilla == 7)
            {
                jugador.GetComponent<RectTransform>().anchoredPosition = vectorObjetos[jugadorCasilla].GetComponent<RectTransform>().anchoredPosition;
                textoNarrador.text = "TELEPORT";
                yield return new WaitForSeconds(1f);
                jugadorCasilla = 13;
                if (vectorCasillas[jugadorCasilla] != 0)
                {
                    textoNarrador.text = "La casilla está ocupada por la IA. Elige una opción.";
                    yield return new WaitForSeconds(1f);
                    casillasAdyacentesCanvas.SetActive(true);
                    turno = true;
                    while (turno == true)
                    {
                        yield return null;
                    }
                }// Evitar la misma casilla
                jugador.GetComponent<RectTransform>().anchoredPosition = vectorObjetos[jugadorCasilla].GetComponent<RectTransform>().anchoredPosition;
                vectorCasillas[jugadorCasilla] = 1;
            } // TELEPORT DE 7 A 13
            else if (jugadorCasilla == 12 || jugadorCasilla == 18) // 2 - VUELVE A TIRAR EL DADO
            {
                jugador.GetComponent<RectTransform>().anchoredPosition = vectorObjetos[jugadorCasilla].GetComponent<RectTransform>().anchoredPosition;
                textoNarrador.text = "¡TIRA EL DADO DE NUEVO!";
                // Activo el canvas del dado.
                dadoCanvas.SetActive(true);
                dadoBoton.SetActive(true);
                dadoGirando = true;
                StartCoroutine(GirarDado());
                turno = true;
                while (turno == true)
                {
                    yield return null;
                } // El juego espera por el jugador
                jugadorCasilla = jugadorCasilla + dadoNumero;
                if (jugadorCasilla > 21)
                {
                    int rebote = jugadorCasilla - 21;
                    jugadorCasilla = 21 - rebote;
                } // Rebotar si el jugador sobrepasa el número.
                if (vectorCasillas[jugadorCasilla] != 0)
                {
                    textoNarrador.text = "La casilla está ocupada por la IA. Elige una opción.";
                    yield return new WaitForSeconds(1f);
                    casillasAdyacentesCanvas.SetActive(true);
                    turno = true;
                    while (turno == true)
                    {
                        yield return null;
                    }
                }// Evitar la misma casilla
                jugador.GetComponent<RectTransform>().anchoredPosition = vectorObjetos[jugadorCasilla].GetComponent<RectTransform>().anchoredPosition;
                vectorCasillas[jugadorCasilla] = 1;
                textoNarrador.text = "Al haber tirado el dado dos veces, cancelas el efecto de la casilla en la que acabas de caer";
                yield return new WaitForSeconds(4f);
            } // LANZA EL DADO DE NUEVO
            else if (jugadorCasilla == 5 || jugadorCasilla == 10 || jugadorCasilla == 14 || jugadorCasilla == 19 || jugadorCasilla == 20) // RETROCEDE -3 CASILLAS
            {
                jugador.GetComponent<RectTransform>().anchoredPosition = vectorObjetos[jugadorCasilla].GetComponent<RectTransform>().anchoredPosition;
                textoNarrador.text = "Retrocede 3 casillas";
                jugadorCasilla -= 3;
                yield return new WaitForSeconds(2f);
                if (vectorCasillas[jugadorCasilla] != 0)
                {
                    textoNarrador.text = "La casilla está ocupada por la IA. Elige una opción.";
                    yield return new WaitForSeconds(1f);
                    casillasAdyacentesCanvas.SetActive(true);
                    turno = true;
                    while (turno == true)
                    {
                        yield return null;
                    }
                }// Evitar la misma casilla
                jugador.GetComponent<RectTransform>().anchoredPosition = vectorObjetos[jugadorCasilla].GetComponent<RectTransform>().anchoredPosition;
                vectorCasillas[jugadorCasilla] = 1;
            } // RETROCEDE 3
            else if (jugadorCasilla == 21) // VICTORIA
            {
                jugador.GetComponent<RectTransform>().anchoredPosition = vectorObjetos[jugadorCasilla].GetComponent<RectTransform>().anchoredPosition;
                textoNarrador.text = "¡El jugador ha ganado!";
                game = false;
                vectorCasillas[jugadorCasilla] = 1;
                StopAllCoroutines();
            }// El jugador ha ganado, se termina el juego.
            else
            {
                jugador.GetComponent<RectTransform>().anchoredPosition = vectorObjetos[jugadorCasilla].GetComponent<RectTransform>().anchoredPosition;
                vectorCasillas[jugadorCasilla] = 1;
            }
            // Turno IA. ----------------------------------------------
            textoTurno.text = turnoIaTexto;
            textoNarrador.text = "¡Tira el dado, IA!";
            StartCoroutine(GirarDado());
            yield return new WaitForSeconds(2f);
            TiradaDadoIA();
            // Moviendo el icono de la IA...
            textoNarrador.text = "Moviendo ficha...";
            yield return new WaitForSeconds(1f);
            vectorCasillas[iaCasilla] = 0; // La IA abandona la casilla.
            iaCasilla = iaCasilla + dadoNumero;
            if (iaCasilla > 21)
            {
                int rebote = iaCasilla - 21;
                iaCasilla = 21 - rebote;
            } // Rebotar si la IA sobrepasa el número.
            // Evitar la misma casilla
            if (vectorCasillas[iaCasilla] != 0)
            {
                textoNarrador.text = "La casilla está ocupada por el jugador. Elige una opción.";
                yield return new WaitForSeconds(1f);
                iaEligeCasilla();
                textoNarrador.text = "La IA se ha decidido.";
                yield return new WaitForSeconds(1f);
            }
            // EFECTOS DE CASILLAS ----------------------------
            //////////////////////////////////////////////////
            if (iaCasilla == 1)  // 1 - TELEPORT
            {
                IA.GetComponent<RectTransform>().anchoredPosition = vectorObjetos[iaCasilla].GetComponent<RectTransform>().anchoredPosition;
                textoNarrador.text = "TELEPORT";
                yield return new WaitForSeconds(1f);
                iaCasilla = 6;
                if (vectorCasillas[iaCasilla] != 0)
                {
                    textoNarrador.text = "La casilla está ocupada por el jugador. Elige una opción.";
                    yield return new WaitForSeconds(1f);
                    iaEligeCasilla();
                    textoNarrador.text = "La IA se ha decidido.";
                    yield return new WaitForSeconds(1f);
                } // Evitar la misma casilla
                IA.GetComponent<RectTransform>().anchoredPosition = vectorObjetos[iaCasilla].GetComponent<RectTransform>().anchoredPosition;
                vectorCasillas[iaCasilla] = 2;
            }
            else if (iaCasilla == 7)
            {
                IA.GetComponent<RectTransform>().anchoredPosition = vectorObjetos[iaCasilla].GetComponent<RectTransform>().anchoredPosition;
                textoNarrador.text = "TELEPORT";
                yield return new WaitForSeconds(1f);
                iaCasilla = 13;
                if (vectorCasillas[iaCasilla] != 0)
                {
                    textoNarrador.text = "La casilla está ocupada por el jugador. Elige una opción.";
                    yield return new WaitForSeconds(1f);
                    iaEligeCasilla();
                    textoNarrador.text = "La IA se ha decidido.";
                    yield return new WaitForSeconds(1f);
                } // Evitar la misma casilla
                IA.GetComponent<RectTransform>().anchoredPosition = vectorObjetos[iaCasilla].GetComponent<RectTransform>().anchoredPosition;
                vectorCasillas[iaCasilla] = 2;
            }
            else if (iaCasilla == 12 || iaCasilla == 18) // 2 - VUELVE A TIRAR EL DADO
            {
                IA.GetComponent<RectTransform>().anchoredPosition = vectorObjetos[iaCasilla].GetComponent<RectTransform>().anchoredPosition;
                textoNarrador.text = "¡TIRA EL DADO DE NUEVO!";
                StartCoroutine(GirarDado());
                yield return new WaitForSeconds(2f);
                TiradaDadoIA();
                iaCasilla = iaCasilla + dadoNumero;
                if (iaCasilla > 21)
                {
                    int rebote = iaCasilla - 21;
                    iaCasilla = 21 - rebote;
                } // Rebotar si la IA sobrepasa el número.
                if (vectorCasillas[iaCasilla] != 0)
                {
                    textoNarrador.text = "La casilla está ocupada por el jugador. Elige una opción.";
                    yield return new WaitForSeconds(1f);
                    iaEligeCasilla();
                    textoNarrador.text = "La IA se ha decidido.";
                    yield return new WaitForSeconds(1f);
                } // Evitar la misma casilla
                IA.GetComponent<RectTransform>().anchoredPosition = vectorObjetos[iaCasilla].GetComponent<RectTransform>().anchoredPosition;
                vectorCasillas[iaCasilla] = 2;
                textoNarrador.text = "Al haber tirado el dado dos veces, cancelas el efecto de la casilla en la que acabas de caer";
                yield return new WaitForSeconds(4f);
            }
            else if (iaCasilla == 5 || iaCasilla == 10 || iaCasilla == 14 || iaCasilla == 19 || iaCasilla == 20) // RETROCEDE -3 CASILLAS
            {
                IA.GetComponent<RectTransform>().anchoredPosition = vectorObjetos[iaCasilla].GetComponent<RectTransform>().anchoredPosition;
                textoNarrador.text = "Retrocede 3 casillas";
                iaCasilla -= 3;
                yield return new WaitForSeconds(2f);
                if (vectorCasillas[iaCasilla] != 0)
                {
                    textoNarrador.text = "La casilla está ocupada por el jugador. Elige una opción.";
                    yield return new WaitForSeconds(1f);
                    iaEligeCasilla();
                    textoNarrador.text = "La IA se ha decidido.";
                    yield return new WaitForSeconds(1f);
                } // Evitar la misma casilla
                IA.GetComponent<RectTransform>().anchoredPosition = vectorObjetos[iaCasilla].GetComponent<RectTransform>().anchoredPosition;
                vectorCasillas[iaCasilla] = 2;
            }
            else if (iaCasilla == 21) // VICTORIA
            {
                IA.GetComponent<RectTransform>().anchoredPosition = vectorObjetos[iaCasilla].GetComponent<RectTransform>().anchoredPosition;
                textoNarrador.text = "¡El jugador ha ganado!";
                game = false;
                vectorCasillas[iaCasilla] = 2;
                StopAllCoroutines();
            }// La IA ha ganado, se termina el juego.
            else
            {
                IA.GetComponent<RectTransform>().anchoredPosition = vectorObjetos[iaCasilla].GetComponent<RectTransform>().anchoredPosition;
                vectorCasillas[iaCasilla] = 2;
            }
            //////////////////////////////////////////////////
            IA.GetComponent<RectTransform>().anchoredPosition = vectorObjetos[iaCasilla].GetComponent<RectTransform>().anchoredPosition;
            vectorCasillas[iaCasilla] = 2;
            if (iaCasilla == 21)
            {
                textoNarrador.text = "¡La IA ha ganado!";
                game = false;
                StopAllCoroutines();
            } // La IA ha ganado, se termina el juego.
        }
    }

    public void SumarUno()
    {
        jugadorCasilla++;
        casillasAdyacentesCanvas.SetActive(false);
        turno = false;
    }

    public void RestarUno()
    {
        jugadorCasilla--;
        casillasAdyacentesCanvas.SetActive(false);
        turno = false;
    }

    public void iaEligeCasilla()
    {
        int randomChoice = Random.Range(0, 2);
        if (randomChoice == 0)
        {
            iaCasilla--;
        }
        else if (randomChoice == 1)
        {
            iaCasilla++;
        }
    }

    public void DetenerDado()
    {
        dadoGirando = false;
        dadoNumero = Random.Range(1, 7);
        dadoNumeroTexto.text = dadoNumero + "";
        turno = false;
        // Desactivo el botón del dado
        dadoBoton.SetActive(false);
    }

    public void TiradaDadoIA()
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
