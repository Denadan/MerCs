#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;

using Mercs.Tactical.Events;
using UnityEngine.EventSystems;
using Denadan.UI;

namespace Mercs.Tactical.UI
{
    public class UnitOverlay : MonoBehaviour, IUnitDamaged, IUnitEventReceiver, IVisionChanged
    {
        [SerializeField] private UnitInfo info;

        [SerializeField] private Image InitImage;
        [SerializeField] private GameObject HpBack;

        [SerializeField] private BasicSlider StructSlider;
        [SerializeField] private BasicSlider ArmorSlider;
        [SerializeField] private BasicSlider ShieldSlider;
        [SerializeField] private BasicSlider HeatSlider;


        [SerializeField] private Sprite[] Active;
        [SerializeField] private Sprite[] Moved;
        [SerializeField] private Sprite UnknownSprite;

        private CanvasGroup group;
        private Visibility.Level vision;

        private void Awake()
        {
            group = GetComponent<CanvasGroup>();
            EventHandler.SubscribeVisionChange(info, gameObject);
        }

        public void MouseUnitClick(UnitInfo unit, PointerEventData.InputButton button)
        {
        }

        public void Start()
        {
            EventHandler.SubscribeUnitHp(info, gameObject);
            EventHandler.UnitSubscribe(gameObject);

            if (info.Faction == GameController.Instance.PlayerFaction)
                vision = Visibility.Level.Friendly;

            if (info.UnitHP.MaxShield <= 0)
                ShieldSlider.Hide();
            else
                ShieldSlider.MaxValue = info.UnitHP.MaxShield;

            ShieldSlider.Value = info.UnitHP.Shield;
            
            ArmorSlider.MaxValue = info.UnitHP.MaxArmor;
            ArmorSlider.Value = info.UnitHP.TotalArmor;
            StructSlider.MaxValue = info.UnitHP.MaxStructure;
            StructSlider.Value = info.UnitHP.TotalStructure;

            HeatSlider.MaxValue = 1;
            HeatSlider.Value = 0;
        }

        private void OnDestroy()
        {
            EventHandler.UnsubscribeUnitHp(info, gameObject);
            EventHandler.UnsubscribeVisionChange(info, gameObject);
            EventHandler.UnitUnSubscribe(gameObject);
        }

        public void MouseUnitEnter(UnitInfo unit)
        {
            if (unit == info)
                group.alpha = 0.3f;
        }

        public void MouseUnitLeave(UnitInfo unit)
        {
            if (unit == info)
                group.alpha = 1f;
        }

        public void UnitDamage(UnitHp hp)
        {
            ShieldSlider.Value = hp.Shield;
            ArmorSlider.Value = hp.TotalArmor;
            StructSlider.Value = hp.TotalStructure;
        }

        public void VisionChanged(Visibility.Level level)
        {
            UnityEngine.Debug.Log($"{info.PilotName} - {level}");

            if (info.Faction == GameController.Instance.PlayerFaction)
                return;

            vision = level;

            switch (level)
            {
                case Visibility.Level.None:
                    InitImage.gameObject.SetActive(false);
                    HpBack.gameObject.SetActive(false);
                    break;
                case Visibility.Level.Sensor:
                    InitImage.gameObject.SetActive(true);
                    InitImage.sprite = UnknownSprite;
                    HpBack.gameObject.SetActive(false);
                    break;
                case Visibility.Level.Visual:
                case Visibility.Level.Scanned:
                    InitImage.gameObject.SetActive(true);
                    HpBack.gameObject.SetActive(true);
                    break;
            }
        }

        private void FixedUpdate()
        {
            if (vision > Visibility.Level.Sensor)
            {
                int n = Mathf.Clamp(info.Movement.Initiative - 1, 0, 4);
                if (info.Active)
                    InitImage.sprite = Active[n];
                else
                    InitImage.sprite = Moved[n];
            }
        }
    }
}
