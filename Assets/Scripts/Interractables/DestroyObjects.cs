using UnityEngine;

public class DestroyObjects : MonoBehaviour
{
    // Assign these in the Unity Inspector
    public GameObject[] objectsToDestroy = new GameObject[4];

    // Call this method to destroy the assigned objects
    public void DestroyAssignedObjects()
    {
        foreach (GameObject obj in objectsToDestroy)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
    }

    // Example: Destroy objects when pressing the "D" key
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            DestroyAssignedObjects();
        }
    }
}