using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Libreria para usar el reinicio del tablero cuando se gana

public class SegundoMomento : MonoBehaviour
{
    public int width; // Variable que representa el ancho
    public int height; // Variable que representa la altura
    public GameObject puzzlePiece; // Variable que representa la pieza del juego
    private GameObject[,] grid; // Matriz de 2 dimensiones
    private int[,] check; // Variable para comprobar la matriz
    bool player_1 = true; // Variable para el turno del jugador 1
    bool player_2 = false; // Variable para el turno del jugador 2
    int contadorRojo; // Variable que cuenta el numero de esferas rojas seguidas
    int contadorAzul; // Variable que cuenta el numero de esferas azules seguidas

    public GameObject eP1; // Variable para la esfera que indica el turno del jugador 1
    public GameObject eP2; // Variable para la esfera que indica el turno del jugador 2

    static int diferenciaPuntuacion = 0; // Variable para calcular l diferencia de puntos
    static int puntuacionP1 = 0; // Variable que marca los puntos del jugador 1
    static int puntuacionP2 = 0; // Variable que marca los puntos del jugador 2
    public TextMesh puntajeP1; // Variable cuadro de texto que nos muestra los puntos del jugador 1
    public TextMesh puntajeP2; // Variable cuadro de texto que nos muestra los puntos del jugador 2

    void Start()
    {
        contadorRojo = 0; // Inicializar contadorRojo para evitar problemas
        contadorAzul = 0; // Inicializar contadorAzul para evitar problemas
        AnuncioTurnoJugador(); // Metodo que enciende la esfera al lado de la etiqueta del jugador en turno
        Marcador2Jugadores(); // Metodo que cuenta los puntos de los jugadores

        grid = new GameObject[width, height]; // Creacion de la matriz de esferas
        check = new int[width, height]; // Inicializacion de la variable check
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
        ReglaEspecial(); // REGLA ESPECIAL: El jugador que vaya perdiendo por 3 puntos tiene asegurada la victoria en la proxima partida
        Vector3 mPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Detecta la posicion del mouse en pantalla
        AnuncioTurnoJugador(); // Cambia el color de la esfera al lado de la etiqueta segun el turno del jugador
        UpdatePickedPiece(mPosition); // Metodo abreviado del ejemplo
        SelectorEsfera(mPosition); // Funcion que nos indica la esfera sobre la que estamos
        ComparadorHorizontal(check); // Funcion que hace la comparacion horizontal de las esferas del mismo color
        ComparadorVertical(); // Funcion que hace la comparacion vertical de las esferas del mismo color
        ComparadorOblicuo1(); // Funcion que hace la comparacion de la primera diagonal de las esferas del mismo color
        ComparadorOblicuo2(); // Funcion que hace la comparacion de la segunda diagonal de las esferas del mismo color
        Marcador2Jugadores(); // Metodo que cuenta los puntos de los jugadores
    }

    /// <summary>
    /// Metodo para la regla especial
    /// </summary>
    void ReglaEspecial() // Victoria asegurada para el que va perdiendo por 3 puntos
    {
        diferenciaPuntuacion = puntuacionP1 - puntuacionP2; // Operacion para calcular la diferencia de puntos
        
        if (diferenciaPuntuacion >= 3) // Condicional para que se cumpla la regla especial para el jugador 2
        {
            player_2 = true;
            player_1 = false;
        }

        if (diferenciaPuntuacion <= -3) // Condicional para que se cumpla la regla especial para el jugador 2
        {
            player_1 = true;
            player_2 = false;
        }
    }

    /// <summary>
    /// Metodo que muestra de forma simple al jugador en turno. Requiere un GameObject en pantalla como indicador
    /// </summary>
    void AnuncioTurnoJugador()
    {
        // Indicador por esfera para turno del jugador 1
        if (player_1)
            eP1.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        else
            eP1.GetComponent<Renderer>().material.SetColor("_Color", Color.white);

        // Indicador por esfera para turno del jugador 2
        if (player_2)
            eP2.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        else
            eP2.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
    }

