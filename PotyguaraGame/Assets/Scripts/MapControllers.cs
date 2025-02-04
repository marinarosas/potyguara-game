using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[System.Serializable]
public class MapTransforms
{
    public Transform vrTarget;
    public Transform ikTarget;

    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void VRMapping()
    {
        ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class MapControllers : MonoBehaviour
{
    [SerializeField] private MapTransforms headTransform;  // Cabeça do avatar
    [SerializeField] private MapTransforms leftHandTransform;  // Mão esquerda do avatar
    [SerializeField] private MapTransforms rightHandTransform;  // Mão direita do avatar

    public float turnSmoothness;
    public Transform ikHead;
    public Vector3 headBodyOffset;


    void LateUpdate()
    {
        transform.position = ikHead.position + headBodyOffset;
        transform.forward = Vector3.Lerp(ikHead.forward, Vector3.ProjectOnPlane(ikHead.forward, Vector3.up).normalized, Time.deltaTime * turnSmoothness);

        headTransform.VRMapping();
        leftHandTransform.VRMapping();
        rightHandTransform.VRMapping();
    }


}

