using UnityEngine;

public class Casilla : MonoBehaviour
{
    public int numeroCasilla;

    private void Awake()
    {
        string name = gameObject.name;
        numeroCasilla = int.Parse(name);
        Debug.Log(numeroCasilla);
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
