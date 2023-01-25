
namespace Unity.Template.VR.Reticle
{
    public class Reticle : ReticleBase
    {
        public override void Switch(bool status)
        {
            gameObject.SetActive(status);
        }
    }
}