using UnityEngine;

public class CanvasFollowObject : MonoBehaviour
{
    public Transform targetObject; // Obiekt, za którym chcemy podążać
    public Vector2 offset; // Przesunięcie względem obiektu docelowego

    private RectTransform canvasRectTransform;

    private void Start()
    {
        canvasRectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (targetObject != null)
        {
            // Pobierz pozycję obiektu docelowego
            Vector3 targetPosition = targetObject.position + new Vector3(offset.x, offset.y, 0f);

            // Przekształć pozycję na ekranową przestrzeń
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(targetPosition);

            // Ustaw nową pozycję dla obiektu Canvas z wartością Z = 3
            canvasRectTransform.position = new Vector3(screenPosition.x, screenPosition.y, 3f);
        }
    }
}