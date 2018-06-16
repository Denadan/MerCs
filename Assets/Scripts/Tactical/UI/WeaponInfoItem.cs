#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{
    public class WeaponInfoItem : MonoBehaviour
    {
        [SerializeField] private Image Back;
        [SerializeField] private Image Action;
        [SerializeField] private Image Icon;
        [SerializeField] private Text Name;
        [SerializeField] private Text Damage;
        [SerializeField] private Image Ammo;
        [SerializeField] private Text Count;
        [SerializeField] private Text Hit;

        [SerializeField] private Color Main;
        [SerializeField] private Color Disabled;

        [SerializeField] private Sprite Cancel;
        [SerializeField] private Sprite Empty;
        [SerializeField] private Sprite None;
        [SerializeField] private Sprite Item1;
        [SerializeField] private Sprite Item2;
        [SerializeField] private Sprite Item3;

        [Header("bar")]
        [SerializeField] private Image ShotBack;
        [SerializeField] private Transform ShotContainer;
        [SerializeField] private GameObject ShotPrefab;

        [SerializeField] private Color BarGreen;
        [SerializeField] private Color BarRed;
        [SerializeField] private Color BarGrey;


        private int shots;

        public void Set(WeaponsData.Info info)
        {
            shots = 1;
            if (info.CanShoot)
            {
                Action.sprite = Empty;
                Back.color = Main;
            }
            else
            {
                Action.sprite = Cancel;
                Back.color = Disabled;
            }

            Name.text = info.Weapon.ShortName;
            Icon.sprite = info.Weapon.Icon;
            if (info.Weapon.Shots > 1)
                if (info.Weapon.VariableShots)
                {
                    Damage.text = $"{info.Weapon.Damage:##0.##}x{shots}";
                }
                else
                {
                    Damage.text = $"{info.Weapon.Damage:##0.##}x{info.Weapon.Shots}";
                    ShotBack.enabled = false;
                    ShotContainer.gameObject.SetActive(false);
                }
            else
            {
                Damage.text = info.Weapon.Damage.ToString("##0.##");
                ShotBack.enabled = false;
                ShotContainer.gameObject.SetActive(false);
            }

            if (info.UseAmmo)
            {
                if (info.Ammo == null)
                {
                    Ammo.sprite = Empty;
                    Count.text = "--";
                }
                else
                {
                    Ammo.sprite = info.Ammo.Icon;
                    Count.text = info.AmmoLeft.ToString();
                }
            }
            else
            {
                Count.text = "";
                Ammo.sprite = None;
            }

            Hit.text = "---";
        }

        public void Set(int percent)
        {

        }

        private void shot_click(int n)
        {
        }
    }
}