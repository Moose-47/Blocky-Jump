using UnityEngine;
using UnityEngine.UI;

public class UnstuckDetector : MonoBehaviour
{
    [Header("References")]
    public Button unstuckButton;

    public Transform player;

    [Header("Raycast Settings")]
    public float rayLength = 20f;

    [Tooltip("Time in seconds between ray checks (helps performance).")]
    public float checkInterval = 0.1f;

    [Tooltip("Color when unstuck is available.")]
    public Color availableColor = Color.green;

    [Tooltip("Color when unstuck is unavailable.")]
    public Color unavailableColor = Color.red;

    private bool canUseUnstuck = false;
    private float checkTimer = 0f;

    private void Update()
    {
        checkTimer += Time.unscaledDeltaTime;

        //Run the raycast check at fixed intervals for performance
        if (checkTimer >= checkInterval)
        {
            CheckUnstuckAvailability();
            checkTimer = 0f;
        }

        //Update button interactability
        if (unstuckButton != null)
            unstuckButton.interactable = canUseUnstuck;
    }

    private void CheckUnstuckAvailability()
    {
        if (player == null)
        {
            Debug.LogWarning("UnstuckDetector: No player assigned!");
            return;
        }

        //Raycast down from this GameObject’s position
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, rayLength);

        bool foundLockedBlock = false;
        bool foundPlayer = false;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider == null) continue;

            //Ignore moving blocks entirely
            if (hit.collider.CompareTag("LockedBlock"))
            {
                foundLockedBlock = true;
                break; //Stop when we find the first locked block
            }

            if (hit.collider.CompareTag("Player"))
            {
                foundPlayer = true;
                break; //Player is visible, stop here
            }
        }

        canUseUnstuck = foundLockedBlock && !foundPlayer;

        //Debug ray
        Debug.DrawRay(transform.position, Vector2.down * rayLength,
            canUseUnstuck ? availableColor : unavailableColor);
    }

    public bool CanUseUnstuck()
    {
        return canUseUnstuck;
    }
}
