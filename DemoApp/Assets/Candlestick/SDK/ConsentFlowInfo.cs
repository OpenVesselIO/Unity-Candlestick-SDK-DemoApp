using System;
using System.Collections;

namespace Candlestick
{

    public class ConsentFlowExperimentInfo
    {

        public string InstallationId { get; }
        public string Extras { get; }

        public ConsentFlowExperimentInfo(string installationId, string extras)
        {
            InstallationId = installationId;
            Extras = extras;
        }

    }

    public class ConsentFlowInfo
    {

        public ConsentFlowExperimentInfo ExperimentInfo { get; }

        public ConsentFlowInfo(ConsentFlowExperimentInfo experimentInfo)
        {
            ExperimentInfo = experimentInfo;
        }

    }

}

