using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class TankCam : FreeLookCam {
    [Range(0,90)]
    public float panRange;
    public Transform focusTarget;
    public float focusSpeed;

    Quaternion origDollyRotation;
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
    float focusStartTime;

    // Use this for initialization
    protected override void Start () {
        if (transform.parent)
        {
            origDollyRotation = transform.parent.rotation;
        }
        origRotation = transform.localRotation;
        origPivotRotation = m_Pivot.transform.localRotation;
        origCamFieldOfView = Camera.main.fieldOfView;
        zoomInDistance = origCamFieldOfView;

        base.Start();
    }

    // Update is called once per frame
    new void Update () {
        if (this.userLookEnabled)
        {
            ClearFocus();
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
        float t =  (Time.fixedTime - focusStartTime) / focusSpeed;
        float focusStep = Mathf.SmoothStep(0, 1, t);
        float zoomStep = (Mathf.Tan(focusStep) - (focusStep*.9f)) * 1.5f;

        if (focusTarget != null)
        {
            toTarget = focusTarget.position - transform.parent.position;
            targetRotation = Quaternion.LookRotation(toTarget, transform.parent.up);
            targetPivotRotation = Quaternion.identity;
            
            if (transform.parent)
            {
                transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, targetRotation, focusStep);
            }
            transform.localRotation = Quaternion.identity;
            m_Pivot.transform.localRotation = Quaternion.identity;
        } else
        {
            targetRotation = origRotation;
            targetPivotRotation = origPivotRotation;

            if (transform.parent)
            {
                transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, origDollyRotation, focusStep);
            }
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, focusStep);
            m_Pivot.transform.localRotation = Quaternion.Lerp(m_Pivot.transform.localRotation, targetPivotRotation, focusStep);
        }        

        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoomInDistance, zoomStep);            
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
        focusStartTime = Time.fixedTime;
        zoomInDistance = zoom;
        slowMoTimeScale = timeScale;
    }

    public void ClearFocus()
    {
        focusTarget = null;
        focusStartTime = Time.fixedTime;
        ResetFocus();
    }

    public void ResetFocus()
    {
        slowMoTimeScale = 1f;
        focusStartTime = Time.fixedTime;
        zoomInDistance = origCamFieldOfView;
    }    
}
