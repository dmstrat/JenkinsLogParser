using System;
using JenkinsLogParser.Reports;

namespace JenkinsLogParser.Events.DotCover
{
    public class DotCoverStarted : EventBase
    {
        public string ProjectName => RegExResult;
        public DateTime Timestamp { get; }
        public DotCoverAction Action { get; }

        public DotCoverStarted(DotCoverStartedEventArgs args) : base(args)
        {
            RegExResult = args.ProjectName;
            Timestamp = args.Timestamp;
            Action = args.Action;
        }
    }

    public class DotCoverStartedEventArgs : EventArgsBase
    {
        public string ProjectName { get; set; }
        public DateTime Timestamp { get; set; }
        public DotCoverAction Action { get; set; }
    }
}
