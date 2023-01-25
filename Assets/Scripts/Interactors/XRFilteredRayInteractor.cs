using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Unity.Template.VR.Interactors
{
    public class XRFilteredRayInteractor : XRRayInteractor, IXRInterctableFilter
    {
        protected override void Awake()
        {
            base.Awake();
            hitClosestOnly = true;
        }

        public override void GetValidTargets(List<IXRInteractable> targets)
        {
            Debug.Log($"Targets: {targets.Count}");
            base.GetValidTargets(targets);
            if (targets.Count == 0) return;
            
            if (hitClosestOnly)
            {
                if (!targets[0].transform.TryGetComponent(out BaseTeleportationInteractable _)) return;
                if (!FilteredInteractable(targets[0])) targets.Remove(targets[0]);    
            }
            // else
            // {
            //     for (int i = 0; i < targets.Count; i++)
            //     {
            //         if (!targets[i].transform.TryGetComponent(out BaseTeleportationInteractable _)) continue;
            //         if (!FilteredInteractable(targets[i]))
            //         {
            //             targets.Remove(targets[i]);
            //             i--;
            //         }
            //     }
            // }
        }
        
        public bool FilteredInteractable(IXRInteractable xrInteractable)
        {
            if (TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                foreach (var collider in xrInteractable.colliders)
                {
                    if (collider == hit.collider)
                    {
                        float dot = Vector3.Dot(hit.normal, Vector3.up);
                        return dot >= 0.5f && !Physics.Raycast(hit.point, Vector3.up, 2.2f) || hit.transform.CompareTag("AllowTeleport");
                    }
                }
            }

            return false;
        }

        // public override void GetValidTargets(List<IXRInteractable> targets)
        // {
        //     Debug.Log($"Targets: {targets.Count}");
        //     base.GetValidTargets(targets);
        //     for (int i = 0; i < targets.Count; i++)
        //     {
        //         // Debug.Log($"Interactable name: {targets[i].transform.name}");
        //         if (!targets[i].transform.TryGetComponent(out BaseTeleportationInteractable _)) continue;
        //         if (!FilteredInteractable(targets[i]))
        //         {
        //             targets.Remove(targets[i]);
        //             i--;
        //         }
        //     }
        // }
        //
        //
        // public bool FilteredInteractable(IXRInteractable xrInteractable)
        // {
        //     if (TryGetCurrent3DRaycastHit(out RaycastHit hit))
        //     {
        //         foreach (var collider in xrInteractable.colliders)
        //         {
        //             if (collider == hit.collider)
        //             {
        //                 float dot = Vector3.Dot(hit.normal, Vector3.up);
        //                 return dot >= 0.5f && !Physics.Raycast(hit.point, Vector3.up, 2.2f) ||
        //                        hit.transform.CompareTag("AllowTeleport");
        //             }
        //         }
        //     }
        //     return false;
        // }
    }
}