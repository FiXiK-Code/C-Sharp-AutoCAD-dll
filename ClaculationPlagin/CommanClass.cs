using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using Autodesk.Windows;
using System;
using System.Windows.Input;

namespace ClaculationPlagin
{
    public class CommanClass
    {
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

        public static string GetCoordinate(string nap)
        {
            Document adoc = Application.DocumentManager.MdiActiveDocument;

            PromptPointResult _pt1 = adoc.Editor.GetPoint("\nУкажите первую точку : ");
            Point3d pt1 = _pt1.Value;

            string output = null;

            switch (nap)
            {
                case "x":
                    output = pt1.X.ToString();
                    break;
                case "y":
                    output = pt1.Y.ToString();
                    break;
                case "z":
                    output = pt1.Z.ToString();
                    break;
            }

            return output;
        }

        public static string GetDimension()
        {
            Document adoc = Application.DocumentManager.MdiActiveDocument;
            Database db = adoc.Database;
            Editor ed = adoc.Editor;
            bool enter = false;
            while (!enter)
            {
                PromptEntityResult result = ed.GetEntity("\nВыберите размер: ");
                if (result.Status == PromptStatus.Cancel) return "Комманда была завершена!";

                ObjectId enId = result.ObjectId;

                if (!enId.ObjectClass.Name.Equals("AcDbAlignedDimension"))
                {
                    ed.WriteMessage("\nВыбран объект: " + result.ObjectId.ObjectClass.Name);
                    continue;
                }

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    AlignedDimension obj = tr.GetObject(enId, OpenMode.ForRead, false, true) as AlignedDimension;

                    Point3d startPoint = obj.XLine1Point;
                    Point3d endPoint = obj.XLine2Point;

                    enter = true;

                    return startPoint.DistanceTo(endPoint).ToString();
                }
            }
            return null;
        }

        public static void InsertTableResult(string insertText)
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

