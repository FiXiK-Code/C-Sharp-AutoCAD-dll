using Autodesk.AutoCAD.ApplicationServices.Core;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FittingsCalculation
{
    public static class CommandClass
    {
        [CommandMethod("FittingCalculation")]
        public static void ShowWPWWindow()
        {
            ModalWinow modalWinow = new ModalWinow();
            Application.ShowModalWindow(modalWinow);
        }
    }
}
