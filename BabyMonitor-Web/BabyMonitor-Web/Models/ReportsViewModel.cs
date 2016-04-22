using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Beta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BabyMonitor_Web.Models
{
    public class ReportsViewModel
    {
        public IList<ReportWorkspace> Workspaces { get; set; }
    }
    public class ReportWorkspace
    {
        public string WorkspaceId { get; set; }
        public string DisplayName { get; set; }
        public IList<Report> Reports { get; set; }
    }
}

