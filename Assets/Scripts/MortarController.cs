using System.Collections;
using UnityEngine;

public class MortarController : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float elevationSpeed = 50f;
    public float maxElevationAngle = 80f;
    public float minElevationAngle = 10f;
    public float muzzleVelocity = 100f;
    public float mouseSensitivity = 100f;
    public Vector2 cameraVerticalLimits = new Vector2(-5f, 40f);
    public Vector2 cameraHorizontalLimits = new Vector2(-90f, 90f);
    
    public Transform barrel;
    public Camera mortarCamera;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public TMPro.TextMeshProUGUI elevationText;
    public TMPro.TextMeshProUGUI azimuthText;
    public TMPro.TextMeshProUGUI rangeText;
    public float zoomedFOV = 30f;
    public float normalFOV = 60f;
    public float zoomSpeed = 10f;

    private float currentElevationAngle = 45f;
    private bool isZoomed = false;
    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;
    private float currentMouseSensitivity;
    private float cameraStartingRotationX;
    private float cameraStartingRotationY;
    private float timeToShowRange = 2f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        cameraStartingRotationX = mortarCamera.transform.localEulerAngles.x;
        cameraStartingRotationY = mortarCamera.transform.localEulerAngles.y;
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
        HandleMouseLook();
        HandleRangeMeasurement();

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Fire();
        }
        
        if (rangeText.gameObject.activeSelf)
        {
            timeToShowRange -= Time.deltaTime;
            if (timeToShowRange <= 0)
            {
                rangeText.gameObject.SetActive(false);
                timeToShowRange = 2f;
            }
        }
    }
    
    void Fire()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = projectileSpawnPoint.up * muzzleVelocity;
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical"); 

        // Rotate the mortar for azimuth
        transform.Rotate(0, horizontal * rotationSpeed * Time.deltaTime, 0);

        // Calculate the new elevation angle
        currentElevationAngle -= vertical * elevationSpeed * Time.deltaTime;
        currentElevationAngle = Mathf.Clamp(currentElevationAngle, minElevationAngle, maxElevationAngle);
        
        // Rotate the barrel for elevation
        barrel.localEulerAngles = new Vector3(0, 0, currentElevationAngle);
        
        // Calculate the azimuth value in mils
        float azimuthMils = Mathf.Round((transform.eulerAngles.y) * 17.777777777777778f); // Must be in the range 0-6400
        if (azimuthMils < 0)
        {
            azimuthMils += 6400;
        }
        azimuthText.text = azimuthMils.ToString();
        elevationText.text = Mathf.Round((90f-currentElevationAngle) * 17.777777777777778f).ToString();
    }
    
    void HandleMouseLook()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * currentMouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * currentMouseSensitivity * Time.deltaTime;

        // Calculate new vertical rotation
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, cameraVerticalLimits.x, cameraVerticalLimits.y);
        
        // Calculate new horizontal rotation
        horizontalRotation += mouseX;
        horizontalRotation = Mathf.Clamp(horizontalRotation, cameraHorizontalLimits.x, cameraHorizontalLimits.y);
        
        // Apply the new rotation to the camera
        mortarCamera.transform.localEulerAngles = new Vector3(verticalRotation + cameraStartingRotationX, horizontalRotation + cameraStartingRotationY, 0);
        
    }

    void HandleZoom()
    {
        if (Input.GetMouseButton(0)) // Left mouse button
        {
            isZoomed = true;
            currentMouseSensitivity = mouseSensitivity / 2;
        }
        else
        {
            isZoomed = false;
            currentMouseSensitivity = mouseSensitivity;
        }

        float targetFOV = isZoomed ? zoomedFOV : normalFOV;
        mortarCamera.fieldOfView = Mathf.Lerp(mortarCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }
    
    void HandleRangeMeasurement()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Ray ray = new Ray(mortarCamera.transform.position, mortarCamera.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Distance to target: " + hit.distance);
                rangeText.text = Mathf.Round(hit.distance).ToString();
            }
            else
            {
                rangeText.text = "----";
            }
            timeToShowRange = 2f;
            rangeText.gameObject.SetActive(true);
        }

    }
   
}