using UnityEngine;

public class InputManager : MonoBehaviour
{
    public event System.Action OnTouchStarted;
    public event System.Action OnTouchEnded;
    public event System.Action<Vector3> OnTiltChanged;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 tilt = Input.acceleration;
        OnTiltChanged?.Invoke(tilt);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                OnTouchStarted?.Invoke();
            else if (touch.phase == TouchPhase.Ended)
                OnTouchEnded?.Invoke(); 
        }
    }

    public Vector2 PrimaryPosition()
    {
        if (Input.touchCount == 0) return Vector2.zero;

        Vector2 touchPosition = Input.GetTouch(0).position;

        if (!mainCamera)
        {
            mainCamera = Camera.main;
            if (!mainCamera) Debug.LogWarning("Main camera not found in scene.");
        }

        return mainCamera ? mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, mainCamera.nearClipPlane)) : Vector2.zero;
    }
}