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
    /// <summary>
    /// Класс, содержащий функции для взаимодействия с чертежом.
    /// </summary>
    public class CommandClass
    {
        /// <summary>
        /// Метод для взаимодействия с чертежом.
        /// </summary>
        /// <returns> Ращделяем выбранный размер в указанной точке на два размера.</returns>
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

        /// <summary>
        /// Дополнительный метод для работы с точками на чертеже.
        /// </summary>
        /// <param name="point"> Указанная точка</param>
        /// <param name="startPoint"> Точка начала отрезка</param>
        /// <param name="endPoint"> Точка конца отрезка</param>
        /// <returns>Возвращает точку проекции указанной точки на указанный отрезок</returns>
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

        /// <summary>
        /// Методя для получения расстояния между точками
        /// </summary>
        /// <returns>Возвращает значение с точностью до 3-х знаков в формате строки(string)</returns>
        public static string GetSize()
        {
            Document adoc = Application.DocumentManager.MdiActiveDocument;

            PromptPointResult _pt1 = adoc.Editor.GetPoint("\nУкажите первую точку : ");
            Point3d pt1 = _pt1.Value;
            PromptPointResult _pt2 = adoc.Editor.GetPoint("\nУкажите первую точку : ");
            Point3d pt2 = _pt2.Value;

            return Math.Round(pt1.DistanceTo(pt2), 3).ToString();
        }

        /// <summary>
        /// Метод для получения длинны выбранного ообъекта
        /// </summary>
        /// <returns>Возвращает длинну окружности, элипса, линии, полилинии, дуги и сплайна в зависимости от того, какой элемент выбран на чертеже</returns>
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


        /// <summary>
        /// Метод для тестовых проверок. Например получение мнногострочного текста.
        /// </summary>
        [CommandMethod("testObj")]
        public static void Test()
        {
            Document adoc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = adoc.Editor;
            Database db = adoc.Database;

            while (true)
            {
                PromptEntityResult result = ed.GetEntity("\nВыберите объект: \n");
                if (result.Status == PromptStatus.Cancel) break;

                ed.WriteMessage(result.ObjectId.ObjectClass.Name);

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    MText obj = tr.GetObject(result.ObjectId, OpenMode.ForRead, false, true) as MText;

                    ed.WriteMessage("\n" + obj.Text);
                }
            }
        }

       
        /// <summary>
        /// Метод для вставки значения в ячейку таблицы.
        /// </summary>
        /// <param name="naemFiting"> Текст для вставки названия арматуры </param>
        /// <param name="result"> Текст для вставки значеия результата вычислений</param>
        public static void InsertTableText(string naemFiting, string result)
        {
            // Получаем текущий документ и базу данных
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            // Запрос у пользователя выбора таблицы
            PromptEntityOptions peo = new PromptEntityOptions("\nВыберите таблицу: ");
            peo.SetRejectMessage("\nВыберите только объекты таблицы.");
            peo.AddAllowedClass(typeof(Table), false);
            PromptEntityResult per = ed.GetEntity(peo);
            if (per.Status != PromptStatus.OK) return;
            Table table = null;
            using (DocumentLock docLock = doc.LockDocument())
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    table = (Table)tr.GetObject(per.ObjectId, OpenMode.ForRead);

                    PromptPointOptions ppo = new PromptPointOptions("\nВыберите ячейку: ");
                    PromptPointResult ppr = ed.GetPoint(ppo);
                    if (ppr.Status != PromptStatus.OK) return;
                    Point3d pt = ppr.Value;

                    // Поиск ячейки
                    Cell cell = FindCell(table, pt);
                    if (cell == null)
                    {
                        ed.WriteMessage("\nНе удалось определить ячейку.");
                        return;
                    }

                    table.UpgradeOpen();
                    table.Cells[cell.Row, cell.Column].TextString = naemFiting;
                    table.Cells[cell.Row, table.Columns.Count -1].TextString = result;

                    tr.Commit();
                }
            }
        }

        /// <summary>
        /// Методя для вставки результата вычислений с пометкой "кг/п.м." - килограмм на погонный метр.
        /// </summary>
        /// <param name="insertText">Текст для вставки значения результата</param>
        /// <param name="_1pm">Вставлять ли в соседнюю ячейку "кг/п.м."</param>
        public static void InsertTableText(string insertText, bool _1pm)
        {
            // Получаем текущий документ и базу данных
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            // Запрос у пользователя выбора таблицы
            PromptEntityOptions peo = new PromptEntityOptions("\nВыберите таблицу: ");
            peo.SetRejectMessage("\nВыберите только объекты таблицы.");
            peo.AddAllowedClass(typeof(Table), false);
            PromptEntityResult per = ed.GetEntity(peo);
            if (per.Status != PromptStatus.OK) return;
            Table table = null;
            using (DocumentLock docLock = doc.LockDocument())
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    table = (Table)tr.GetObject(per.ObjectId, OpenMode.ForRead);
                    
                    PromptPointOptions ppo = new PromptPointOptions("\nВыберите ячейку: ");
                    PromptPointResult ppr = ed.GetPoint(ppo);
                    if (ppr.Status != PromptStatus.OK) return;
                    Point3d pt = ppr.Value;

                    // Поиск ячейки
                    Cell cell = FindCell(table, pt);
                    if (cell == null)
                    {
                        ed.WriteMessage("\nНе удалось определить ячейку.");
                        return;
                    }

                    table.UpgradeOpen();
                    table.Cells[cell.Row, cell.Column].TextString = insertText;
                    if( _1pm == true  ) table.Cells[cell.Row, cell.Column+1].TextString = "кг/п.м";

                    tr.Commit();
                }
            }
        }


        /// <summary>
        /// Всопогательный метод для поиска ячейки в таблице.
        /// </summary>
        /// <param name="table"> Таблица в которой происходит поиск</param>
        /// <param name="pt"> Точка нажатия</param>
        /// <returns> Если ячейка найдена - возвращает ячейку. Иначе - null.</returns>
        private static Cell FindCell(Table table, Point3d pt)
        {
            int numRows = table.Rows.Count;
            int numCols = table.Columns.Count;

            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    Cell cell = table.Cells[row, col];
                    Point3dCollection cellExtents = cell.GetExtents();

                    Extents3d bbox = new Extents3d();
                    bbox.AddPoint(cellExtents[0]);
                    bbox.AddPoint(cellExtents[3]);

                    if (pt.X > bbox.MinPoint.X && pt.X < bbox.MaxPoint.X &&
                        pt.Y > bbox.MinPoint.Y && pt.Y < bbox.MaxPoint.Y)
                    {
                        // Найдена ячейка
                        return cell;
                    }
                }
            }

            // Ячейка не найдена
            return null;
        }

        /// <summary>
        /// Метод для получения значения из ячейки таблицы.
        /// </summary>
        /// <returns>Если ячейка существует - возвращает строку с контентом.</returns>
        public static string GetTableValue()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            // Запрос у пользователя выбора таблицы
            PromptEntityOptions peo = new PromptEntityOptions("\nВыберите таблицу: ");
            peo.SetRejectMessage("\nВыберите только объекты таблицы.");
            peo.AddAllowedClass(typeof(Table), false);
            PromptEntityResult per = ed.GetEntity(peo);
            if (per.Status != PromptStatus.OK) return null;
            Table table = null;
            using (DocumentLock docLock = doc.LockDocument())
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    table = (Table)tr.GetObject(per.ObjectId, OpenMode.ForRead);

                    PromptPointOptions ppo = new PromptPointOptions("\nВыберите ячейку: ");
                    PromptPointResult ppr = ed.GetPoint(ppo);
                    if (ppr.Status != PromptStatus.OK) return null;
                    Point3d pt = ppr.Value;

                    // Поиск ячейки
                    Cell cell = FindCell(table, pt);
                    if (cell == null)
                    {
                        ed.WriteMessage("\nНе удалось определить ячейку.");
                        return null;
                    }
                    tr.Commit();

                    return table.Cells[cell.Row, cell.Column].TextString;
                }
            }
        }



        /// <summary>
        /// Класс для инициализации и отрисовки модального окна.
        /// </summary>
        public class StartClass : IExtensionApplication
        {
            /// <summary>
            /// Метод для запуска отрисоки модального окна.
            /// </summary>
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

