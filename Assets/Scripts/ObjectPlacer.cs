using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new List<GameObject>();

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;
        placedGameObjects.Add(newObject);
        return placedGameObjects.Count - 1;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (gameObjectIndex >= 0 && gameObjectIndex < placedGameObjects.Count)
        {
            GameObject removedObject = placedGameObjects[gameObjectIndex];
            placedGameObjects.RemoveAt(gameObjectIndex);
            Destroy(removedObject);
        }
    }
}
