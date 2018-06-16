using Mercs.Sprites;
using UnityEngine;

namespace Mercs.Tactical
{
    public class UnitGFX : MonoBehaviour
    {
        
        [SerializeField] private SpriteRenderer sprite = null;
        [SerializeField] private SpriteRenderer blip = null;
        [SerializeField] private CamoSprite camo = null;


        public void SetData(UnitTemplate template)
        {
            
            sprite.sprite = template.Sprite;
            if (blip != null) 
            switch (template.Type)
            {
                case UnitType.MerC:
                    blip.sprite = TacticalUIController.Instance.MercBlip;
                    break;
                case UnitType.Vehicle:
                    blip.sprite = TacticalUIController.Instance.TankBlip;
                    break;
                case UnitType.Tank:
                    blip.sprite = TacticalUIController.Instance.TankBlip;
                    break;
                case UnitType.Turret:
                    blip.sprite = TacticalUIController.Instance.TurretBlip;
                    break;
                }
        }

        public void SetFaction(Faction faction)
        {
            camo.CamoTemplate = faction.CamoTemplate;
            camo.ColorB = faction.CamoColorB;
            camo.ColorR = faction.CamoColorR;
            camo.ColorG = faction.CamoColorG;
        }

        public void AddCollider()
        {
            gameObject.AddComponent<PolygonCollider2D>();
        }

        public void RemoveCollider()
        {
            Destroy(gameObject.GetComponent<Collider2D>());
        }
    }
}