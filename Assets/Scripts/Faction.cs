using UnityEngine;


namespace Mercs
{
    public class Faction : ScriptableObject
    {
        public string Name;
        public Texture2D Flag;
        public Texture2D CamoTemplate;
        public Color CamoColorR;
        public Color CamoColorG;
        public Color CamoColorB;

        [SerializeField]
        private Material BaseMaterial;

        private Material faction_material;

        public Material CamoMaterial
        {
            get
            {
                if (faction_material == null)
                    UpdateMaterial();
                return faction_material;
            }
        }

        private void UpdateMaterial()
        {
            if (BaseMaterial == null)
                return;
            if (faction_material == null)
                faction_material = Instantiate(BaseMaterial);

            faction_material.SetTexture("_Camo", CamoTemplate);
            faction_material.SetColor("_ColorR", CamoColorR);
            faction_material.SetColor("_ColorG", CamoColorG);
            faction_material.SetColor("_ColorB", CamoColorB);
        }
    }


}
