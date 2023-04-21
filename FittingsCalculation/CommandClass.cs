using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using Autodesk.Windows;
using System;
using System.Windows.Input;

namespace FittingsCalculation
{
    public class CommandClass
    {

        public static string RunCommand2()
        {
            Document adoc = Application.DocumentManager.MdiActiveDocument;
            Database db = adoc.Database;
            Editor ed = adoc.Editor;
            while (true)
            {
                PromptEntityResult result = ed.GetEntity("\nВыберите размер: ");
                if (result.Status == PromptStatus.Cancel) return "Размер не выбран!";

                ObjectId enId = result.ObjectId;

                if (!enId.ObjectClass.Name.Equals("AcDbAlignedDimension"))
                {
                    ed.WriteMessage("\nВыбран объект: " + result.ObjectId.ObjectClass.Name);
                    continue;
                }
                using (DocumentLock docLock = adoc.LockDocument())
                {
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        AlignedDimension obj = tr.GetObject(enId, OpenMode.ForRead, false, true) as AlignedDimension;
                        BlockTable blockTable = tr.GetObject(db.BlockTableId, OpenMode.ForNotify) as BlockTable;
                        BlockTableRecord blockTableRes = null;
                        try
                        {
                            blockTableRes = tr.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                        }
                        catch (System.Exception ex) { ed.WriteMessage("Exep!"); return ex.Message; }

                        Point3d startPoint = obj.XLine1Point;
                        Point3d endPoint = obj.XLine2Point;
                        Point3d dimPoint = obj.DimLinePoint;

                        PromptPointResult pt1 = adoc.Editor.GetPoint("\nУкажите точку разделения: ");
                        Point3d sepPoint = pt1.Value;
                        if (pt1.Status == PromptStatus.Cancel) return "Точка не выбрана!";

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

        public static string GetSize()
        {
            Document adoc = Application.DocumentManager.MdiActiveDocument;

            PromptPointResult _pt1 = adoc.Editor.GetPoint("\nУкажите первую точку : ");
            Point3d pt1 = _pt1.Value;
            PromptPointResult _pt2 = adoc.Editor.GetPoint("\nУкажите первую точку : ");
            Point3d pt2 = _pt2.Value;

            return Math.Round(pt1.DistanceTo(pt2), 3).ToString();
        }

        public static string GetPolySize()
        {
            Document adoc = Application.DocumentManager.MdiActiveDocument;
            Database db = adoc.Database;
            Editor ed = adoc.Editor;

            double outRez = 0.0;

            while (true)
            {
                PromptEntityResult result = ed.GetEntity("\nВыберите объект: ");
                if (result.Status == PromptStatus.Cancel) break;

                ObjectId enId = result.ObjectId;

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    var obj = tr.GetObject(enId, OpenMode.ForRead, false, true);

                    switch (result.ObjectId.ObjectClass.Name)
                    {
                        case "AcDbCircle":
                            Circle circl = obj as Circle;
                            outRez += circl.Radius * 2 * Math.PI;
                            break;

                        case "AcDbEllipse":
                            Ellipse ellips = obj as Ellipse;
                            outRez += 2 * Math.PI * Math.Sqrt((Math.Pow(ellips.MajorRadius, 2) + Math.Pow(ellips.MinorRadius, 2)) / 2);
                            break;

                        case "AcDbLine":
                            Line line = obj as Line;
                            outRez += line.Length;
                            break;

                        case "AcDbPolyline":
                            Polyline polyLine = obj as Polyline;
                            outRez += polyLine.Length;
                            break;

                        case "AcDbArc":
                            Arc arc = obj as Arc;
                            outRez += arc.Length;
                            break;

                        case "AcDbSpline":
                            Curve spline = obj as Curve;
                            outRez += spline.GetDistanceAtParameter(spline.EndParam) - spline.GetDistanceAtParameter(spline.StartParam);
                            break;
                    }
                }
            }

            return Math.Round(outRez, 3).ToString();
        }

        [CommandMethod("testObj")]
        public static void Test()
        {
            Document adoc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = adoc.Editor;

            while (true)
            {
                PromptEntityResult result = ed.GetEntity("\nВыберите объект: ");
                if (result.Status == PromptStatus.Cancel) break;


                if (result.ObjectId.ObjectClass.Name.Equals("AcDbTable"))
                {
                    ed.WriteMessage("Выбрана не таблица!");
                    continue;
                }

                ObjectId enId = result.ObjectId;


            }
        }

        [CommandMethod("INSERTTABLEVALUE")]
        public void InsertTableValue()
        {
            // Получаем текущий документ и его базу данных
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            // Запрашиваем пользователю выбор ячейки таблицы
            PromptPointResult ptRes = ed.GetPoint("\nВыберите ячейку таблицы:");
            if (ptRes.Status != PromptStatus.OK)
            {
                ed.WriteMessage("Ошибка: не удалось выбрать ячейку таблицы");
                return;
            }

            // Открываем таблицу и ищем выбранную ячейку
            Table table = null;
            Cell cell = null;
            BlockTableRecord btr = null;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)tr.GetObject(doc.Database.BlockTableId, OpenMode.ForRead);
                btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);
                foreach (ObjectId entId in btr)
                {
                    Entity ent = (Entity)tr.GetObject(entId, OpenMode.ForRead);
                    if (ent is Table)
                    {
                        table = (Table)ent;
                        break;
                    }
                    if (ent is Cell)
                    {
                        Cell c = (Cell)ent;
                        Point3d ptCell = c.Position.TransformBy(ed.CurrentUserCoordinateSystem);
                        if (ptCell.X <= ptRes.Value.X && ptCell.X + c.BlockWidth > ptRes.Value.X && ptCell.Y <= ptRes.Value.Y && ptCell.Y + c.BlockHeight > ptRes.Value.Y)
                        {
                            cell = c;
                            break;
                        }
                    }
                }
                tr.Commit();
            }

            // Вставляем значение ячейки на чертеж в виде однострочного текста
            if (cell != null)
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                    DBText text = new DBText();
                    text.Position = cell.Position;
                    text.TextString = cell.Value.ToString();
                    text.Height = cell.Height;
                    text.Rotation = cell.Rotation;
                    btr.AppendEntity(text);
                    tr.AddNewlyCreatedDBObject(text, true);
                    tr.Commit();

                    ed.WriteMessage("Значение ячейки таблицы вставлено на чертеж в виде однострочного текста.");
                }
            }
            else
            {
                ed.WriteMessage("Ошибка: ячейка таблицы не найдена.");
            }
        }
        


        public class StartClass : IExtensionApplication
        {
            public void Initialize()
            {
                ModalWinow modalWinow = new ModalWinow();
                Application.ShowModelessWindow(modalWinow);
            }

            public void Terminate()
            {
                throw new NotImplementedException();
            }
        }
    }
}

