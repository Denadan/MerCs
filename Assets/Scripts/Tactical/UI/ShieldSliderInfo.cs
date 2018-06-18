namespace Mercs.Tactical.UI
{
    public class ShieldSliderInfo : HpSliderInfo
    {
        public override string MakeText(float value)
        {
            if (level == Visibility.Level.Visual)
                switch (value / MaxValue)
                {
                    case float i when i >= 0.95f:
                        return "FULL";
                    case float i when i >= 0.6f:
                        return "HIGH";
                    case float i when i >= 0.2f:
                        return "LOW";
                    case float i when i > 0:
                        return "CRITICAL";
                    default:
                        return "DEPLETED";
                }
            else
                return base.MakeText(value);
        }
    }
}