using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public UnityEvent PullRecord;
    public float jumpForce = 5f; // Adjust the jump force as needed
    public bool isGrounded; // Flag to check if the player is grounded

    [SerializeField]
    private LineRenderer lineRenderer;

    private bool isIdle;
    private bool isAiming;

    // Flag to indicate whether the game has started

    private Rigidbody rigidbody1;

    [SerializeField]
    private float stopVelocity = .05f;

    [SerializeField]
    private float shotPower;

    public Vector3 checkpointPos;

    CanvasIntoManager canvas;

    public bool started = false;

    private void Awake()
    {
        rigidbody1 = GetComponent<Rigidbody>();

        isAiming = false;

        lineRenderer.enabled = false;

        PullRecord.AddListener(
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().Pull
        );

        PullRecord.AddListener(
            GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>().CountPull
        );

        canvas = GameObject.FindGameObjectWithTag("UIController").GetComponent<CanvasIntoManager>();
    }

    void Start()
    {
        checkpointPos = transform.position;
    }

    private void Update()
    {
        if (rigidbody1.velocity.magnitude < stopVelocity)
            Stop();

        ProcessAim();

        if (Input.GetKey(KeyCode.R))
        {
            Die();
        }

        // if (canvas.gameStarted)
        // {
        //     started = canvas.gameStarted;
        //     print("game starsts!!");
        // }
    }

    // Method to start the game

    private void Stop()
    {
        rigidbody1.velocity = Vector3.zero;
        rigidbody1.angularVelocity = Vector3.zero;
        isIdle = true;
    }

    private void OnMouseDown()
    {
        if (isIdle && canvas.gameStarted)
            isAiming = true;
    }

    private void ProcessAim()
    {
        if (!isIdle || !canvas.gameStarted)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit[] hits;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                Debug.Log("Hit object: " + hit.collider.gameObject.name);
            }

            if (hits.Length > 0)
            {
                // Assuming you only want to aim if the player is hit by the raycast
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        isAiming = true;
                        break; // Exit the loop once the player is found
                    }
                }
            }
        }

        if (isAiming)
        {
            Vector3? worldPoint = CastMouseClickRay();

            if (!worldPoint.HasValue)
                return;

            DrawLine(worldPoint.Value);

            if (Input.GetMouseButtonUp(0))
            {
                Shoot(worldPoint.Value);
            }
        }
    }

    private void Shoot(Vector3 worldPoint)
    {
        isAiming = false;
        lineRenderer.enabled = false;

        Vector3 horizontalWorldPoint = new Vector3(
            worldPoint.x,
            transform.position.y,
            worldPoint.z
        );

        Vector3 direction = horizontalWorldPoint - transform.position;
        direction.Normalize();

        float strength = Vector3.Distance(transform.position, horizontalWorldPoint);

        rigidbody1.AddForce(direction * strength * shotPower);

        PullRecord.Invoke();
    }

    private void Jump()
    {
        rigidbody1.velocity = new Vector3(rigidbody1.velocity.x, 0f, rigidbody1.velocity.z);
        rigidbody1.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void DrawLine(Vector3 worldPoint)
    {
        Vector3[] positions = { transform.position, worldPoint };

        lineRenderer.SetPositions(positions);
        lineRenderer.enabled = true;
    }

    private Vector3? CastMouseClickRay()
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane
        );

        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane
        );

        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        RaycastHit hit;

        if (
            Physics.Raycast(
                worldMousePosNear,
                worldMousePosFar - worldMousePosNear,
                out hit,
                float.PositiveInfinity
            )
        )
        {
            return hit.point;
        }
        else
        {
            return null;
        }
    }

    public void Die()
    {
        StartCoroutine(Respawn(0.5f));
    }

    IEnumerator Respawn(float duration)
    {
        yield return new WaitForSeconds(duration);
        rigidbody1.velocity = Vector2.zero;
        transform.position = checkpointPos;
    }

    public void UpdateCheckpoint(Vector3 position)
    {
        checkpointPos = position;
    }

    // Check if the player is grounded
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}