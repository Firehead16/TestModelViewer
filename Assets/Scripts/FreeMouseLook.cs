using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FreeMouseLook : MonoBehaviour
{
    [SerializeField]
    private Camera freelookCamera;

    [SerializeField]
    private float Sensitivity = 5.0f;

    void Update()
    {
        var rotationX = (Sensitivity * Mouse.current.delta.x.ReadValue() * 20) * Time.deltaTime;
        var rotationY = (Sensitivity * Mouse.current.delta.y.ReadValue() * 20) * Time.deltaTime;

        Vector3 cameraRotation = freelookCamera.transform.rotation.eulerAngles;

        if (Mouse.current.rightButton.ReadValue() > 0)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            transform.Rotate(Vector3.right, rotationY);
            transform.Rotate(transform.up, rotationX);
            transform.SetPositionAndRotation(transform.position,
                Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0));
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        #region Зум колесом мыши

        if (Mouse.current.scroll.y.ReadValue() == 120) freelookCamera.fieldOfView =
                Mathf.Clamp(freelookCamera.fieldOfView - 5.0f, 20, 75);

        if (Mouse.current.scroll.y.ReadValue() == -120) freelookCamera.fieldOfView =
            Mathf.Clamp(freelookCamera.fieldOfView + 5.0f, 20, 75); 
        #endregion


        #region Ограничения вращения по Y

        if (transform.rotation.eulerAngles.x < 90)
        {
            transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.rotation.eulerAngles.x, -10, 60), transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        else
        {
            transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.rotation.eulerAngles.x, 300, 370), transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        } 
        #endregion
        


        #region Описания при наведении и нажатии лкм

        if (Mouse.current.leftButton.ReadValue() > 0)
        {
            Ray ray = freelookCamera.ScreenPointToRay(new Vector3(Mouse.current.position.x.value, Mouse.current.position.y.value, 0));
            RaycastHit DescriptionHit;
            bool outOfCamera = Physics.Raycast(ray, out DescriptionHit);
            if (outOfCamera && DescriptionHit.transform.GetComponent<Description>() != null)
            {
                ViewerManager.Instance.CurrentObject = DescriptionHit.transform.gameObject;
            }

        }
        #endregion
    }
}
