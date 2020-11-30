using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public float minDistance = 1.0f;
    public float maxDistance = 6.0f;
    public float smooth = 10.0f;

    public float distance;

    public Vector3 dollyDirAdjusted;
    Vector3 dollyDir;

    
    // Start is called before the first frame update
    void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 disiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;

        if (Physics.Linecast(transform.parent.position, disiredCameraPos, out hit))
        {
            distance = Mathf.Clamp((hit.distance * 0.95f), minDistance, maxDistance);      // der kan ændres på den float man ganger hit.distance med, hvis kameraet stadig går igennem jorden og objekter (0.9 er udgangspunktet så måske 0.85 eller endda 0.8)
        }
        else
        {
            distance = maxDistance;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
    }
}
