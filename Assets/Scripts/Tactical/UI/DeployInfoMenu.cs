using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Mercs.Tactical.UI
{
    public class DeployInfoMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject TextPrefab;
        private DeployParameters prams;

        private Text Caption;
        private Text WeightCaption;
        private Text WeightMin;
        private Text WeightMax;

        private Text CountCaption;
        private Text CountMin;
        private Text CountMax;

        private Text DeployedWeight;
        private Text DeployedCount;

        public bool InitDeployInfo(DeployParameters prams)
        {
            foreach(Transform child in transform)
                Destroy(child.gameObject);

            this.prams = prams;
            Caption = Instantiate(TextPrefab, transform).GetComponent<Text>();

            if (prams.CheckCount != DeployParameters.Range.None)
            {
                CountCaption = Instantiate(TextPrefab, transform).GetComponent<Text>();
                CountCaption.text = "Count:";
                if (prams.CheckCount.HasFlag(DeployParameters.Range.Min))
                {
                    CountMin = Instantiate(TextPrefab, transform).GetComponent<Text>();
                    CountMin.text = $" Min: {prams.CountLimit.x}";
                }

                if (prams.CheckCount.HasFlag(DeployParameters.Range.Max))
                {
                    CountMax = Instantiate(TextPrefab, transform).GetComponent<Text>();
                    CountMax.text = $" Max: {prams.CountLimit.y}";
                }
                DeployedCount = Instantiate(TextPrefab, transform).GetComponent<Text>();
            }

            if (prams.CheckWeight != DeployParameters.Range.None)
            {
                WeightCaption = Instantiate(TextPrefab, transform).GetComponent<Text>();
                WeightCaption.text = "Weight:";
                if (prams.CheckWeight.HasFlag(DeployParameters.Range.Min))
                {
                    WeightMin = Instantiate(TextPrefab, transform).GetComponent<Text>();
                    WeightMin.text = $" Min: {prams.WeighLimit.x}";
                }

                if (prams.CheckWeight.HasFlag(DeployParameters.Range.Max))
                {
                    WeightMax = Instantiate(TextPrefab, transform).GetComponent<Text>();
                    WeightMax.text = $" Max: {prams.WeighLimit.y}";
                }
                DeployedWeight = Instantiate(TextPrefab, transform).GetComponent<Text>();
            }

            return UpdateDeployInfo();

        }

        public bool UpdateDeployInfo()
        {
            int count = TacticalController.Instance.Units.Count(unit => !unit.Reserve);
            int weight = TacticalController.Instance.Units.Where(unit => !unit.Reserve).Sum(unit => unit.Weight);

            if (prams.CheckWeight == DeployParameters.Range.None && prams.CheckCount == DeployParameters.Range.None)
            {
                Caption.text = "Deploy: Free";
                Caption.color = Color.green;
                return count > 0;
            }

            bool w = true;
            if (prams.CheckWeight != DeployParameters.Range.None)
            {
                if (prams.CheckWeight.HasFlag(DeployParameters.Range.Min))
                {
                    bool c = prams.WeighLimit.x <= weight;
                    w &= c;
                    WeightMin.color = color(c);
                }

                if (prams.CheckWeight.HasFlag(DeployParameters.Range.Max))
                {
                    bool c = prams.WeighLimit.y >= weight;
                    w &= c;
                    WeightMax.color = color(c);
                }

                DeployedWeight.text = $" Deployed: {weight}";
                DeployedWeight.color = WeightCaption.color = color(w);
                
            }

            bool n = true;
            if (prams.CheckCount != DeployParameters.Range.None)
            {
                if (prams.CheckCount.HasFlag(DeployParameters.Range.Min))
                {
                    bool c = prams.CountLimit.x <= count;
                    n &= c;
                    CountMin.color = color(c);
                }

                if (prams.CheckCount.HasFlag(DeployParameters.Range.Max))
                {
                    bool c = prams.CountLimit.y >= count;
                    n &= c;
                    CountMax.color = color(c);
                }

                DeployedCount.text = $" Deployed: {count}";
                DeployedCount.color = CountCaption.color = color(n);
            }

            w &= n;

            Caption.color = color(w);
            Caption.text = w ? "Deploy: Ready" : "Deploy: Forbidden";
            return w;
        }

        private Color color(bool flag) => flag ? Color.green : Color.red;
    }
}