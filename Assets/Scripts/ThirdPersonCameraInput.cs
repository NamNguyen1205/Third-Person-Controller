using Unity.Cinemachine;
using UnityEngine;

public class ThirdPersonCameraInput : MonoBehaviour
{
    [SerializeField] private CinemachineInputAxisController CameraAxisController;
    [SerializeField] private float x_Sensitivity = 5f;
    [SerializeField] private float y_Sensitivity = 2f;

    const string LookOrbitX = "Look Orbit X";
    const string LookOrbitY = "Look Orbit Y";

    private void Awake()
    {
        // look sensitive 
        foreach(var axis in CameraAxisController.Controllers)
        {
            if(axis.Name == LookOrbitX)
                axis.Input.Gain = x_Sensitivity;

            if (axis.Name == LookOrbitY)
                axis.Input.Gain = y_Sensitivity;
        }
    }

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            UnlockCursor();
        

        if (Input.GetMouseButtonDown(0))
            LockCursor();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
