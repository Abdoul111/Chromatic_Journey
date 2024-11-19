using UnityEngine;

namespace Bundos.WaterSystem
{
    public class WaterDebug : MonoBehaviour
    {
        private Water waterComponent;

        private void Start()
        {
            waterComponent = GetComponent<Water>();
            ValidateSetup();
        }

        private void ValidateSetup()
        {
            // Check Water Object Setup
            var collider = GetComponent<BoxCollider2D>();
            if (collider == null)
            {
                Debug.LogError("Water object is missing a Collider2D component!");
                return;
            }

            if (!collider.isTrigger)
            {
                Debug.LogError("Water collider must have 'Is Trigger' enabled!");
                return;
            }

            // Check mesh bounds vs collider bounds
            var meshFilter = GetComponent<MeshFilter>();
            if (meshFilter != null && meshFilter.sharedMesh != null)
            {
                var meshBounds = meshFilter.sharedMesh.bounds;
                var colliderBounds = collider.bounds;

                if (!Approximately(meshBounds.size, colliderBounds.size, 0.1f))
                {
                    Debug.LogWarning("Water collider size doesn't match mesh bounds! This might cause interaction issues.");
                    Debug.LogWarning($"Mesh size: {meshBounds.size}, Collider size: {colliderBounds.size}");
                }
            }
        }

        private bool Approximately(Vector3 a, Vector3 b, float tolerance)
        {
            return Mathf.Abs(a.x - b.x) < tolerance &&
                   Mathf.Abs(a.y - b.y) < tolerance &&
                   Mathf.Abs(a.z - b.z) < tolerance;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"Water Trigger Enter - GameObject: {other.gameObject.name}");
            Debug.Log($"Has Rigidbody2D: {other.GetComponent<Rigidbody2D>() != null}");
            Debug.Log($"Layer: {LayerMask.LayerToName(other.gameObject.layer)}");
            Debug.Log($"Interactive mode: {waterComponent.interactive}");
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log($"Water Trigger Exit - GameObject: {other.gameObject.name}");
        }
    }
}