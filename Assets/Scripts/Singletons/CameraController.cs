using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float minX;
    float maxX;
    float minY;
    float maxY;

    Vector3 mouseDownPos;
    bool isDraggingMouse = false;

    void Start()
    {
        var board = FindObjectOfType<GameBoard>();

        var camera = GetComponent<Camera>();

        var vertExtent = camera.orthographicSize;    
        var horzExtent = vertExtent * Screen.width / Screen.height;

        var mapWidth = 1 + 0.75f * board.GetWidth();
        var mapHeight = 1 + 0.5f * board.GetHeight();
 
        // Calculations assume map is position at the origin
        minX = horzExtent;
        maxX = mapWidth - minX;
        minY = vertExtent;
        maxY = mapHeight - minY;

        transform.position = new Vector3(horzExtent, vertExtent);
    }

    void Update()
    {
        if (Input.GetMouseButton(0)){
            if (!isDraggingMouse) {
                isDraggingMouse = true;
                mouseDownPos = GetMouseWorldPos();
            } else {
                var mouseDragOffset = mouseDownPos - GetMouseWorldPos();
                var newPos = transform.position;
                
                newPos += mouseDragOffset;

                newPos.x = Mathf.Max(Mathf.Min(newPos.x, maxX), minX);
                newPos.y = Mathf.Max(Mathf.Min(newPos.y, maxY), minY);

                transform.position = newPos;
            }
        }
        else {
            isDraggingMouse = false;
        }
    }

    private Vector3 GetMouseWorldPos() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
