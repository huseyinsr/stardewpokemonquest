using UnityEngine;

public class TelescopeDetecter : MonoBehaviour
{
    [SerializeField] private GameObject cameraPlace;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Telescope"))
        {
            Destroy(cameraPlace);
            ZoomManager.Instance.ExitZoom();
            //Debug.Log("Telescope detected, camera place destroyed and zoom exited.");
        }
    }

}