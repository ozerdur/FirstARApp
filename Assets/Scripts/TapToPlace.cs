using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TapToPlace : MonoBehaviour
{

    public GameObject placementIndicator;
    public GameObject objectToPlace;
    List<ARRaycastHit> hitResults = new List<ARRaycastHit>();
    private bool planeFound;
    private Pose placementPose;
    
    [SerializeField]
    private ARRaycastManager m_ARRaycastManager;
    
    // Start is called before the first frame update
    void Start()
    {
        m_ARRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        if (planeFound &&  Input.touches.Length > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }

    void PlaceObject()
    {
        Instantiate(objectToPlace, placementPose.position, placementPose.rotation);
    }
    
    void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(.5f, .5f));
        m_ARRaycastManager.Raycast(screenCenter, hitResults, TrackableType.PlaneWithinPolygon);
        planeFound = hitResults.Count > 0;
        if (planeFound)
        {
            placementPose = hitResults[0].pose;
            
        }
    }

    void UpdatePlacementIndicator()
    {
        placementIndicator.SetActive(planeFound);
        if (planeFound)
        {
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
    }
}
