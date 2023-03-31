using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
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
            

            while (true)
            {
                PromptPointResult pt1 = adoc.Editor.GetPoint("\nУкажите первую точку: ");
                Point3d startPoint = pt1.Value;
                if (pt1.Status == PromptStatus.Cancel) return;
                

                PromptPointResult pt2 = adoc.Editor.GetPoint("\nУкажите вторую точку: ");
                Point3d endPoint = pt2.Value;
                if (pt2.Status == PromptStatus.Cancel) return;

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    BlockTable blockTable;
                    BlockTableRecord blockTableRes;


                    blockTable = tr.GetObject(db.BlockTableId, OpenMode.ForNotify) as BlockTable;

                    blockTableRes = tr.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;


                    using (Line acLine = new Line(startPoint,endPoint))
                    {
                        blockTableRes.AppendEntity(acLine);
                        tr.AddNewlyCreatedDBObject(acLine, true);
                    }

                    PromptPointResult pt3 = adoc.Editor.GetPoint("\nУкажите вылет размера: ");
                    Point3d ptDim = pt3.Value;
                    if (pt3.Status == PromptStatus.Cancel) return;

                    using (AlignedDimension newDim = new AlignedDimension(startPoint, endPoint, ptDim, null, default))
                    {
                        blockTableRes.AppendEntity(newDim);
                        tr.AddNewlyCreatedDBObject(newDim, true);
                    }



                        tr.Commit();
                }


            }


        }

        [CommandMethod("SeparationPoint")]
        public void RunCommand2()
        {
            Document adoc = Application.DocumentManager.MdiActiveDocument;
            Database db = adoc.Database;
            Editor ed = adoc.Editor;
            while (true)
            {
                PromptEntityResult result = ed.GetEntity("\nВыберите размер: ");
                if (result.Status == PromptStatus.Cancel)
                {
                    return;
                }

                ObjectId enId = result.ObjectId;

                if (!enId.ObjectClass.Name.Equals("AcDbAlignedDimension"))
                {
                    ed.WriteMessage("\nВыбран объект: " + result.ObjectId.ObjectClass.Name);
                    return;
                }



                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    AlignedDimension obj = tr.GetObject(enId, OpenMode.ForRead, false, true) as AlignedDimension;
                    BlockTable blockTable = tr.GetObject(db.BlockTableId, OpenMode.ForNotify) as BlockTable;
                    BlockTableRecord blockTableRes = tr.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;



                    Point3d startPoint = obj.XLine1Point;
                    Point3d endPoint = obj.XLine2Point;
                    Point3d dimPoint = obj.DimLinePoint;



                    PromptPointResult pt1 = adoc.Editor.GetPoint("\nУкажите точку разделения: ");
                    Point3d sepPoint = pt1.Value;
                    ed.WriteMessage("\nString result: " + pt1.StringResult);
                    if (pt1.Status == PromptStatus.Cancel) return;


                    sepPoint = GetProjectionOnLine(sepPoint, startPoint, endPoint);
                    if (sepPoint == new Point3d())
                    {
                        ed.WriteMessage("\nТочка за пределами отрезка!");
                    }
                    else
                    {
                        obj.UpgradeOpen();
                        obj.Erase();

                        using (AlignedDimension newDim = new AlignedDimension(startPoint, sepPoint, dimPoint, null, default))
                        {
                            blockTableRes.AppendEntity(newDim);
                            tr.AddNewlyCreatedDBObject(newDim, true);
                        }

                        using (AlignedDimension newDim = new AlignedDimension(sepPoint, endPoint, dimPoint, null, default))
                        {
                            blockTableRes.AppendEntity(newDim);
                            tr.AddNewlyCreatedDBObject(newDim, true);
                        }
                    }

                    tr.Commit();
                }
            }
            

        }

        public static Point3d GetProjectionOnLine(Point3d point, Point3d startPoint, Point3d endPoint)
        {
            var line = new Line(startPoint, endPoint);
            var projectionOnStartToEnd = point - startPoint;
            var startToEnd = endPoint - startPoint;
            var direction = startToEnd.GetNormal();
            var projectionOnDirection = projectionOnStartToEnd.DotProduct(direction) * direction;

            if (startPoint.DistanceTo(startPoint + projectionOnDirection) >= startPoint.DistanceTo(endPoint)
                || endPoint.DistanceTo(startPoint) <= endPoint.DistanceTo(startPoint + projectionOnDirection))
            {
                return new Point3d();
            }

            return startPoint + projectionOnDirection;
        }
    }
}