                    tr.Commit();
                }
            }
        }

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

        public static string GetTextValue()
        {
            //Выберите объект: AcDbMLeader выноска
            //Выберите объект: AcDbText текст
            //Выберите объект: AcDbMText М текст

            Document adoc = Application.DocumentManager.MdiActiveDocument;
            Database db = adoc.Database;
            Editor ed = adoc.Editor;
            bool enter = false;
            while (!enter)
            {
                PromptEntityResult result = ed.GetEntity("\nВыберите элемент: ");
                if (result.Status == PromptStatus.Cancel) return null;

                ObjectId enId = result.ObjectId;

                string classObj = enId.ObjectClass.Name;

                if (!classObj.Equals("AcDbMLeader") || !classObj.Equals("AcDbText") || !classObj.Equals("AcDbMText"))
                {
                    ed.WriteMessage("\nВыбран объект: " + result.ObjectId.ObjectClass.Name);
                    continue;
                }

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    if (classObj == "AcDbMLeader")
                    {
                        MLeader obj = tr.GetObject(enId, OpenMode.ForRead, false, true) as MLeader;
                        return obj.MText.Text;
                    }
                    if (classObj == "AcDbText")
                    {
                        DBText obj = tr.GetObject(enId, OpenMode.ForRead, false, true) as DBText;
                        return obj.TextString;
                    }
                    if (classObj == "AcDbMText")
                    {
                        MText obj = tr.GetObject(enId, OpenMode.ForRead, false, true) as MText;
                        return obj.Text;
                    }

                    enter = true;

                }
            }
            return null;
        }


        [CommandMethod("MLeaderInsert")]
        public void MLeaderInsert()
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            // Запрос на выбор точки вставки
            PromptPointOptions insPointPrompt = new PromptPointOptions("\nВыберите точку вставки: ");
            PromptPointResult insPointResult = acDoc.Editor.GetPoint(insPointPrompt);
            if (insPointResult.Status != PromptStatus.OK) return;

            // Запрос на выбор точки положения мультивыноски
            PromptPointOptions mLeaderPointPrompt = new PromptPointOptions("\nВыберите точку положения мультивыноски: ");
            PromptPointResult mLeaderPointResult = acDoc.Editor.GetPoint(mLeaderPointPrompt);
            if (mLeaderPointResult.Status != PromptStatus.OK) return;

            // Создание объекта мультивыноски
            MLeader mLeader = new MLeader();

            // Создание объекта MultileaderStyle для задания стиля мультивыноски
            DBDictionary mLeaderStyles = acCurDb.MLeaderStyleDictionaryId.GetObject(OpenMode.ForRead) as DBDictionary;
            //MultileaderStyle mLeaderStyle = mLeaderStyles.GetAt("Standard").GetObject(OpenMode.ForRead) as MultileaderStyle;
            //mLeader.Style = mLeaderStyle;

            // Создание и заполнение объекта MText
            MText mtext = new MText();
            mtext.Contents = "проверено";
            mLeader.MText = mtext;

            // Задание точки вставки и точки положения мультивыноски
            //mLeader.Location = insPointResult.Value;
            //mLeader.SetLeaderLinePoints(new Point3dCollection() { mLeaderPointResult.Value });


            // Добавление созданной мультивыноски в пространство модели
            BlockTableRecord acBlkTblRec;
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                acBlkTblRec = acTrans.GetObject(acCurDb.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;
                acBlkTblRec.AppendEntity(mLeader);
                acTrans.AddNewlyCreatedDBObject(mLeader, true);
                acTrans.Commit();
            }
            // Обновление экрана и вывод сообщения об успешном создании объекта
            acDoc.Editor.Regen();
            acDoc.Editor.WriteMessage("\nMLeader успешно вставлен");
        }



        [CommandMethod("_mpCalc")]
        public static void Satart()
        {
            AddPanel.StartWindow();
        }

        

        public class AddPanel : IExtensionApplication
        {

            // создание новой панели
            RibbonPanelSource ribbonPanelSource = new RibbonPanelSource();
            RibbonPanel ribbonPanel = new RibbonPanel();

            // создание новой кнопки
            RibbonButton ribbonButton = new RibbonButton();

            public void Initialize()
            {
                // установка свойств панели
                ribbonPanelSource.Title = "Калькулятор";
                ribbonPanelSource.Id = "Id_pmCalc_Panel";
                // добавление панели на вкладку Главная (Home)
                Document adoc = Application.DocumentManager.MdiActiveDocument;
                Editor ed = adoc.Editor;
                RibbonControl ribbon = Autodesk.Windows.ComponentManager.Ribbon;

                RibbonTab homeTab = ribbon.FindTab("ACAD.ID_TabHome");

                // установка свойств кнопки
                ribbonButton.Id = "Id_mpCalc";
                ribbonButton.ToolTip = "Калькулятор для работы с чертежом.";

                // добавление обработчика события при нажатии на кнопку
                ribbonButton.CommandParameter = "_mpCalc";
                ribbonButton.CommandHandler = new ButtonClickCommand();

                // добавление кнопки на созданную панель
                ribbonButton.Text = "Калькулятор";
                try
                {
                    ribbonPanelSource.Items.Add(ribbonButton);
                }
                catch (System.Exception ex) { ed.WriteMessage(ex.Message + "\n Error on ribbonPanelSource.Items.Add(ribbonButton);"); return; }

                try
                {
                    ribbonPanel.Source = ribbonPanelSource;
                }
                catch (System.Exception ex) { ed.WriteMessage(ex.Message + "\n Error on ribbonPanel.Source = ribbonPanelSource;"); return; }

                try
                {
                    homeTab.Panels.Add(ribbonPanel);
                }
                catch (System.Exception ex) { ed.WriteMessage(ex.Message + "\n Error on homeTab.Panels.Add(ribbonPanel);"); return; }

            }

            public static void StartWindow()
            {
                MainWindow mainWinow = new MainWindow();
                Application.ShowModelessWindow(mainWinow);
            }

            public void Terminate()
            {
                throw new NotImplementedException();
            }
        }

        public class ButtonClickCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                Document doc = Application.DocumentManager.MdiActiveDocument;
                if (parameter is RibbonButton)
                {
                    // Просто берем команду, записанную в CommandParameter кнопки
                    // и выпоняем её используя функцию SendStringToExecute
                    RibbonButton button = parameter as RibbonButton;
                    Application.DocumentManager.MdiActiveDocument.SendStringToExecute(
                        button.CommandParameter + " ", true, false, true);
                }
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

        }
    }

    
}
