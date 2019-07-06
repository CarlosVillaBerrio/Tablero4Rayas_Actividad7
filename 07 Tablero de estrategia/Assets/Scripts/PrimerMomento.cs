using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimerMomento : MonoBehaviour
{
    public int width; // Variable que representa el ancho
    public int height; // Variable que representa la altura
    public GameObject puzzlePiece; // Variable que representa la pieza del juego
    private GameObject[,] grid; // Matriz de 2 dimensiones

    void Start()
    {
        grid = new GameObject[width, height]; // Creacion de la matriz
        for (int x = 0; x < width; x++) // Ciclo for anidado
        {
            for (int y = 0; y < height; y++) // Del ciclo for crea y posiciona las esferas
            {
                GameObject go = GameObject.Instantiate(puzzlePiece) as GameObject; // Variable que representa al objeto creado
                Vector3 position = new Vector3(x, y, 0); // Variable que le da la ubicacion a go
                go.transform.position = position; // Sentencia que asigna la posiciona a go
                grid[x, y] = go; // linea que ubica a go
            }
        }
    }

    void Update()
    {
        Vector3 mPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Detecta la posicion del mouse en pantalla
        UpdatePickedPiece(mPosition); // Metodo abreviado del ejemplo
    }


    void UpdatePickedPiece(Vector3 position) // Metodo abreviado
    {

        int x = (int)(position.x + 0.5f); // Valor de x aproximado al siguiente entero
        int y = (int)(position.y + 0.5f); // Valor de y aproximado al siguiente entero


        for (int _x = 0; _x < width; _x++) // Ciclo for anidado
        {
            for (int _y = 0; _y < height; _y++) // Del ciclo for anidado que retorna la esfera a blanca
            {
                GameObject go = grid[_x, _y]; // Toma el objeto de la posicion
                go.GetComponent<Renderer>().material.SetColor("_Color", Color.white); // El objeto de la posicion lo vuelve de color blanco
            }
        }

        if (x >= 0 && y >= 0 && x < width && y < height) // Condicional que permite cambiar de color la esfera seleccionada
        {
            GameObject go = grid[x, y]; // Se ubica en la posicion de una esfera
            go.GetComponent<Renderer>().material.SetColor("_Color", Color.red); // Colorea la esfera sobre la que se encuentre el mouse
        }
    }
}
