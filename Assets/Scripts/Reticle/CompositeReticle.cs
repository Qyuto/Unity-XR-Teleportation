using Unity.Template.VR.Teleport;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Unity.Template.VR.Reticle
{
    public class CompositeReticle : MonoBehaviour
    {
        [SerializeField] private XRRayInteractor rayInteractor;
        [SerializeField] private TeleportValidate teleportValidate;

        [SerializeField] private ReticleBase telportReticle;
        [SerializeField] private ReticleBase interactableReticle;
        [SerializeField] private ReticleBase defaultReticle;
        
        private void Reset()
        {
            if (rayInteractor == null)
                rayInteractor = GetComponentInParent<XRRayInteractor>();
            if (teleportValidate == null)
                teleportValidate = GetComponentInParent<TeleportValidate>();
        }

        private void Awake()
        {
            if (rayInteractor == null)
                rayInteractor = GetComponentInParent<XRRayInteractor>();
            if (teleportValidate == null)
                teleportValidate = GetComponentInParent<TeleportValidate>();

            rayInteractor.hoverEntered.AddListener(HoverEntered);
        }

        private RaycastHit _currentHit;
        private XRBaseInteractable _baseInteractable;
        private ReticleType _currentType;
        private void HoverEntered(HoverEnterEventArgs eventArgs)
        {
            if (!rayInteractor.TryGetCurrent3DRaycastHit(out _currentHit)) return;
            _baseInteractable = rayInteractor.interactablesHovered[0] as XRBaseInteractable;
            DetermineReticleType(_baseInteractable);
            // if (rayInteractor.interactablesHovered.Count == 0) return;
            // DetermineReticleType(rayInteractor.interactablesHovered[0].transform.GetComponent<XRBaseInteractable>());
            // foreach (var collider in eventArgs.interactableObject.colliders)
            // {
            //     if (_currentHit.collider != collider) continue;
            //     DetermineReticleType(collider.GetComponentInParent<XRBaseInteractable>());
            // }
        }

        private void Update()
        {
            if(_currentHit.transform == null) return;
            DetermineReticleType(_baseInteractable);
        }

        private void DetermineReticleType(XRBaseInteractable baseInteractable)
        {
            if (baseInteractable == null)
            {
                ChangeActiveReticle(ReticleType.Default);
                return;
            }
            if (!teleportValidate.IsValidPosition)
            {
                DisableAllReticles();
                return;
            }
            
            switch (baseInteractable)
            {
                case BaseTeleportationInteractable:
                    ChangeActiveReticle(ReticleType.Teleport);
                    break;
                case XRSimpleInteractable:
                    ChangeActiveReticle(ReticleType.Interactable);
                    break;
                default:
                    ChangeActiveReticle(ReticleType.Default);
                    break;
            }
        }

        private void ChangeActiveReticle(ReticleType type)
        {
            DisableAllReticles();
            switch (type)
            {
                case ReticleType.Default:
                {
                    if (defaultReticle == null) return;
                    defaultReticle.Switch(true);
                    break;
                }
                case ReticleType.Teleport:
                {
                    if (telportReticle == null)
                        ChangeActiveReticle(ReticleType.Default);
                    else
                        telportReticle.Switch(true);
                    break;
                }
                case ReticleType.Interactable:
                {
                    if (interactableReticle == null)
                        ChangeActiveReticle(ReticleType.Default);
                    else
                        interactableReticle.Switch(true);
                    break;
                }
                default:
                {
                    if (defaultReticle == null) return;
                    defaultReticle.Switch(true);
                    break;
                }
            }
        }

        private void DisableAllReticles()
        {
            if (telportReticle != null)
                telportReticle.Switch(false);

            if (interactableReticle != null)
                interactableReticle.Switch(false);

            if (defaultReticle != null)
                defaultReticle.Switch(false);
        }
    }

    enum ReticleType
    {
        Default,
        Teleport,
        Interactable
    }
}