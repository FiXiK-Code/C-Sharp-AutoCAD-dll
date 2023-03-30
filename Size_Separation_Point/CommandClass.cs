using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Interop;
using Autodesk.AutoCAD.Interop.Common;
using Autodesk.AutoCAD.Runtime;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Size_Separation_Point
{
    public class CommandClass
    {
        [CommandMethod("TestCom")]
        public void RunCommand()
        {

            //получение списка слоев
            Document adoc = Application.DocumentManager.MdiActiveDocument;

            if (adoc == null) return;

            Database db = adoc.Database;

            ObjectId layerTableId = db.LayerTableId;

            List<string> LayerNames = new List<string>();

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                LayerTable layerTable = tr.GetObject(layerTableId, OpenMode.ForRead) as LayerTable;
                foreach (ObjectId layerId in layerTable)
                {
                    LayerTableRecord LTRecord = tr.GetObject(layerId, OpenMode.ForRead) as LayerTableRecord;

                    LayerNames.Add(LTRecord.Name);
                }



                tr.Commit();
            }

            Editor ed = adoc.Editor;
            foreach (string layerName in LayerNames)
            {
                ed.WriteMessage("\n" + layerName);
            }






            //рисование линии по двум точкам
            AcadApplication acApp = (AcadApplication)Marshal.GetActiveObject("AutoCad.Application");

            AcadDocument acaddoc = acApp.ActiveDocument;

            AcadUtility acUtilit = acaddoc.Utility;

            AcadModelSpace acModel = acaddoc.Database.ModelSpace;

            acaddoc.ActiveSpace = AcActiveSpace.acModelSpace;

            while (true)
            {
                double[] pt1 = acUtilit.GetPoint(Prompt: "\nУкажите первую точку: ");

                if (pt1 == null) return;

                double[] pt2 = acUtilit.GetPoint(pt1, "\nУкажите вторую точку: ");

                if (pt2 == null) return;

                double[] pt1wcs = acUtilit.TranslateCoordinates
                    (pt1, AcCoordinateSystem.acUCS, AcCoordinateSystem.acWorld, 0);

                double[] pt2wcs = acUtilit.TranslateCoordinates
                    (pt2, AcCoordinateSystem.acUCS, AcCoordinateSystem.acWorld, 0);

                AcadLine acLine = acModel.AddLine(pt1wcs, pt2wcs);

                acUtilit.Prompt("\nСоздан объект: " + acLine.Handle);
            }


        }

        [CommandMethod("SeparationPoint")]
        public void RunCommand2()
        {
            Document adoc = Application.DocumentManager.MdiActiveDocument;

            Database db = adoc.Database;

            Editor ed = adoc.Editor;

            PromptEntityResult result = ed.GetEntity("\nВыберите размер: ");
            if (result.Status != PromptStatus.OK) return;

            ObjectId enId = result.ObjectId;

            bool isCorrect = enId.IsValid
                && !enId.IsErased
                && enId.IsEffectivelyErased
                && enId.ObjectClass.Name.Equals("AcDbAlignedDimension");
            if (!isCorrect)
            {
                ed.WriteMessage("\nВыбран некорректный объект!");
                return;
            }

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                DBObject obj = tr.GetObject(enId, OpenMode.ForRead, false, true);




                tr.Commit();
            }

        }
    }
}
