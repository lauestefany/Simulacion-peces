using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace EstanqueDePeces
{
    public class SimulacionEstanque : MonoBehaviour
    {
        // CONDICIONES INICIALES
      
        [Header("Condiciones iniciales del caso (Po, r, C)")]

        // Población inicial de peces
        public float poblacionInicial = 100f;

        // Tasa de reproducción
        // 0.10 representa el 10%
        public float tasaReproduccion = 0.10f;

        // Cantidad de peces que se pescan cada día
        public float tasaPesca = 2f;

        // Tiempo que representa un día dentro de la simulación
        public float tiempoEntreDias = 1.5f;


        // CONFIGURACIÓN DE LA SIMULACIÓN
       
        [Header("Configuración de la simulación")]

        // La simulación termina en el día 10
        public int diaMaximo = 10;

        
        // CONFIGURACIÓN VISUAL DEL ESTANQUE
        
        [Header("Configuración visual del estanque")]

        // Límite mínimo donde pueden aparecer los peces
        public Vector2 limitesAguaMin =
            new Vector2(-6.5f, -4.0f);

        // Límite máximo donde pueden aparecer los peces
        public Vector2 limitesAguaMax =
            new Vector2(6.5f, 0.4f);

        // PREFAB DEL PEZ
      
        [Header("Prefab del pez")]

        // Aquí se asigna el Prefab del pez desde Unity
        public GameObject prefabPez;

        // VARIABLES DE ESTADO

        private float poblacionActual;
        private int diaActual;
        private float cronometro;

        // Indica si el estanque quedó vacío.
        private bool estanqueVacio;
        private bool simulacionTerminada;


        // LISTA DE PECES VISUALES
       

        // Aquí guardo todos los peces que aparecen en pantalla.
        private readonly List<GameObject> pecesVisuales =
            new List<GameObject>();


        // Texto que muestra la información de la simulación.
        private Text txtInfo;

        // Población con la que comienza cada día.
        private float poblacionInicioDia;
        private float pecesDespuesPesca;
        private float nuevosIndividuos;


        // INICIO

        private void Start()
        {
            // ESTADO INICIAL
        

            // Comienzo con 100 peces
            poblacionActual = poblacionInicial;

            // El día 0 representa el estado inicial
            diaActual = 0;

            // Inicio el cronómetro en cero
            cronometro = 0f;

            // El estanque comienza con peces
            estanqueVacio = false;

            // La simulación todavía no ha terminado
            simulacionTerminada = false;





            // Creo el texto que aparecerá en pantalla
            CrearTextoInfo();


            // Muestro los peces iniciales
            DrawView();

            // MOSTRAR DÍA 0 EN LA CONSOLA
      

            Debug.Log(
                "========================================"
            );

            Debug.Log(
                $"Día 0 | " +
                $"Población: {poblacionActual:F2} | " +
                $"Peces visibles: {pecesVisuales.Count}"
            );

            Debug.Log(
                "========================================"
            );
        }

        // ACTUALIZACIÓN

        private void Update()
        {
            // Si el estanque está vacío, no continúo
            if (estanqueVacio)
                return;

            // Si llegué al día máximo, no continúo
            if (simulacionTerminada)
                return;


            // Acumulo el tiempo transcurrido
            cronometro += Time.deltaTime;


            // Cuando pasa el tiempo establecido comienza un nuevo día
            if (cronometro >= tiempoEntreDias)
            {
                // Reinicio el cronómetro.
                cronometro = 0f;


                // Realizo los cálculos del nuevo día
                Simulate();


                // Actualizo la cantidad de peces visibles
                DrawView();
            }
        }
        private void Simulate()
        {
            // AVANZAR UN DÍA
           

            diaActual++;  

            poblacionInicioDia =
                poblacionActual;


            
            // 1. PESCA
           
            pecesDespuesPesca =
                Mathf.Max(
                    0f,
                    poblacionActual - tasaPesca
                );
            // 2. REPRODUCCIÓN
            

            // Se calcula el 10% de los peces que quedaron después de la pesca

        
            nuevosIndividuos =
                pecesDespuesPesca *
                tasaReproduccion;


            // 3. NUEVA POBLACIÓN

            poblacionActual =
                pecesDespuesPesca +
                nuevosIndividuos;


          
            // MOSTRAR CÁLCULOS EN LA CONSOLA

            Debug.Log(
                $"Día {diaActual} | " +
                $"Población inicial: {poblacionInicioDia:F2} | " +
                $"Pesca: {tasaPesca:F0} | " +
                $"Después de pesca: {pecesDespuesPesca:F2} | " +
                $"Nuevos peces: {nuevosIndividuos:F2} | " +
                $"Población final: {poblacionActual:F2}"
            );


            // COMPROBAR SI EL ESTANQUE QUEDÓ VACÍO
           

            if (poblacionActual <= 0f)
            {
                // Evito valores negativos.
                poblacionActual = 0f;

                // Marco el estanque como vacío.
                estanqueVacio = true;

                Debug.Log(
                    "El estanque quedó vacío."
                );

                return;
            }


            // COMPROBAR FINAL DE LA SIMULACIÓN

            if (diaActual >= diaMaximo)
            {
                // Detengo la simulación.
                simulacionTerminada = true;


                Debug.Log(
                    "========================================"
                );

                Debug.Log(
                    $"SIMULACIÓN TERMINADA EN EL DÍA " +
                    $"{diaActual}"
                );

                Debug.Log(
                    $"Población final: " +
                    $"{poblacionActual:F2}"
                );

                Debug.Log(
                    "========================================"
                );
            }
        }

        // ACTUALIZAR VISTA
        private void DrawView()
        {
            // 1. ELIMINAR LOS PECES ANTERIORES
           

            LimpiarTodosLosPeces();


            // 2. CALCULAR CUÁNTOS PECES MOSTRAR
           

            int cantidadObjetivo =
                CalcularCantidadVisual(
                    poblacionActual
                );


           
            // 3. CREAR LOS NUEVOS PECES
          

            for (
                int i = 0;
                i < cantidadObjetivo;
                i++
            )
            {
                CrearPezVisual();
            }

            // 4. ACTUALIZAR INFORMACIÓN EN PANTALLA
           
            if (txtInfo != null)
            {
                string estado = "";


                if (estanqueVacio)
                {
                    estado = " | ESTANQUE VACÍO";
                }
                else if (simulacionTerminada)
                {
                    estado = " | SIMULACIÓN TERMINADA";
                }


                txtInfo.text =
                    $"Día: {diaActual} / {diaMaximo} | " +
                    $"Población: {poblacionActual:F2} | " +
                    $"Peces visibles: {pecesVisuales.Count}" +
                    estado;
            }
        }


        // ELIMINAR TODOS LOS PECES VISUALES

        private void LimpiarTodosLosPeces()
        {
            // Recorro la lista de peces.
            for (
                int i = pecesVisuales.Count - 1;
                i >= 0;
                i--
            )
            {
                GameObject pez =
                    pecesVisuales[i];


                // Si el pez existe, lo elimino.
                if (pez != null)
                {
                    Destroy(pez);
                }
            }


            // Limpio la lista.
            pecesVisuales.Clear();
        }


        // CREAR TEXTO DE INFORMACIÓN

        private void CrearTextoInfo()
        {
            // Creo un Canvas para mostrar información
            GameObject canvasObj =
                new GameObject("CanvasInfo");


            canvasObj.transform.SetParent(
                transform
            );


            // Agrego el componente Canvas.
            Canvas canvas =
                canvasObj.AddComponent<Canvas>();


            canvas.renderMode =
                RenderMode.ScreenSpaceOverlay;


            canvas.sortingOrder = 50;

            CanvasScaler scaler =
                canvasObj.AddComponent<CanvasScaler>();


            scaler.uiScaleMode =
                CanvasScaler.ScaleMode.ScaleWithScreenSize;


            scaler.referenceResolution =
                new Vector2(
                    1920,
                    1080
                );


            scaler.matchWidthOrHeight =
                0.5f;


            // BARRA
       

            GameObject barra =
                new GameObject("Barra");


            barra.transform.SetParent(
                canvasObj.transform,
                false
            );


            RectTransform rt =
                barra.AddComponent<RectTransform>();


            rt.anchorMin =
                new Vector2(
                    0f,
                    1f
                );


            rt.anchorMax =
                new Vector2(
                    1f,
                    1f
                );


            rt.pivot =
                new Vector2(
                    0.5f,
                    1f
                );


            rt.anchoredPosition =
                new Vector2(
                    0f,
                    -10f
                );


            rt.sizeDelta =
                new Vector2(
                    -40f,
                    48f
                );


            
            // FONDO DE LA BARRA

            Image img =
                barra.AddComponent<Image>();


            img.color =
                new Color(
                    0.12f,
                    0.12f,
                    0.12f,
                    0.85f
                );

            // TEXTO

            GameObject textoObj =
                new GameObject("TextoInfo");


            textoObj.transform.SetParent(
                barra.transform,
                false
            );


            RectTransform rtTxt =
                textoObj.AddComponent<RectTransform>();


            rtTxt.anchorMin =
                Vector2.zero;


            rtTxt.anchorMax =
                Vector2.one;


            rtTxt.offsetMin =
                new Vector2(
                    20f,
                    0f
                );


            rtTxt.offsetMax =
                new Vector2(
                    -20f,
                    0f
                );


            // Creo el componente Text.
            txtInfo =
                textoObj.AddComponent<Text>();


            // Uso una fuente incluida en Unity.
            txtInfo.font =
                Resources.GetBuiltinResource<Font>(
                    "LegacyRuntime.ttf"
                );


            txtInfo.fontSize = 18;


            txtInfo.fontStyle =
                FontStyle.Bold;


            txtInfo.color =
                Color.white;


            txtInfo.alignment =
                TextAnchor.MiddleLeft;
        }


       
        // CALCULAR PECES VISIBLES
        

        private int CalcularCantidadVisual(
            float poblacion
        )
        {
            // Si no hay peces, no creo ninguno
            if (poblacion <= 0f)
                return 0;


            // Convierto la población decimal en una cantidad entera de peces

            int cantidad =
                Mathf.RoundToInt(
                    poblacion
                );


            return Mathf.Max(
                1,
                cantidad
            );
        }

        // CREAR UN PEZ
      
        private void CrearPezVisual()
        {
            // Compruebo que exista el Prefab.
            if (prefabPez == null)
            {
                Debug.LogError(
                    "No se ha asignado el Prefab del pez " +
                    "en SimulacionEstanque."
                );

                return;
            }
            // POSICIÓN ALEATORIA
            

            float x =
                Random.Range(
                    limitesAguaMin.x + 0.5f,
                    limitesAguaMax.x - 0.5f
                );


            float y =
                Random.Range(
                    limitesAguaMin.y + 0.5f,
                    limitesAguaMax.y - 0.5f
                );


            // CREAR PEZ
           

            GameObject obj =
                Instantiate(
                    prefabPez,
                    new Vector3(
                        x,
                        y,
                        0f
                    ),
                    Quaternion.identity,
                    transform
                );


            // CONFIGURAR MOVIMIENTO

            AgentePez agente =
                obj.GetComponent<AgentePez>();


            if (agente != null)
            {
                // Le envío los límites del estanque.
                agente.Configurar(
                    limitesAguaMin,
                    limitesAguaMax
                );
            }
            else
            {
                Debug.LogError(
                    "El Prefab Pez no tiene el componente " +
                    "AgentePez."
                );
            }


            // Guardo el pez en la lista.
            pecesVisuales.Add(obj);
        }
    }
}