    /// <summary>
    /// Metodo que muestra de forma simple el puntaje de 2 jugadores. Requiere un Cuadro de texto para mostrar los cambios y una variable estatica para que no vuelva a ser cero con el reset
    /// </summary>
    void Marcador2Jugadores()
    {
        puntajeP1.text = puntuacionP1.ToString(); // Linea que convierte el numero en texto para el marcador del jugador 1
        puntajeP2.text = puntuacionP2.ToString(); // Linea que convierte el numero en texto para el marcador del jugador 2
    }

    /// <summary>
    /// Metodo que convierte la esfera que se le clickea en su color correspondiente
    /// </summary>
    /// <param name="position"></param>
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
                    go.GetComponent<Renderer>().material.SetColor("_Color", Color.red); // Mantiene la esfera de color rojo si antes era de ese color

                }
                if (check[_x, _y] == 2)
                {
                    go.GetComponent<Renderer>().material.SetColor("_Color", Color.blue); // Mantiene la esfera de color azul si antes era de ese color

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

    /// <summary>
    ///  Metodo que se encarga de seleccionar la esfera y pintarla del color del jugador en turno
    /// </summary>
    /// <param name="position"></param>
    void SelectorEsfera(Vector3 position)
    {
        int x = (int)(position.x + 0.5f); // Valor de x aproximado al siguiente entero
        int y = (int)(position.y + 0.5f); // Valor de y aproximado al siguiente entero

        if (player_1 && check[x, y] == 0) // Turno del jugador 1
        {
            if (Input.GetMouseButtonDown(0)) // Recibe la entrada de un click izquierdo del mouse
            {
                GameObject go = grid[x, y]; // Se ubica en la posicion de una esfera
                go.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                check[x, y] = 1;
                player_1 = false; // Cambio de turno. True significa el siguiente turno
                player_2 = true;
            }
           
        }

        if (player_2 && check[x, y] == 0) // Turno del jugador 2
        {
            if (Input.GetMouseButtonDown(0)) // Recibe la entrada de un click izquierdo del mouse
            {
                GameObject go = grid[x, y]; // Se ubica en la posicion de una esfera
                go.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                check[x, y] = 2;
                player_1 = true; // Cambio de turno. True significa el siguiente turno
                player_2 = false;
            }
            
        }
    }

    /// <summary>
    /// Metodo que realiza el analisis oblicuo (derecha -> izquierda) de la tabla
    /// </summary>
    void ComparadorOblicuo1()
    {
        bool sCreciente; // Variable que hace de interruptor de aumento o decremento
        int nIteraciones; // Variable que aumenta o disminuye para ajustarse al numero de esferas en la diagonal
        int xI, yI, x, y, n; // Variables que son los terminos de la ecuacion
        xI = 4; //xF = 10; // Valor inicial y final del barrido en X
        yI = 10; //yF = 4; // Valor inicial y final del barrido en y
        n = 0;
        sCreciente = true;
        nIteraciones = 4;
        x = (xI - 1) - n; // ecuacion para el primer analisis diagonal para x
        y = (yI - 1) - n; // ecuacion para el primer analisis diagonal para y

        for (int i = 0; i < 13; i++) // Numero de diagonales en el analisis
        {
            x = (xI - 1) - n; // Los valores de "x" y "y" deben ser actualizados, ya que C# almacena numeros y no ecuaciones.
            y = (yI - 1) - n; // Posteriores cambios de sus terminos no influyen en la variable que representa la ecuacion.

            // Condicionales que se ajustan al numero de esferas en las diagonales
            if (nIteraciones == 4) // Activa el bool para aumentar el numero de iteraciones
                sCreciente = true;
            if (nIteraciones == 10) // Desactiva el bool para disminuir el numero de iteraciones
                sCreciente = false;

            for (int j = 0; j < nIteraciones; j++) // ciclo que analiza toda una diagonal
            {
                x = (xI - 1) - n; // Los valores de "x" y "y" deben ser actualizados, ya que C# almacena numeros y no ecuaciones.
                y = (yI - 1) - n; // Posteriores cambios de sus terminos no influyen en la variable que representa la ecuacion.
                if (!player_1 && check[x, y] == 1) // Condicional para contar las esferas del jugador 1
                {
                    contadorRojo = contadorRojo + 1;  // Linea que cuenta el numero de esferas rojas
                    if (contadorRojo == 4) // Condicional que ayuda a saber si el jugador gano
                    {
                        break; // Rompe el ciclo si la funcion cumple su objetivo
                    }
                }
                else if (!player_1 && check[x, y] != 1) // Condicional que restablece el contador si la esfera que sigue no coincide con el color analizado
                {
                    contadorRojo = 0;
                }

                if (!player_2 && check[x, y] == 2) // Condicional para contar las esferas del jugador 2
                {
                    contadorAzul = contadorAzul + 1; // Linea que cuenta el numero de esferas azules
                    if (contadorAzul == 4) // Condicional que ayuda a saber si el jugador gano
                    {
                        break; // Rompe el ciclo si la funcion cumple su objetivo
                    }
                }
                else if (!player_2 && check[x, y] != 2) // Condicional que restablece el contador si la esfera que sigue no coincide con el color analizado
                {
                    contadorAzul = 0;
                }
                n++; // Actualizacion del termino de la ecuacion que ajusta a "x" y "y" para que coincidan con la siguiente esfera en diagonal
            }

            n = 0; // Reinicio del termino de la ecuacion

            if (sCreciente) // Condicional que ajusta un termino de la ecuacion para que haga el analisis como queremos
                xI++;
            if (!sCreciente) // Condicional que ajusta un termino de la ecuacion para que haga el analisis como queremos
                yI--;

            if (sCreciente) // Con el interruptor en True el numero de iteraciones aumenta
                nIteraciones++;
            if (!sCreciente) // Con el interruptor en False el numero de iteraciones disminuye
                nIteraciones--;

            if (contadorRojo == 4) // Condicional que indica que el jugador 1 Gano!!!
            {
                SceneManager.LoadScene(0); // Reinicio del tablero
                Debug.Log("Player 1 Wins!!!"); // Mensaje en consola para mostrar que gano el jugador 1
                puntuacionP1++; // Variable estatica para los puntos del jugador 1
                break; // Rompe el ciclo si la funcion cumple su objetivo
            }
            if (contadorAzul == 4) // Condicional que indica que el jugador 2 Gano!!!
            {
                SceneManager.LoadScene(0); // Reinicio del tablero
                Debug.Log("Player 2 Wins!!!"); // Mensaje en consola para mostrar que gano el jugador 2
                puntuacionP2++; // Variable estatica para los puntos del jugador 2
                break; // Rompe el ciclo si la funcion cumple su objetivo
            }
        }
    }

    /// <summary>
    /// Metodo que realiza el analisis oblicuo (izquierda -> derecha) de la tabla
    /// </summary>
    void ComparadorOblicuo2()
    {
        bool sCreciente; // Variable que hace de interruptor de aumento o decremento
        int nIteraciones; // Variable que aumenta o disminuye para ajustarse al numero de esferas en la diagonal
        int xI, yI, x, y, n; // Variables que son los terminos de la ecuacion
        xI = 7; //xF = 10 // Valor inicial y final del barrido en X
        yI = 10; //yF = 4 // Valor inicial y final del barrido en y
        n = 0;
        sCreciente = true;
        nIteraciones = 4;
        x = (xI - 1) + n; // Los valores de "x" y "y" deben ser actualizados, ya que C# almacena numeros y no ecuaciones.
        y = (yI - 1) - n; // Posteriores cambios de sus terminos no influyen en la variable que representa la ecuacion.

        for (int i = 0; i < 13; i++) // Numero de diagonales en el analisis
        {
            x = (xI - 1) + n; // Los valores de "x" y "y" deben ser actualizados, ya que C# almacena numeros y no ecuaciones.
            y = (yI - 1) - n; // Posteriores cambios de sus terminos no influyen en la variable que representa la ecuacion.

            // Condicionales que se ajustan al numero de esferas en las diagonales
            if (nIteraciones == 4) // Activa el bool para aumentar el numero de iteraciones
                sCreciente = true;
            if (nIteraciones == 10) // Desactiva el bool para disminuir el numero de iteraciones
                sCreciente = false;

            for (int j = 0; j < nIteraciones; j++) // ciclo que analiza toda una diagonal
            {
                x = (xI - 1) + n; // Los valores de "x" y "y" deben ser actualizados, ya que C# almacena numeros y no ecuaciones.
                y = (yI - 1) - n; // Posteriores cambios de sus terminos no influyen en la variable que representa la ecuacion.
                if (!player_1 && check[x, y] == 1) // Condicional para contar las esferas del jugador 1
                {
                    contadorRojo = contadorRojo + 1; // Linea que cuenta el numero de esferas rojas
                    if (contadorRojo == 4) // Condicional que ayuda a saber si el jugador gano
                    {
                        break; // Rompe el ciclo si la funcion cumple su objetivo
                    }
                }
                else if (!player_1 && check[x, y] != 1) // Condicional que restablece el contador si la esfera que sigue no coincide con el color analizado
                {
                    contadorRojo = 0;
                }

                if (!player_2 && check[x, y] == 2) // Condicional para contar las esferas del jugador 2
                {
                    contadorAzul = contadorAzul + 1; // Linea que cuenta el numero de esferas azules
                    if (contadorAzul == 4) // Condicional que ayuda a saber si el jugador gano
                    {
                        break; // Rompe el ciclo si la funcion cumple su objetivo
                    }
                }
                else if (!player_2 && check[x, y] != 2) // Condicional que restablece el contador si la esfera que sigue no coincide con el color analizado
                {
                    contadorAzul = 0;
                }
                n++; // Actualizacion del termino de la ecuacion que ajusta a "x" y "y" para que coincidan con la siguiente esfera en diagonal
            }

            n = 0; // Reinicio del termino de la ecuacion

            if (sCreciente) // Condicional que ajusta un termino de la ecuacion para que haga el analisis como queremos
                xI--;
            if (!sCreciente) // Condicional que ajusta un termino de la ecuacion para que haga el analisis como queremos
                yI--;

            if (sCreciente) // Con el interruptor en True el numero de iteraciones aumenta
                nIteraciones++;
            if (!sCreciente) // Con el interruptor en False el numero de iteraciones disminuye
                nIteraciones--;

            if (contadorRojo == 4) // Condicional que indica que el jugador 1 Gano!!!
            {
                SceneManager.LoadScene(0); // Reinicio del tablero
                Debug.Log("Player 1 Wins!!!"); // Mensaje en consola para mostrar que gano el jugador 1
                puntuacionP1++; // Variable estatica para los puntos del jugador 1
                break; // Rompe el ciclo si la funcion cumple su objetivo
            }
            if (contadorAzul == 4) // Condicional que indica que el jugador 2 Gano!!!
            {
                SceneManager.LoadScene(0); // Reinicio del tablero
                Debug.Log("Player 2 Wins!!!"); // Mensaje en consola para mostrar que gano el jugador 2
                puntuacionP2++; // Variable estatica para los puntos del jugador 2
                break; // Rompe el ciclo si la funcion cumple su objetivo
            }

        }
    }

    /// <summary>
    /// Metodo que realiza el analisis vertical de la tabla
    /// </summary>
    void ComparadorVertical()
    {
        for (int x = 0; x < width; x++) // Ciclo for anidado sube -->
        {
            for (int y = 0; y < height; y++) // Del ciclo for crea y posiciona las esferas
            {
                if(!player_1 && check[x, y] == 1) // Condicional para contar las esferas del jugador 1
                {
                    contadorRojo = contadorRojo + 1; // Linea que cuenta el numero de esferas rojas
                    if (contadorRojo == 4) // Condicional que ayuda a saber si el jugador gano
                    {
                        break; // Rompe el ciclo si la funcion cumple su objetivo
                    }
                }
                else if(!player_1 && check[x, y] != 1) // Condicional que restablece el contador si la esfera que sigue no coincide con el color analizado
                {
                    contadorRojo = 0;
                }

                if (!player_2 && check[x, y] == 2) // Condicional para contar las esferas del jugador 2
                {
                    contadorAzul = contadorAzul + 1; // Linea que cuenta el numero de esferas azules
                    if (contadorAzul == 4) // Condicional que ayuda a saber si el jugador gano
                    {
                        break; // Rompe el ciclo si la funcion cumple su objetivo
                    }
                }
                else if(!player_2 && check[x, y] != 2) // Condicional que restablece el contador si la esfera que sigue no coincide con el color analizado
                {
                    contadorAzul = 0;
                }
            }
            if (contadorRojo == 4) // Condicional que indica que el jugador 1 Gano!!!
            {
                SceneManager.LoadScene(0); // Reinicio del tablero
                Debug.Log("Player 1 Wins!!!"); // Mensaje en consola para mostrar que gano el jugador 1
                puntuacionP1++; // Variable estatica para los puntos del jugador 1
                break; // Rompe el ciclo si la funcion cumple su objetivo
            }
            if (contadorAzul == 4) // Condicional que indica que el jugador 2 Gano!!!
            {
                SceneManager.LoadScene(0); // Reinicio del tablero
                Debug.Log("Player 2 Wins!!!"); // Mensaje en consola para mostrar que gano el jugador 2
                puntuacionP2++; // Variable estatica para los puntos del jugador 2
                break; // Rompe el ciclo si la funcion cumple su objetivo
            }
        }
        

        
    }

    /// <summary>
    /// Metodo que realiza el analisis horizontal de la tabla
    /// </summary>
    void ComparadorHorizontal(int[,] check) // El parametro incluye las variables width y height
    {
        for (int x = 0; x < width; x++) // Ciclo for anidado sube -->
        {
            for (int y = 0; y < height; y++) // Del ciclo for crea y posiciona las esferas
            {
                if (!player_1 && check[y, x] == 1) // Condicional para contar las esferas del jugador 1
                {
                    contadorRojo = contadorRojo + 1; // Linea que cuenta el numero de esferas rojas
                    if (contadorRojo == 4)
                    {
                        break; // Rompe el ciclo si la funcion cumple su objetivo
                    }
                }
                else if (!player_1 && check[y, x] != 1) // Condicional que restablece el contador si la esfera que sigue no coincide con el color analizado
                {
                    contadorRojo = 0;
                }

                if (!player_2 && check[y, x] == 2) // Condicional para contar las esferas del jugador 2
                {
                    contadorAzul = contadorAzul + 1; // Linea que cuenta el numero de esferas azules
                    if (contadorAzul == 4)
                    {
                        break; // Rompe el ciclo si la funcion cumple su objetivo
                    }
                }
                else if (!player_2 && check[y, x] != 2) // Condicional que restablece el contador si la esfera que sigue no coincide con el color analizado
                {
                    contadorAzul = 0;
                }
            }
            if (contadorRojo == 4)
            {
                SceneManager.LoadScene(0); // Reinicio del tablero
                Debug.Log("Player 1 Wins!!!"); // Mensaje en consola para mostrar que gano el jugador 1
                puntuacionP1++; // Variable estatica para los puntos del jugador 1
                break; // Rompe el ciclo si la funcion cumple su objetivo
            }
            if (contadorAzul == 4)
            {
                SceneManager.LoadScene(0); // Reinicio del tablero
                Debug.Log("Player 2 Wins!!!"); // Mensaje en consola para mostrar que gano el jugador 2
                puntuacionP2++; // Variable estatica para los puntos del jugador 2
                break; // Rompe el ciclo si la funcion cumple su objetivo
            }
        }
    }
}