using System.Text;
using UnityEngine;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "MerCs/Module/Weapon")]
    public partial class Weapon : ModuleInfo<WeaponTemplate>, IHeatProducer
    {
        private bool apply = false;


        public override ModuleType ModType => ModuleType.Weapon;



        /// <summary>
        /// модификатор урона боеприпаса
        /// </summary>
        public float DamageMult { get; private set; }
        /// <summary>
        /// общий урон
        /// </summary>
        public float Damage => EDamage + BDamage + MDamage;

        public AmmoType AmmoType => Template.Ammo;

        /// <summary>
        /// балистический урон
        /// </summary>
        public float BDamage =>  (LoadedAmmo ?? Template.StockAmmo).BDamage * DamageMult;
        /// <summary>
        /// энергетический урон
        /// </summary>
        public float EDamage => (LoadedAmmo ?? Template.StockAmmo).EDamage * DamageMult;
        /// <summary>
        /// взрывной урон
        /// </summary>
        public float MDamage => (LoadedAmmo ?? Template.StockAmmo).MDamage * DamageMult;

        /// <summary>
        /// нагрев при попадании
        /// </summary>
        public float HeatDamage => (LoadedAmmo ?? Template.StockAmmo).HeatDamage * DamageMult;
        /// <summary>
        /// урон по стабильности
        /// </summary>
        public float StabDamage => (LoadedAmmo ?? Template.StockAmmo).StabDamage * DamageMult;

        /// <summary>
        /// для каждого выстрела проводится отдельный рассчет попадания
        /// </summary>
        public bool IndepedndedShots => Template.IndependedShots;

        /// <summary>
        /// Можно лли изменять количество выстрелов
        /// </summary>
        public bool VariableShots => Template.VariableShots;

        /// <summary>
        /// минимальное расстояние стрельбы
        /// артилерия - нет урона
        /// автопушки - уменьшена точность
        /// лазеры - уменьшена точность
        /// ракеты - нет урона
        /// </summary>
        public float MinRange { get; private set; }
        /// <summary>
        /// min-optimal = расстояние на котором точность максимальна
        /// </summary>
        public float Optimal { get; private set; }
        /// <summary>
        /// optimal-fallof - расстояние с уменьшением точности
        /// </summary>
        public float Falloff { get; private set; }

        /// <summary>
        /// количество выстрелов
        /// </summary>
        public int Shots { get; private set; }

        /// <summary>
        /// нагрев от залпа или выстрела при изменяемом числе выстрелов
        /// </summary>
        public float HeatForShot { get; private set; }

        /// <summary>
        /// рейтинг нагрева для статистики
        /// </summary>
        public float Heat => Template.VariableShots ? Shots * HeatForShot : HeatForShot;

        #region game_data
        public Ammo LoadedAmmo;
        #endregion

        /// <summary>
        /// применение апгрейда
        /// ВАЖНО: до использования этого метода инфа модуля не верна!
        /// </summary>
        public override void ApplyUpgrade()
        {
            if (Template == null || Template == null || Template.StockAmmo == null)
                return;

            base.ApplyUpgrade();
            DamageMult = upgrade(UpgradeType.Damage, Template.DamageMult);
            MinRange = upgrade(UpgradeType.Range, Template.MinRange);
            Optimal = upgrade(UpgradeType.Range, Template.Optimal);
            Falloff = upgrade(UpgradeType.Range, Template.Falloff);
            HeatForShot = upgrade(UpgradeType.Heat, Template.HeatForShot);
            Shots = (int)upgrade(UpgradeType.Shots, Template.Shots);

            Name = Name.Replace("%SHOT%", Shots.ToString());
            ShortName = ShortName.Replace("%SHOT%", Shots.ToString());
            BaseName = BaseName.Replace("%SHOT%", Shots.ToString());
            if(Template.StockAmmo != null)
                Template.StockAmmo.ApplyUpgrade();

            apply = true;
            LoadAmmo(LoadedAmmo);
        }

        /// <summary>
        /// зарядить боеприпас
        /// </summary>
        /// <param name="ammo"></param>
        public void LoadAmmo(Ammo ammo)
        {
            var old = LoadedAmmo;
            if (old != null && old.Type == Template.Ammo && !apply)
            {
                MinRange /= old.RangeMod;
                Optimal /= old.RangeMod;
                Falloff /= old.RangeMod;
            }
            apply = false;


            LoadedAmmo = (ammo != null && ammo.Type == AmmoType) ? ammo : null;
            if(LoadedAmmo != null)
            {
                MinRange *= LoadedAmmo.RangeMod;
                Optimal *= LoadedAmmo.RangeMod;
                Falloff *= LoadedAmmo.RangeMod;
            }
        }

        
#if UNITY_EDITOR
        public override string ToString()
        {
            if (Template == null || Upgrade == null || Template.StockAmmo == null)
                return "NO TEMPLATE!";


            StringBuilder sb = new StringBuilder();
            sb.Append("Weapon\n");
            sb.Append(base.ToString());
            sb.Append($"\nClass: {Template.Type}({Template.DamageType})\n");
            sb.Append($"================ AMMO =============\n");
            sb.Append((LoadedAmmo ?? Template.StockAmmo).ToString());
            sb.Append($"\n==================================\n");

            sb.Append($"Damage multuplier: {DamageMult:F2}\n");
            sb.Append($"Damage: {Damage:F2} (E:{EDamage:F2} B:{BDamage:F2} M:{MDamage:F2})\n");
            sb.Append($"Heat: {HeatDamage:F2}  Stab:{StabDamage:F2}\n");
            sb.Append($"Shots: {Shots} {(Template.VariableShots ? '~' : ' ')}\n");
            sb.Append($"Heat Generate:{HeatForShot:F2}\n");
            sb.Append($"Range: {MinRange}/{Optimal}/{Falloff}\n");
            sb.Append($"FireMode:");
            if (Template.DirectFire)
                sb.Append(" direct");
            if (Template.IndirectFire)
                sb.Append(" indirect");
            if (Template.SupportFire)
                sb.Append(" support");

            return sb.ToString();
        }
#endif
    }
}