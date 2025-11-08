using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InputManager))]
public class TapDetection : MonoBehaviour
{
    private InputManager inputManager;

    [Header("Tap Settings")]
    public float tapTimeOut = 0.2f;

    private float startTime;
    private Vector2 startPosition;

    public event System.Action<Vector2> OnTap;

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        inputManager.OnTouchStarted += HandleTouchStarted;
        inputManager.OnTouchEnded += HandleTouchEnded;
    }

    private void OnDestroy()
    {
        inputManager.OnTouchStarted -= HandleTouchStarted;
        inputManager.OnTouchEnded -= HandleTouchEnded;
    }

    private void HandleTouchStarted()
    {
        startTime = Time.time;
        startPosition = inputManager.PrimaryPosition();
    }

    private void HandleTouchEnded()
    {
        float timeHeld = Time.time - startTime;

#if UNITY_EDITOR
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;
#else
        if (EventSystem.current != null && Input.touchCount > 0 &&
            EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            return;
#endif

        if (timeHeld <= tapTimeOut)
            OnTap?.Invoke(startPosition);
    }
}