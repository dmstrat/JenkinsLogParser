using System;
using JenkinsLogParser.Reports;

namespace JenkinsLogParser.Events.DotCover
{
    public class DotCoverEnded : EventBase
    {
        public string ProjectName => RegExResult;
        public DateTime Timestamp { get; }
        public DotCoverAction Action { get; }

        public DotCoverEnded(DotCoverEndedEventArgs args) : base(args)
        {
            RegExResult = args.ProjectName;
            Timestamp = args.Timestamp;
            Action = args.Action;
        }
    }

    public class DotCoverEndedEventArgs : EventArgsBase
    {
        public string ProjectName { get; set; }
        public DateTime Timestamp { get; set; }
        public DotCoverAction Action { get; set; }
    }
}
