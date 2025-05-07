using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [Header("Movement")] [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 30f;

    [Header("Perlin Noise")] [SerializeField]
    private float perlinNoiseScale = 0.5f;

    [SerializeField] private float noiseStrength = 1f;
    [SerializeField] private float noiseSampleSpeed = 0.5f;
    private float noiseOffset;

    [Header("Bounds")] 
    [SerializeField] private float maxXRadius = 5f;

    private Vector3 _currentVelocity;
    private Vector3 _lastPosition;
    private Vector3 _closestPoint;

    public Vector3 CurrentVelocity => _currentVelocity;

    private void Start()
    {
        noiseOffset = Random.Range(0f, 100f);
    }

    private void Update()
    {
        _currentVelocity = (transform.position - _lastPosition) / Time.fixedDeltaTime;
        _lastPosition = transform.position;
        
        transform.Translate(Vector3.forward * (moveSpeed * Time.deltaTime));

        float perlinValue = Mathf.PerlinNoise(noiseOffset, Time.time * noiseSampleSpeed);

        float turnDirection = (perlinValue * 2f - 1f) * noiseStrength;

        float distanceFromCenter = Mathf.Abs(transform.position.x);
        float distanceRatio = distanceFromCenter / maxXRadius;

        float directionToCenter = transform.position.x > 0 ? -1f : 1f;

        if (distanceRatio > 0)
        {
            turnDirection *= (1f - distanceRatio * 0.9f);

            if (Mathf.Sign(turnDirection) != directionToCenter)
            {
                turnDirection *= (1f - distanceRatio);

                if (distanceRatio > 0.8f)
                {
                    turnDirection += directionToCenter * (distanceRatio - 0.8f) * 5f;
                }
            }
        }

        transform.Rotate(0f, turnDirection * rotationSpeed * Time.deltaTime, 0f);

        if (Mathf.Abs(transform.position.x) > maxXRadius)
        {
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, -maxXRadius, maxXRadius);
            transform.position = clampedPosition;

            float forcedTurn = transform.position.x > 0 ? -1f : 1f;
            transform.Rotate(0f, forcedTurn * rotationSpeed * Time.deltaTime * 2f, 0f);
        }

        noiseOffset += noiseSampleSpeed * Time.deltaTime;
    }

    public Vector3 GetNearestPointTo(Vector3 position)
    {
        _closestPoint = _collider.ClosestPoint(position);
        return _closestPoint;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-maxXRadius, 0, transform.position.z - 7), new Vector3(-maxXRadius, 0, transform.position.z + 7));
        Gizmos.DrawLine(new Vector3(maxXRadius, 0, transform.position.z - 7), new Vector3(maxXRadius, 0, transform.position.z + 7));
        
        Gizmos.DrawSphere(_closestPoint, 0.2f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(-maxXRadius * 0.8f, 0, transform.position.z - 7),
            new Vector3(-maxXRadius * 0.8f, 0, transform.position.z + 7));
        Gizmos.DrawLine(new Vector3(maxXRadius * 0.8f, 0, transform.position.z - 7),
            new Vector3(maxXRadius * 0.8f, 0, transform.position.z + 7));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(-maxXRadius * 0.5f, 0, transform.position.z - 7),
            new Vector3(-maxXRadius * 0.5f, 0, transform.position.z + 7));
        Gizmos.DrawLine(new Vector3(maxXRadius * 0.5f, 0, transform.position.z - 7),
            new Vector3(maxXRadius * 0.5f, 0, transform.position.z + 7));
    }
}