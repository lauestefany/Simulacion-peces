using UnityEngine;

namespace EstanqueDePeces
{
    public class AgentePez : MonoBehaviour
    {
        private SpriteRenderer sr;
        // Límite mínimo del área donde puede nadar el pez
        private Vector2 minLim = new Vector2(-6.5f, -4.0f);
        private Vector2 maxLim = new Vector2(6.5f, 0.2f);

        private Vector2 destino;
        private float velocidad;
        private float tiempoCambio;
        private float desfase;

        private void Awake()
        {
            // El SpriteRenderer
            sr = GetComponent<SpriteRenderer>();

            if (sr == null)
            {
                Debug.LogError("El Prefab del pez necesita un SpriteRenderer.");
                return;
            }

            desfase = Random.Range(0f, Mathf.PI * 2f);
            velocidad = Random.Range(1.3f, 2.3f);

            // Orden de dibujo
            sr.sortingOrder = 10;

            // Tamaño del pez
            transform.localScale = Vector3.one * 0.3f;
        }
        // Este método recibe los límites del estanque y se llama desde simulacion estanque
        public void Configurar(Vector2 min, Vector2 max)
        {
            minLim = min;
            maxLim = max;

            NuevoDestino();
        }

        private void Update()
        {
            if (Time.time >= tiempoCambio ||
                Vector2.Distance(transform.position, destino) < 0.4f)
            {
                NuevoDestino();
            }

            Vector2 pos = transform.position;
            Vector2 dir = (destino - pos).normalized;

            // Movimiento del pez
            transform.position = Vector2.MoveTowards(
                pos,
                destino,
                velocidad * Time.deltaTime
            );

            // Ondulación
            float ondulacion =
                Mathf.Sin((Time.time * 8f) + desfase) * 5f;

            // Girar hacia donde se mueve
            if (Mathf.Abs(dir.x) > 0.05f)
            {
                bool izq = dir.x < 0;

                sr.flipX = izq;

                float angulo =
                    Mathf.Atan2(
                        dir.y,
                        Mathf.Abs(dir.x)
                    ) * Mathf.Rad2Deg;

                if (izq)
                    angulo *= -1f;

                transform.rotation =
                    Quaternion.Euler(
                        0,
                        0,
                        angulo + ondulacion
                    );
            }
        }
        // Este método selecciona una nueva posición aleatoria
        private void NuevoDestino()
        {
            float x = Random.Range(minLim.x, maxLim.x);
            float y = Random.Range(minLim.y, maxLim.y);

            destino = new Vector2(x, y);

            tiempoCambio =
                Time.time + Random.Range(2.5f, 5.0f);

            velocidad =
                Random.Range(1.2f, 2.2f);
        }
    }
}