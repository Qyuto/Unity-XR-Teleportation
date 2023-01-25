using Unity.Template.VR.Reticle;
using Unity.Template.VR.Teleport;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InvalidReticlePosition : MonoBehaviour
{
    [SerializeField] private ReticleBase invalidReticle;
    [SerializeField] private TeleportValidate teleportValidate;
    private ILineRenderable _renderable;

    private void Awake()
    {
        _renderable = GetComponent<ILineRenderable>();
        if (!invalidReticle.gameObject.scene.IsValid())
            invalidReticle = Instantiate(invalidReticle, transform.position, invalidReticle.transform.rotation);

        if (teleportValidate == null)
            teleportValidate = GetComponentInParent<TeleportValidate>();
    }

    private void Update()
    {
        if (_renderable.TryGetHitInfo(out Vector3 hPosition, out Vector3 _, out int _, out bool isValidTarget))
        {
            invalidReticle.transform.position = hPosition;
            if (isValidTarget && !teleportValidate.IsValidPosition)
                invalidReticle.Switch(true);
            else if (!isValidTarget)
                invalidReticle.Switch(true);
            else
                invalidReticle.Switch(false);
        }
        else
            invalidReticle.Switch(false);
    }
}