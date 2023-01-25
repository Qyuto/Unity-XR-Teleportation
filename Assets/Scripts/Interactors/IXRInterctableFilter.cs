using UnityEngine.XR.Interaction.Toolkit;

namespace Unity.Template.VR.Interactors
{
    public interface IXRInterctableFilter
    {
        bool FilteredInteractable(IXRInteractable xrInteractable);
    }
}