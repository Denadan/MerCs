using UnityEngine;

namespace Mercs.Sprites
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Renderer))]
    public class CamoSprite : MonoBehaviour
    {
        public Texture2D CamoTemplate;
        public Color ColorR;
        public Color ColorG;
        public Color ColorB;

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
            if (props == null)
                Start();
#endif
            m_renderer.GetPropertyBlock(props);

            if (CamoTemplate != null)
            {
                props.SetTexture("_Camo", CamoTemplate);
                props.SetColor("_ColorR", ColorR);
                props.SetColor("_ColorG", ColorG);
                props.SetColor("_ColorB", ColorB);
            }

            m_renderer.SetPropertyBlock(props);
        }
    }
}
