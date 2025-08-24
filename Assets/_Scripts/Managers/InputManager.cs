using UnityEngine;
using UnityEngine.InputSystem;

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
#if UNITY_EDITOR || UNITY_STANDALONE
        // Simulate tilt with keys, independent of legacy Input Manager

        float h = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) h -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) h += 1f;
        OnTiltChanged?.Invoke(new Vector3(h, 0f, 0f));

        // Simulate tap with mouse
        if (Input.GetMouseButtonDown(0)) OnTouchStarted?.Invoke();
        if (Input.GetMouseButtonUp(0)) OnTouchEnded?.Invoke();
#else
        // Real device
        OnTiltChanged?.Invoke(Input.acceleration);

        if (Input.touchCount > 0)
        {
            var t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began) OnTouchStarted?.Invoke();
            if (t.phase == TouchPhase.Ended) OnTouchEnded?.Invoke();
        }
#endif
    }

    public Vector2 PrimaryPosition()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (!Input.GetMouseButton(0)) return Vector2.zero;
        return mainCamera ? (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition) : Vector2.zero;
#else
        if (Input.touchCount == 0) return Vector2.zero;
        return mainCamera ? (Vector2)mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position) : Vector2.zero;
#endif
    }
}
