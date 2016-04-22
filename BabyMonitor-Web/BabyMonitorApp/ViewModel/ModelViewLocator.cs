using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApp.ViewModel
{
    public class ModelViewLocator
    {
        public ModelViewLocator()
        {
            Main = new MainView();
        }

        public MainView Main { get; set; }

    }
}
