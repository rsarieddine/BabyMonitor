using System.Web;

namespace BabyMonitor_Web.Models
{
    public class ReportUploadModel
    {
        public string ReportName { get; set; }
        public HttpPostedFileBase PbixReport { get; set; }
        public string ReportCategory { get; set; }
        public string NewReportCategory { get; set; }
    }
}
