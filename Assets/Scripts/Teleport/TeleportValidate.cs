using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Unity.Template.VR.Teleport
{
    public class TeleportValidate : MonoBehaviour
    {
        [SerializeField] private ActionBasedController xrController;
        private XRRayInteractor _rayInteractor;
        private InputActionProperty _teleportAction;

        public bool IsValidPosition { get; private set; }
        
        private void Reset()
        {
            if (xrController == null)
                xrController = GetComponentInParent<ActionBasedController>();
        }

        private void Awake()
        {
            if (xrController == null)
                xrController = GetComponentInParent<ActionBasedController>();

            _rayInteractor = xrController.GetComponent<XRRayInteractor>();
            _teleportAction = xrController.selectAction;
        }

        private void Update()
        {
            ValidatingHitPosition();
        }

        private void ValidatingHitPosition()
        {
            if (!_rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit)) return;

            var teleportation = hit.collider.GetComponentInParent<BaseTeleportationInteractable>();
            if (teleportation == null)
            {
                xrController.selectAction = _teleportAction;
                return;
            }

            if (_rayInteractor.TryGetHitInfo(out Vector3 _, out Vector3 hNormal, out int _, out bool isValidTarget))
            {
                float dot = Vector3.Dot(hNormal, Vector3.up);
                if (isValidTarget && dot >= 0.5f && !Physics.Raycast(hit.point, Vector3.up, 2.2f) || hit.transform.CompareTag("AllowTeleport"))
                {
                    IsValidPosition = true;
                    xrController.selectAction = _teleportAction;
                }
                else
                {
                    IsValidPosition = false;
                    xrController.selectAction = default;
                }
            }
        }
    }
}