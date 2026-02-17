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
    private bool dadoGirando = true;
    private TextMeshProUGUI dadoNumeroTexto;
    private GameObject dadoCanvas;
    private GameObject detenerDadoCanvas;

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
        2 - vuelve a tirar
        -3 - retrocede 3 casillas
        99 - victoria
         */

        infoCasillas = new int[22];
        infoCasillas[0]  = 0;
        infoCasillas[1]  = 1;
        infoCasillas[2]  = 0;
        infoCasillas[3]  = 0;
        infoCasillas[4]  = 0;
        infoCasillas[5]  = -3;
        infoCasillas[6]  = 1;
        infoCasillas[7]  = 1;
        infoCasillas[8]  = 0;
        infoCasillas[9]  = 0;
        infoCasillas[10] = -3;
        infoCasillas[11] = 0;
        infoCasillas[12] = 2;
        infoCasillas[13] = 1;
        infoCasillas[14] = -3;
        infoCasillas[15] = 0;
        infoCasillas[16] = 0;
        infoCasillas[17] = 0;
        infoCasillas[18] = 2;
        infoCasillas[19] = -3;
        infoCasillas[20] = -3;
        infoCasillas[21] = 99;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Vector para las posiciones de las casillas
        vectorObjetos   = GameObject.FindGameObjectsWithTag("casillas");

        // Canvas del dado
        dadoCanvas        = GameObject.Find("Dado");
        detenerDadoCanvas = GameObject.Find("DetenerDado");
        dadoNumeroTexto   = GameObject.Find("NumeroDado").GetComponent<TextMeshProUGUI>();
        StartCoroutine(GirarDado());
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
