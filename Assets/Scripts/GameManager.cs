using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int[] vectorCasillas; // Posición de los jugadores. 0 -> Vacio / 1 -> Jugador / 2 -> IA
    int[] infoCasillas; // Info casillas
    GameObject[] vecotrObjetos;

    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
