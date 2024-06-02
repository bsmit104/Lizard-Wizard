using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    private float moveSpeed; // Randomized movement speed
    private float moveDistance; // Randomized movement distance
    private Vector3 startPosition;
    private Vector3 endPosition; // End position for movement
    private Vector3 currentPosition; // Current position of the platform
    private bool isMovingForward; // Flag to indicate the direction of movement
    private bool isEngulfed = false;

    public float minSpeed = 2f; // Minimum movement speed
    public float maxSpeed = 8f; // Maximum movement speed
    public float minDistance = 2f; // Minimum movement distance
    public float maxDistance = 8f; // Maximum movement distance

    void Start()
    {
        startPosition = transform.position;
        RandomizeMovementParams();
        RandomizeDirection();
        CalculateEndPosition();
        currentPosition = startPosition;
    }

    void Update()
    {
        if (isEngulfed)
        {
            // Calculate movement
            if (isMovingForward)
            {
                currentPosition = Vector3.MoveTowards(currentPosition, endPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                currentPosition = Vector3.MoveTowards(currentPosition, startPosition, moveSpeed * Time.deltaTime);
            }

            transform.position = currentPosition;

            // If movement reaches the end, change direction
            if (Vector3.Distance(transform.position, endPosition) < 0.01f || Vector3.Distance(transform.position, startPosition) < 0.01f)
            {
                isMovingForward = !isMovingForward;
            }
        }
    }

    void CalculateEndPosition()
    {
        // Calculate end position for movement
        endPosition = startPosition + (isMovingForward ? Vector3.right : -Vector3.right) * moveDistance;
    }

    void RandomizeMovementParams()
    {
        // Randomize movement speed and distance within the specified ranges
        moveSpeed = Random.Range(minSpeed, maxSpeed);
        moveDistance = Random.Range(minDistance, maxDistance);
    }

    void RandomizeDirection()
    {
        // Randomize initial direction of movement
        isMovingForward = Random.value >= 0.5f;
    }

    public void Engulf()
    {
        if (!isEngulfed)
        {
            isEngulfed = true;
            startPosition = transform.position; // Update startPosition to the current position
            RandomizeMovementParams(); // Randomize movement parameters
            RandomizeDirection(); // Randomize initial direction of movement
            CalculateEndPosition(); // Recalculate end position
            currentPosition = startPosition; // Reset current position
        }
    }

    public void ResetMovement() {
        isEngulfed = false;
    }
}