using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlaceOnClick : MonoBehaviour
{
    [Header("Que placer ?")]
    [SerializeField] private GameObject prefabToPlace;

    [Header("Reference AR")]
    [SerializeField] private ARRaycastManager raycastManager;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GameObject placedObject;

    void Update()
    {
        if (placedObject != null)
            return;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            Vector2 screenPos = Mouse.current.position.ReadValue();
            TryPlaceObject(screenPos);
        }
    }

    private void TryPlaceObject(Vector2 screenPosition)
    {
        if (raycastManager.Raycast(screenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            placedObject = Instantiate(
                prefabToPlace,
                hitPose.position,
                hitPose.rotation
            );
        }
    }

    public void ResetAll()
    {
        if (placedObject != null)
        {
            Destroy(placedObject);
            placedObject = null;
        }
    }
}