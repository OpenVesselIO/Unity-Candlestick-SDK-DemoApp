using System;
using System.Collections;

namespace Candlestick
{

    public class ConsentFlowExperimentInfo
    {

        public string InstallationId { get; }
        public string GenericFlow { get; }
        public string InitTimers { get; }

        public ConsentFlowExperimentInfo(string installationId, string genericFlow, string initTimers)
        {
            InstallationId = installationId;
            GenericFlow = genericFlow;
            InitTimers = initTimers;
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

