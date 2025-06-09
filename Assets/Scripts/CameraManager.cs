using System.Runtime.CompilerServices;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private CinemachinePositionComposer positionComposer;

    [SerializeField] private bool canChangeCameraDistance;
    [SerializeField] private float cameraChangeRate = 1f;
    [SerializeField] private float cameraChangeThreshold = 0.1f; // Minimum distance between current and target distance.

    private float targetCameraDistance;

    // Determined by weapon. Declared in Weapon.cs. Called in PlayerWeaponController.
    public void ChangeCameraDistance(float cameraDistance) => targetCameraDistance = cameraDistance;

    private void Update()
    {
        UpdateCameraDistance();
    }
    private void UpdateCameraDistance()
    {
        if (!canChangeCameraDistance)
            return;

        // Prevents the Lerp method to infinitly get closer to the targetCameraDistance.
        if (Mathf.Abs(targetCameraDistance - positionComposer.CameraDistance) < cameraChangeThreshold)
            return;

        positionComposer.CameraDistance = Mathf.Lerp(
            positionComposer.CameraDistance, targetCameraDistance, cameraChangeRate * Time.deltaTime);
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("You had more than one CameraManager");
            Destroy(gameObject);
        }

        positionComposer = GetComponentInChildren<CinemachinePositionComposer>();
        
    }
}
