using System.Runtime.Serialization;

namespace GameStop.SupplyChain.ThinkGeekDataContracts
{
    [DataContract]
    public class Dimensions : IDimensions
    {
        [DataMember(Name = "weight", IsRequired = true)]
        public string weight { get; set; }
        [DataMember(Name = "weight_override", IsRequired = true)]
        public string weight_override { get; set; }
        [DataMember(Name = "length", IsRequired = true)]
        public string length { get; set; }
        [DataMember(Name = "height", IsRequired = true)]
        public string height { get; set; }
        [DataMember(Name = "depth", IsRequired = true)]
        public string depth { get; set; }
        [DataMember(Name = "volume", IsRequired = true)]
        public string volume { get; set; }
        [DataMember(Name = "case_weight", IsRequired = true)]
        public string case_weight { get; set; }
        [DataMember(Name = "case_length", IsRequired = true)]
        public string case_length { get; set; }
        [DataMember(Name = "case_height", IsRequired = true)]
        public string case_height { get; set; }
        [DataMember(Name = "case_depth", IsRequired = true)]
        public string case_depth { get; set; }
        [DataMember(Name = "case_volume", IsRequired = true)]
        public string case_volume { get; set; }

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
