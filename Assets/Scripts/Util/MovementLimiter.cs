using UnityEngine;

public class MovementLimiter : MonoBehaviour
{
    [SerializeField] private GameObject limited;
    private bool isActive = false;
    private float maxXPosition;

    public void SetLimit(float maxX)
    {
        isActive = true;
        maxXPosition = maxX;
    }

    private void Update()
    {
        if (!isActive) return;
        if (limited.transform.position.x <= maxXPosition) return;
        limited.transform.position = new Vector3(maxXPosition, limited.transform.position.y, limited.transform.position.z);
    }
}