using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportTagFinder : MonoBehaviour
{
    private const string teleportTag = "AllowTeleport";

    private void Start()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(teleportTag);

        foreach (var obj in objects)
        {
            if (!obj.TryGetComponent(out TeleportationArea _))
            {
                TeleportationArea teleportation = obj.AddComponent<TeleportationArea>();
                teleportation.teleportTrigger = BaseTeleportationInteractable.TeleportTrigger.OnSelectEntered;
            }
        }
    }
}
