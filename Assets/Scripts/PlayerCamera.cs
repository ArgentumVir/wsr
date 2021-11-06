using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public Transform Player;
    Vector3 target, mousePosition, wtf;
    float maxCameraDistance = 3.5f;
    float smoothTime = 0.2f, playerZPosition;
    // Start is called before the first frame update
    public static PlayerCamera Singleton;
    
    void Awake()
    {
        Singleton = this;
    }


    public void SetPlayer(Transform player)
    {
        Player = player;
        target = Player.position;
        playerZPosition = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null) { return; }

        mousePosition = CaptureMousePosition();
        target = UpdateTargetPosition();
        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        Vector3 tempPosition = Vector3.SmoothDamp(transform.position, target, ref wtf, smoothTime);
        transform.position = tempPosition;
    }

    Vector3 UpdateTargetPosition()
    {
        Vector3 mouseOffset = mousePosition * maxCameraDistance;
        Vector3 targetPosition = Player.position + mouseOffset;
        targetPosition.z = playerZPosition;
        return targetPosition;
    }

    Vector3 CaptureMousePosition()
    {
        Vector2 mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        mousePosition *= 2;
        mousePosition -= Vector2.one;
        float max = 0.9f;

        if (Mathf.Abs(mousePosition.x) > max || Mathf.Abs(mousePosition.y) > max)
        {
            mousePosition = mousePosition.normalized;
        }

        return mousePosition;
    }
}
