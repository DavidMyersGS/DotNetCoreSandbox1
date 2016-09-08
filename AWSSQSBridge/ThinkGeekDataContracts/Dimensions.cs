namespace GameStop.SupplyChain.DataContracts.ThinkGeek
{
    public class Dimensions
    {
        public string weight { get; set; }

        public string weight_override { get; set; }

        public string length { get; set; }

        public string height { get; set; }

        public string depth { get; set; }

        public string volume { get; set; }

        public string case_weight { get; set; }

        public string case_length { get; set; }

        public string case_height { get; set; }

        public string case_depth { get; set; }

        public string case_volume { get; set; }

        public Dimensions()
        {

        }

        public Dimensions(bool initDefaults = false)
        {
            if (initDefaults)
            {
                weight = ".45";
                weight_override = "0";
                length = "17";
                height = ".5";
                depth = "10";
                volume = "85";
                case_weight = "4.5";
                case_length = "170";
                case_height = "5";
                case_depth = "100";
                case_volume = "8500";
            }
        }
    }
}
