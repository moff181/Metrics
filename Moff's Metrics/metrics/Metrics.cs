using System.Collections.Generic;

namespace Moff_s_Metrics.metrics {
    abstract class Metrics {
        public Dictionary<string, float> Values { get; internal set; } = new Dictionary<string, float>();
    }
}
