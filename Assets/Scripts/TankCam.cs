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
    Quaternion origPivotRotation;
    Vector3 currentRotation;
    bool userLookEnabled;
    float origCamFieldOfView;
    float zoomInDistance;
    float slowMoTimeScale = 1f;
    float deltaTimeRatio = .02f;
    Vector3 toTarget;
    Quaternion targetRotation;
    Quaternion targetPivotRotation;

    // Use this for initialization
    protected override void Start () {        
        origRotation = transform.localRotation;
        origPivotRotation = m_Pivot.transform.localRotation;
        origCamFieldOfView = Camera.main.fieldOfView;
        zoomInDistance = origCamFieldOfView;

        base.Start();
    }

    // Update is called once per frame
    new void Update () {
        if (this.userLookEnabled && this.focusTarget == null)
        {
            base.Update();
        } else
        {
            m_LookAngle = 0f;
            m_TiltAngle = 0f;
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
            targetPivotRotation = Quaternion.identity;
        } else
        {
            targetRotation = origRotation;
            targetPivotRotation = origPivotRotation;
        }

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, focusSpeed);
        m_Pivot.transform.localRotation = Quaternion.Slerp(m_Pivot.transform.localRotation, targetPivotRotation, focusSpeed);

        Camera.main.fieldOfView = Mathf.SmoothStep(Camera.main.fieldOfView, zoomInDistance, focusSpeed*2);            
        Time.timeScale = slowMoTimeScale;
        Time.fixedDeltaTime = deltaTimeRatio * Time.timeScale;        
    }   
    
    void ClampPanRange()
    {
        currentRotation = transform.localRotation.eulerAngles;
        
        if (currentRotation.y > panRange && currentRotation.y < 180)
        {
            currentRotation.y = panRange;
        }
        else if (currentRotation.y < 360 - panRange && currentRotation.y > 180)
        {
            currentRotation.y = 360 - panRange;
        }

        transform.localRotation = Quaternion.Euler(currentRotation);
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
