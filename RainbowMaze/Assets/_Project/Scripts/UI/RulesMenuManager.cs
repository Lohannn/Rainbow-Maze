using UnityEngine;
using UnityEngine.UI;

public class RulesMenuManager : MonoBehaviour
{
    [SerializeField] private Image darkPanelImage; // Arraste a Image aqui
    [SerializeField] private RectTransform rulePanelRect; // Arraste o RectTransform aqui
    [SerializeField] private float speed = 5f;
    [SerializeField] private float closeLimiar = 5f;

    private bool canMove = false;
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    private Color originalColor;
    private float targetOpacity;

    [SerializeField] private GridManager grid;
    [SerializeField] private Player player;

    private void Start()
    {
        initialPosition = rulePanelRect.localPosition;
        originalColor = darkPanelImage.color;
    }

    private void Update()
    {
        if (!canMove) return;

        float currentAlpha = darkPanelImage.color.a;
        float newAlpha = Mathf.MoveTowards(currentAlpha, targetOpacity, Time.deltaTime * speed);
        darkPanelImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);

        rulePanelRect.localPosition = Vector3.Lerp(rulePanelRect.localPosition, targetPosition, Time.deltaTime * speed);

        if (Vector3.Distance(rulePanelRect.localPosition, targetPosition) < closeLimiar && Mathf.Abs(newAlpha - targetOpacity) < closeLimiar)
        {
            rulePanelRect.localPosition = targetPosition;
            darkPanelImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, targetOpacity);
            canMove = false;

            if (targetPosition == initialPosition)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Open()
    {
        player.CanMove = false;
        gameObject.SetActive(true);
        targetPosition = Vector2.zero;
        targetOpacity = 0.85f;
        canMove = true;
    }

    public void Close()
    {
        targetPosition = initialPosition;
        targetOpacity = 0f;
        canMove = true;
        player.CanMove = true;
    }

    public void ResetPosition()
    {
        grid.ResetPlayerPosition();
        Close();
    }
}