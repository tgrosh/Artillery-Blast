using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class TankCam : FreeLookCam {
    [Range(0,90)]
    public float panRange;
    public Transform focusTarget;
    public float focusSpeed;

    Quaternion origRotation;
    Vector3 currentRotation;
    bool userLookEnabled;
    float origCamFieldOfView;
    float zoomInDistance;
    float slowMoTimeScale = 1f;
    float deltaTimeRatio = .02f;
    Vector3 toTarget;
    Quaternion targetRotation;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        
        origRotation = transform.rotation;
        origCamFieldOfView = Camera.main.fieldOfView;
        zoomInDistance = origCamFieldOfView;
    }

    // Update is called once per frame
    new void Update () {
        if (this.userLookEnabled && this.focusTarget == null)
        {
            base.Update();
        } else if (this.focusTarget == null)
        {
            m_Pivot.transform.localRotation = Quaternion.identity;
        }

        userLookEnabled = Input.GetMouseButton(1);
        
        ClampPanRange();
    }

    void FixedUpdate()
    {
        if (focusTarget != null)
        {
            toTarget = focusTarget.position - transform.position;
            targetRotation = Quaternion.LookRotation(toTarget);
        } else
        {
            targetRotation = origRotation;
        }

        transform.localRotation = Quaternion.Slerp(transform.rotation, targetRotation, focusSpeed);

        Camera.main.fieldOfView = Mathf.SmoothStep(Camera.main.fieldOfView, zoomInDistance, focusSpeed*2);            
        Time.timeScale = slowMoTimeScale;
        Time.fixedDeltaTime = deltaTimeRatio * Time.timeScale;        
    }   
    
    void ClampPanRange()
    {
        currentRotation = transform.rotation.eulerAngles;
        if (currentRotation.y < 180f && currentRotation.y > origRotation.y + panRange)
        {
            currentRotation.y = origRotation.y + panRange;
        }
        if (currentRotation.y > 180f && currentRotation.y - 360f < origRotation.y - panRange)
        {
            currentRotation.y = origRotation.y - panRange + 360f;
        }
        transform.rotation = Quaternion.Euler(currentRotation);
    } 

    public void FocusOn(Transform target, float zoom, float timeScale)
    {
        focusTarget = target;
        zoomInDistance = zoom;
        slowMoTimeScale = timeScale;
    }

    public void ClearFocus()
    {
        focusTarget = null;
        zoomInDistance = origCamFieldOfView;
        slowMoTimeScale = 1f;
    }
}
