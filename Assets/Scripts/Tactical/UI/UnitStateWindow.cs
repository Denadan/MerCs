using System.Collections.Generic;
using UnityEngine;

namespace Mercs.Tactical.UI
{
    public class UnitStateWindow : MonoBehaviour
    {
        [SerializeField] private GameObject PartPrefab;
        [SerializeField] private Transform PartsHolder;

        private Dictionary<Parts, UnitPartState> parts = new Dictionary<Parts, UnitPartState>();
        private UnitInfo info;

        public void SetUnit(UnitInfo info)
        {
            parts.Clear();
            this.info = info;
            foreach (Transform child in PartsHolder)
                Destroy(child.gameObject);
            foreach (var part in info.UnitHP.AllParts)
            {
                var p = Instantiate(PartPrefab, PartsHolder, false).GetComponent<UnitPartState>();
                p.Init(this, part, info.UnitHP);
                parts.Add(part,p);
            }
        }

        public void ShowPartDetail(Parts part)
        {
        }

        public void HidePartDetail()
        {
        }
    }
}