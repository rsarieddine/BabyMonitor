using Microsoft.PowerBI.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitor_Web.Models
{
   public class ReportViewModel
    {
        public IReport Report { get; set; }
        public string AccessToken { get; set; }
    }
}
