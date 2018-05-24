using UnityEngine;

namespace Denadan.Sprites
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Renderer))]
    public class OverlayedSprite : MonoBehaviour
    {
        public Texture2D Mask;
        public Color MaskColor;

        private Renderer m_renderer;
        private MaterialPropertyBlock props;

       [ExecuteInEditMode]
       private void Start()
        {
            m_renderer = GetComponent<Renderer>();
            props = new MaterialPropertyBlock();
        }

        public void Update()
        {
#if UNITY_EDITOR
            if(props == null)
                Start();
#endif
            m_renderer.GetPropertyBlock(props);

            if (Mask != null)
            {
                props.SetTexture("_MaskTex", Mask);
                props.SetColor("_MaskColor", MaskColor);
            }

            m_renderer.SetPropertyBlock(props);
        }
    }
}