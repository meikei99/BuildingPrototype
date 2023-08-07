using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public GameObject[] objectsToToggle;

    // This method will be called when the button is clicked
    public void ToggleObjectVisibility()
    {
        foreach (GameObject obj in objectsToToggle)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }
}
