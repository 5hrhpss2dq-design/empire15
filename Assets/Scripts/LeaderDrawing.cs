using UnityEngine;
using System.Collections.Generic;
using System;

namespace Empire15.Strategy
{
    /// <summary>
    /// Allows leader role to draw tactical paths that affect gameplay
    /// Drawn paths influence zone objectives and team strategy
    /// Emits OnLineFinished event when drawing is complete
    /// </summary>
    public class LeaderDrawing : MonoBehaviour
    {
        [Header("Drawing Settings")]
        [SerializeField] private float drawDistance = 100f;
        [SerializeField] private float lineWidth = 0.3f;
        [SerializeField] private Color drawColor = Color.cyan;
        [SerializeField] private Material lineMaterial;
        
        [Header("Gameplay Settings")]
        [SerializeField] private float pathInfluenceRadius = 10f;
        [SerializeField] private LayerMask groundLayer;
        
        // Drawing state
        private bool isDrawing = false;
        private List<Vector3> currentPath = new List<Vector3>();
        private LineRenderer currentLineRenderer;
        private GameObject currentPathObject;
        private List<GameObject> drawnPaths = new List<GameObject>();
        
        // Camera reference
        private Camera mainCamera;
        
        // Events
        public event Action<List<Vector3>> OnLineFinished;
        
        public List<Vector3> CurrentPath => new List<Vector3>(currentPath);
        public bool IsDrawing => isDrawing;
        
        private void Start()
        {
            mainCamera = Camera.main;
        }
        
        private void Update()
        {
            HandleDrawingInput();
        }
        
        /// <summary>
        /// Handles mouse input for drawing paths
        /// </summary>
        private void HandleDrawingInput()
        {
            // Start drawing with right mouse button
            if (Input.GetMouseButtonDown(1))
            {
                StartDrawing();
            }
            
            // Continue drawing while holding
            if (Input.GetMouseButton(1) && isDrawing)
            {
                UpdateDrawing();
            }
            
            // Finish drawing on release
            if (Input.GetMouseButtonUp(1) && isDrawing)
            {
                FinishDrawing();
            }
            
            // Clear all paths with C key
            if (Input.GetKeyDown(KeyCode.C))
            {
                ClearAllPaths();
            }
        }
        
        /// <summary>
        /// Starts a new drawing path
        /// </summary>
        private void StartDrawing()
        {
            isDrawing = true;
            currentPath.Clear();
            
            // Create new path object
            currentPathObject = new GameObject("LeaderPath");
            currentLineRenderer = currentPathObject.AddComponent<LineRenderer>();
            
            // Configure line renderer
            currentLineRenderer.startWidth = lineWidth;
            currentLineRenderer.endWidth = lineWidth;
            currentLineRenderer.material = lineMaterial != null ? lineMaterial : new Material(Shader.Find("Sprites/Default"));
            currentLineRenderer.startColor = drawColor;
            currentLineRenderer.endColor = drawColor;
            currentLineRenderer.positionCount = 0;
            currentLineRenderer.useWorldSpace = true;
            
            Debug.Log("Leader started drawing path");
        }
        
        /// <summary>
        /// Updates the current drawing path
        /// </summary>
        private void UpdateDrawing()
        {
            // Cast ray from camera to world
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, drawDistance, groundLayer))
            {
                Vector3 point = hit.point;
                
                // Add point if far enough from last point (to avoid cluttering)
                if (currentPath.Count == 0 || Vector3.Distance(point, currentPath[currentPath.Count - 1]) > 0.5f)
                {
                    currentPath.Add(point);
                    UpdateLineRenderer();
                }
            }
        }
        
        /// <summary>
        /// Updates the line renderer with current path points
        /// </summary>
        private void UpdateLineRenderer()
        {
            if (currentLineRenderer != null && currentPath.Count > 0)
            {
                currentLineRenderer.positionCount = currentPath.Count;
                currentLineRenderer.SetPositions(currentPath.ToArray());
            }
        }
        
        /// <summary>
        /// Finishes the current drawing path
        /// </summary>
        private void FinishDrawing()
        {
            isDrawing = false;
            
            if (currentPath.Count > 1)
            {
                drawnPaths.Add(currentPathObject);
                
                // Emit OnLineFinished event
                OnLineFinished?.Invoke(new List<Vector3>(currentPath));
                
                // Connect path to gameplay (legacy method, can be replaced by event handlers)
                ConnectPathToGameplay();
                
                Debug.Log($"Leader finished drawing path with {currentPath.Count} points");
            }
            else
            {
                // Not enough points, delete the path
                if (currentPathObject != null)
                    Destroy(currentPathObject);
            }
            
            currentPathObject = null;
            currentLineRenderer = null;
        }
        
        /// <summary>
        /// Connects the drawn path to gameplay mechanics
        /// Activates zones near the path
        /// </summary>
        private void ConnectPathToGameplay()
        {
            if (currentPath.Count < 2)
                return;
            
            // Find zones near the path
            var captureZones = FindObjectsOfType<GameLoop.CaptureZone>();
            
            foreach (var zone in captureZones)
            {
                // Check if zone is near any point on the path
                foreach (var point in currentPath)
                {
                    float distance = Vector3.Distance(point, zone.transform.position);
                    if (distance < pathInfluenceRadius)
                    {
                        // Path influences this zone
                        Debug.Log($"Leader path affects {zone.ZoneName}");
                        // Could activate the zone or give it priority
                        break;
                    }
                }
            }
        }
        
        /// <summary>
        /// Clears all drawn paths
        /// </summary>
        public void ClearAllPaths()
        {
            foreach (var path in drawnPaths)
            {
                if (path != null)
                    Destroy(path);
            }
            
            drawnPaths.Clear();
            Debug.Log("All leader paths cleared");
        }
        
        /// <summary>
        /// Gets all currently drawn paths
        /// </summary>
        public List<List<Vector3>> GetAllPaths()
        {
            List<List<Vector3>> allPaths = new List<List<Vector3>>();
            
            foreach (var pathObj in drawnPaths)
            {
                if (pathObj != null)
                {
                    LineRenderer lr = pathObj.GetComponent<LineRenderer>();
                    if (lr != null)
                    {
                        Vector3[] positions = new Vector3[lr.positionCount];
                        lr.GetPositions(positions);
                        allPaths.Add(new List<Vector3>(positions));
                    }
                }
            }
            
            return allPaths;
        }
    }
}
