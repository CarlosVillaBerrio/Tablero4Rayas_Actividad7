using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SegundoMomento : MonoBehaviour
{
    public int width; // Variable que representa el ancho
    public int height; // Variable que representa la altura
    public GameObject puzzlePiece; // Variable que representa la pieza del juego
    private GameObject[,] grid; // Matriz de 2 dimensiones
    private int[,] check;
    bool player_1 = true;
    bool player_2 = false;
    public int contadorRojo;
    public int contadorAzul;

    void Start()
    {
        contadorRojo = 0;
        contadorAzul = 0;
        grid = new GameObject[width, height]; // Creacion de la matriz de esferas
        check = new int[width, height];
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
        SelectorEsfera(mPosition);
        ComparadorHorizontal(check);
        ComparadorVertical();

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
                if (check[_x, _y] == 0)
                {
                    go.GetComponent<Renderer>().material.SetColor("_Color", Color.white); // El objeto de la posicion lo vuelve de color blanco
                }
                if (check[_x, _y] == 1)
                {
                    go.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

                }
                if (check[_x, _y] == 2)
                {
                    go.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);

                }
            }      
        }
    

        if (x >= 0 && y >= 0 && x < width && y < height) // Condicional que permite cambiar de color la esfera seleccionada
        {
            if (player_1)
            {
                GameObject go = grid[x, y]; // Se ubica en la posicion de una esfera
                go.GetComponent<Renderer>().material.SetColor("_Color", Color.red); // Colorea la esfera sobre la que se encuentre el mouse
            }

            if (player_2)
            {
                GameObject go = grid[x, y]; // Se ubica en la posicion de una esfera
                go.GetComponent<Renderer>().material.SetColor("_Color", Color.blue); // Colorea la esfera sobre la que se encuentre el mouse
            }
        }
    }

    void SelectorEsfera(Vector3 position)
    {
        int x = (int)(position.x + 0.5f); // Valor de x aproximado al siguiente entero
        int y = (int)(position.y + 0.5f);

        if (player_1 && check[x, y] == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject go = grid[x, y]; // Se ubica en la posicion de una esfera
                go.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                check[x, y] = 1;
                player_1 = false;
                player_2 = true;
            }
           
        }

        if (player_2 && check[x, y] == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject go = grid[x, y]; // Se ubica en la posicion de una esfera
                go.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                check[x, y] = 2;
                player_1 = true;
                player_2 = false;
            }
            
        }
    }

    void ComparadorVertical()
    {
        for (int x = 0; x < width; x++) // Ciclo for anidado sube -->
        {
            for (int y = 0; y < height; y++) // Del ciclo for crea y posiciona las esferas
            {
                if(!player_1 && check[x, y] == 1)
                {
                    contadorRojo = contadorRojo + 1;
                }
                else
                {
                    contadorRojo = 0;
                }

                if (!player_2 && check[x, y] == 2)
                {
                    contadorAzul = contadorAzul + 1;
                }
                else
                {
                    contadorAzul = 0;
                }
            }
        }
        if (contadorRojo == 4)
        {
            SceneManager.LoadScene(0);
            Debug.Log("Player 1 Wins!!!");
        }

        if (contadorAzul == 4)
        {
            SceneManager.LoadScene(0);
            Debug.Log("Player 2 Wins!!!");
        }
    }

    void ComparadorHorizontal(int[,] check)
    {

    }
}


