using Managers;
using Map;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    
    [SerializeField] private float movementSpeedNormal = 0.03f;
    [SerializeField] private float movementSpeedFast = 0.06f;
    [SerializeField] private float movementTime = 8.07f;
    [SerializeField] private float rotationAmount = 0.18f;
    [SerializeField] private Vector3 zoomAmount = new Vector3(0, 0.15f, -0.15f);
    
    [SerializeField] private Vector3 newPosition;
    [SerializeField] private Quaternion newRotation;
    [SerializeField] private Vector3 newZoom;
    [SerializeField] private Vector3 dragStartPosition;
    [SerializeField] private Vector3 dragCurrentPosition;

    [SerializeField] private Vector3 minimumPos;
    [SerializeField] private Vector3 maximumPos;
    [SerializeField] private float minimumZoom = 1;
    [SerializeField] private float maximumZoom = 10;

    private void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = targetCamera.transform.localPosition;

        // Subscribe to tile placement and removal events
        TileManager tileManager = GameManager.GetInstance().TileManager;
        tileManager.OnPlacedTile += OnTilePlaced;
        tileManager.OnRemovedTile += OnTileRemoved;

        // Initialize camera bounds based on the current state of the grid
        SetCameraBounds(tileManager.Tiles);
        CenterCamera();
    }

    private void SetCameraBounds(List<Tile> tiles)
    {
        // Calculate the minimum and maximum positions based on active tiles
        if (tiles.Count > 0)
        {
            Vector3 minTilePos = tiles[0].transform.position;
            Vector3 maxTilePos = tiles[0].transform.position;

            foreach (Tile tile in tiles)
            {
                if (!tile.gameObject.activeSelf)
                    continue;

                minTilePos = Vector3.Min(minTilePos, tile.transform.position);
                maxTilePos = Vector3.Max(maxTilePos, tile.transform.position);
            }

            // Set camera positions based on active tiles
            minimumPos = minTilePos;
            maximumPos = maxTilePos;
        }
    }

    private void CenterCamera()
    {
        newPosition = (minimumPos + maximumPos) / 2;
    }

    private void OnTilePlaced()
    {
        SetCameraBounds(GameManager.GetInstance().TileManager.Tiles);
    }

    private void OnTileRemoved()
    {
        SetCameraBounds(GameManager.GetInstance().TileManager.Tiles);
    }

    private void LateUpdate()
    {
        HandleMouseInput();
        HandleMovementInput();
    }

    private void HandleMouseInput()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom -= Input.mouseScrollDelta.y * zoomAmount;
        }
            
        if (Input.GetMouseButtonDown(2))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }

        if (Input.GetMouseButton(2))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }
    }
        
    private void HandleMovementInput()
    {
        float movementSpeed = 0;

        //fast move toggle
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = movementSpeedFast;
        }
        else
        {
            movementSpeed = movementSpeedNormal;
        }

        //forward
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed);
        }
            
        //left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movementSpeed);
        }
            
        //back
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movementSpeed);
        }
            
        //right
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) 
        {
            newPosition += (transform.right * movementSpeed);
        }

        //rotate left
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.AngleAxis(rotationAmount, Vector3.up);
        }

        //rotate right
        if (Input.GetKey(KeyCode.E)) 
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            newRotation = Quaternion.identity;
            CenterCamera();
        }

        newPosition.x = Mathf.Clamp(newPosition.x, minimumPos.x, maximumPos.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minimumPos.y, maximumPos.y);
        newPosition.z = Mathf.Clamp(newPosition.z, minimumPos.z, maximumPos.z);

        newZoom = Vector3.ClampMagnitude(newZoom, maximumZoom);
            
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        targetCamera.transform.localPosition =
            Vector3.Lerp(targetCamera.transform.localPosition, newZoom, Time.deltaTime * movementTime);
    }
}