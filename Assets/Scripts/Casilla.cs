using UnityEngine;

public class Casilla : MonoBehaviour
{
    public int numeroCasilla;

    private void Awake()
    {
        string casillaString = gameObject.name.Substring(7);
        numeroCasilla = int.Parse(casillaString);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
