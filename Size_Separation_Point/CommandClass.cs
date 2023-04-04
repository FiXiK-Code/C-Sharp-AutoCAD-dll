using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using Autodesk.Windows;
using System;
using System.Windows.Input;

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
    public class YourPluginClass : IExtensionApplication
    {

        // создание новой панели
        RibbonPanelSource ribbonPanelSource = new RibbonPanelSource();
        RibbonPanel ribbonPanel = new RibbonPanel();

        // создание новой кнопки
        RibbonButton ribbonButton = new RibbonButton();

        public void Initialize()
        {
            

            // установка свойств панели
            ribbonPanelSource.Title = "Разделение размера";
            ribbonPanelSource.Id = "ID_SepPoint_Panel";
            ribbonPanelSource.Title = "Разделение размера";
            // добавление панели на вкладку Главная (Home)
            Document adoc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = adoc.Editor;
            RibbonControl ribbon = Autodesk.Windows.ComponentManager.Ribbon;
            
            RibbonTab homeTab = ribbon.FindTab("ACAD.ID_TabHome");
            
            // установка свойств кнопки
            ribbonButton.Id = "Id_SepPoint";
            ribbonButton.ToolTip = "Разделение размера на две части в указанной точке.";

            // добавление обработчика события при нажатии на кнопку
            ribbonButton.CommandParameter = "SeparationPoint";
            ribbonButton.CommandHandler = new YourCommandHandler();

            // добавление кнопки на созданную панель
            ribbonButton.Text = "Разделение";
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

        public void Terminate()
        {
            throw new NotImplementedException();
        }
    }

    public class YourCommandHandler : ICommand
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

