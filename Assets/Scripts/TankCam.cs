using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class TankCam : FreeLookCam {
    [Range(0,90)]
    public float panRange;
    public Transform lookAtTarget;
    public float lookAtSpeed;    

    float startRotationY;
    Vector3 currentRotation;
    bool camEnabled;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        
        startRotationY = transform.rotation.y;
	}

    // Update is called once per frame
    new void Update () {
        if (this.camEnabled && this.lookAtTarget == null)
        {
            base.Update();
        }

        camEnabled = Input.GetMouseButton(1);
        currentRotation = transform.rotation.eulerAngles;
        if (currentRotation.y < 180f && currentRotation.y > startRotationY + panRange)
        {
            currentRotation.y = startRotationY + panRange;
        }
        if (currentRotation.y > 180f && currentRotation.y - 360f < startRotationY - panRange)
        {
            currentRotation.y = startRotationY - panRange + 360f;
        }
        transform.rotation = Quaternion.Euler(currentRotation);
    }

    void FixedUpdate()
    {
        if (lookAtTarget != null)
        {
            Vector3 toTarget = lookAtTarget.position - transform.position;            
            Quaternion targetRotation = Quaternion.LookRotation(toTarget);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookAtSpeed);
        }
    }    
}
