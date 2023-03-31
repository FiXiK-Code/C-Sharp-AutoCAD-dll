using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System.Collections.Generic;

namespace Size_Separation_Point
{
    public class CommandClass
    {
        [CommandMethod("SeparationPoint")]
        public void RunCommand2()
        {
            Document adoc = Application.DocumentManager.MdiActiveDocument;
            Database db = adoc.Database;
            Editor ed = adoc.Editor;
            while (true)
            {
                PromptEntityResult result = ed.GetEntity("\nВыберите размер: ");
                if (result.Status == PromptStatus.Cancel) return;

                ObjectId enId = result.ObjectId;

                if (!enId.ObjectClass.Name.Equals("AcDbAlignedDimension"))
                {
                    ed.WriteMessage("\nВыбран объект: " + result.ObjectId.ObjectClass.Name);
                    continue;
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
