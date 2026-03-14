using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Touch/Swipe")]
    [SerializeField] private float swipeSensitivity = 50f;
    [SerializeField] private float minimumSwipeTime = 0.1f;

    private Touch touch;
    private Vector2 touchStartPosition;
    private Vector2 touchEndPosition;
    private float currentSwipeTime;

    //PlayerController player;

    private void Awake()
    {
        //player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        //Caso não tenha nenhum toque na tela, não faça nada (Otimização)
        if (Input.touchCount == 0) return;

        Swipe();
    }

    private void Swipe()
    {
        //Pega apenas o primeiro toque da tela
        touch = Input.GetTouch(0);

        if(touch.phase == TouchPhase.Began)
        {
            //Quando um toque começar, salva a posição inicial
            touchStartPosition = touch.position;
            currentSwipeTime = 0 + Time.deltaTime; //Reinicia o tempo do swipe para o novo toque
        }
        else if(touch.phase == TouchPhase.Moved)
        {
            //Enquanto o toque estiver na tela e se movendo, salva a posição final
            touchEndPosition = touch.position;
            currentSwipeTime += Time.deltaTime; //Acumula o tempo do swipe enquanto estiver se movendo
        }
        else if(touch.phase == TouchPhase.Ended)
        {
            //Quando o toque terminar, calcula a direção do swipe
            Vector2 swipeDirection = touchEndPosition - touchStartPosition;

            //Verifica se a magnitude (o comprimento) do swipe foi maior que o valor de sensibilidade
            //e se o toque durou por tempo o suficiente para ser considerado um swipe,
            //assim evitando que um toque rápido não gere movimento
            if (swipeDirection.magnitude > swipeSensitivity && currentSwipeTime > minimumSwipeTime)
            {
                if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                {
                    //MOVIMENTO HORIZONTAL

                    if (touchStartPosition.x < touchEndPosition.x)
                    {
                        print("Direita");
                    }
                    else
                    {
                        print("Esquerda");
                    }
                }
                else
                {
                    //MOVIMENTO VERTICAL

                    if (touchStartPosition.y < touchEndPosition.y)
                    {
                        print("Cima");
                    }
                    else
                    {
                        print("Baixo");
                    }
                }
            }
            else
            {
                print("Deslizou não ein");
            }
        }
    }
}
