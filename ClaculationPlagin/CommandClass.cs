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
    /// <summary>
    /// Класс, содержащий функции для взаимодействия с чертежом.
    /// </summary>
    public class CommandClass
    {
        /// <summary>
        /// Метод для запуска отрисовки модального окна по стредствам выполнения комманды в консоли.
        /// </summary>
        [CommandMethod("_mpCalc")]
        public static void Satart()
        {
            AddPanel.StartWindow();
        }


        #region Input //////////

        /// <summary>
        /// Методя для получния расстояния между точками
        /// </summary>
        /// <returns>Расстояние между точками в формате строки(string) без округления</returns>
        public static string GetSize(string znac)
        {

            Document adoc = Application.DocumentManager.MdiActiveDocument;
            double sum = 0.0;
            if(znac != null)
            {
                while (true)
                {
                    PromptPointResult _pt1 = adoc.Editor.GetPoint("\nУкажите первую точку : ");
                    if (_pt1.Status == PromptStatus.Cancel) break;

                    Point3d pt1 = _pt1.Value;

                    PromptPointResult _pt2 = adoc.Editor.GetPoint("\nУкажите вторую точку : ");
                    if (_pt2.Status == PromptStatus.Cancel) break;

                    Point3d pt2 = _pt2.Value;
                    if(sum == 0.0)
                    {
                        sum += pt1.DistanceTo(pt2);
                        continue;
                    }
                    switch (znac)
                    {
                        case "+":
                            sum += pt1.DistanceTo(pt2);
                            break;
                        case "-":
                            sum -= pt1.DistanceTo(pt2);
                            break;
                        case "*":
                            sum *= pt1.DistanceTo(pt2);
                            break;
                        case "/":
                            sum /= pt1.DistanceTo(pt2);
                            break;
                    }
                   
                }
            }
            else
            {
                PromptPointResult _pt1 = adoc.Editor.GetPoint("\nУкажите первую точку : ");
                Point3d pt1 = _pt1.Value;
                PromptPointResult _pt2 = adoc.Editor.GetPoint("\nУкажите вторую точку : ");
                Point3d pt2 = _pt2.Value;

                sum = pt1.DistanceTo(pt2);
            }
            

            return sum.ToString();
        }

        /// <summary>
        /// Метод для получения суммы длин выбранных объектов 
        /// </summary>
        /// <returns>Возвращает длинну окружности, элипса, линии, полилинии, дуги и сплайна в зависимости от того, какой элемент выбран на чертеже, без округления</returns>
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

            return outRez.ToString();
        }

        /// <summary>
        /// Метод для получения координаты
        /// </summary>
        /// <param name="nap">Необхадима координата - x, y или z</param>
        /// <returns>Координы выбранной точки x, y или z</returns>
        public static string GetCoordinate(string nap, string znac)
        {
            Document adoc = Application.DocumentManager.MdiActiveDocument;

            

            double output = 0.0;
            if (znac != null)
            {
                while (true)
                {
                    PromptPointResult _pt1 = adoc.Editor.GetPoint("\nУкажите точку : ");
                    if (_pt1.Status == PromptStatus.Cancel) break;

                    Point3d pt1 = _pt1.Value;
                    if (output == 0.0)
                    {
                        switch (nap)
                        {
                            case "x":
                                output += pt1.X;
                                break;
                            case "y":
                                output += pt1.Y;
                                break;
                            case "z":
                                output += pt1.Z;
                                break;
                        }
                        continue;
                    }
                    switch (znac)
                    {
                        case "+":
                            switch (nap)
                            {
                                case "x":
                                    output += pt1.X;
                                    break;
                                case "y":
                                    output += pt1.Y;
                                    break;
                                case "z":
                                    output += pt1.Z;
                                    break;
                            }
                            break;
                        case "-":
                            switch (nap)
                            {
                                case "x":
                                    output -= pt1.X;
                                    break;
                                case "y":
                                    output -= pt1.Y;
                                    break;
                                case "z":
                                    output -= pt1.Z;
                                    break;
                            }
                            break;
                        case "*":
                            switch (nap)
                            {
                                case "x":
                                    output *= pt1.X;
                                    break;
                                case "y":
                                    output *= pt1.Y;
                                    break;
                                case "z":
                                    output *= pt1.Z;
                                    break;
                            }
                            break;
                        case "/":
                            switch (nap)
                            {
                                case "x":
                                    output /= pt1.X;
                                    break;
                                case "y":
                                    output /= pt1.Y;
                                    break;
                                case "z":
                                    output /= pt1.Z;
                                    break;
                            }
                            break;
                    }
                   
                }
            }
            else
            {
                PromptPointResult _pt1 = adoc.Editor.GetPoint("\nУкажите точку : ");
                Point3d pt1 = _pt1.Value;
                switch (nap)
                {
                    case "x":
                        output = pt1.X;
                        break;
                    case "y":
                        output = pt1.Y;
                        break;
                    case "z":
                        output = pt1.Z;
                        break;
                }
            }
            

            return output.ToString();
        }


        /// <summary>
        /// Метод для полечения значения размера.
        /// </summary>
        /// <returns>Значение размера в формате строки (string), без округления</returns>
        public static string GetDimension(string znac)
        {
            Document adoc = Application.DocumentManager.MdiActiveDocument;
            Database db = adoc.Database;
            Editor ed = adoc.Editor;

            double sum = 0.0;

            if(znac != null)
            {
                while (true)
                {
                    PromptEntityResult result = ed.GetEntity("\nВыберите размер: ");
                    if (result.Status == PromptStatus.Cancel) break;

                    ObjectId enId = result.ObjectId;

                    if (!enId.ObjectClass.Name.Equals("AcDbAlignedDimension"))
                    {
                        ed.WriteMessage("\nВыбран не размер!");
                        continue;
                    }

                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        AlignedDimension obj = tr.GetObject(enId, OpenMode.ForRead, false, true) as AlignedDimension;

                        Point3d startPoint = obj.XLine1Point;
                        Point3d endPoint = obj.XLine2Point;

                        if (sum == 0.0)
                        {
                            sum += startPoint.DistanceTo(endPoint);
                            continue;
                        }

                        switch (znac)
                        {
                            case "+":
                                sum += startPoint.DistanceTo(endPoint);
                                break;
                            case "-":
                                sum -= startPoint.DistanceTo(endPoint);
                                break;
                            case "*":
                                sum *= startPoint.DistanceTo(endPoint);
                                break;
                            case "/":
                                sum /= startPoint.DistanceTo(endPoint);
                                break;
                        }
                       
                    }
                }
                return sum.ToString();
            }
            else
            {
                while (true)
                {
                    PromptEntityResult result = ed.GetEntity("\nВыберите размер: ");
                    if (result.Status == PromptStatus.Cancel) return "Комманда была завершена!";

                    ObjectId enId = result.ObjectId;

                    if (!enId.ObjectClass.Name.Equals("AcDbAlignedDimension"))
                    {
                        ed.WriteMessage("\nВыбран не размер!");
                        continue;
                    }

                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        AlignedDimension obj = tr.GetObject(enId, OpenMode.ForRead, false, true) as AlignedDimension;

                        Point3d startPoint = obj.XLine1Point;
                        Point3d endPoint = obj.XLine2Point;

                        return startPoint.DistanceTo(endPoint).ToString();
                        break;
                    }
                }
                return null;
            }
        }


        /// <summary>
        /// Метод для получения значения из ячейки таблицы.
        /// </summary>
        /// <returns>Если ячейка существует - возвращает строку с контентом.</returns>
        public static string GetTableValue(string znac)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            double sum = 0.0;
            string altrsSum = "";
            bool alter = false;

            if (znac != null)
            {
                // добавить проверку и сообщение на вставку всей строки для ручного редактирования
                while (true)
                {
                    PromptEntityOptions peo = new PromptEntityOptions("\nВыберите таблицу: ");
                    peo.SetRejectMessage("\nВыберите только объекты таблицы.");
                    peo.AddAllowedClass(typeof(Table), false);
                    PromptEntityResult per = ed.GetEntity(peo);
                    if (per.Status != PromptStatus.OK) break;
                    Table table = null;
                    using (DocumentLock docLock = doc.LockDocument())
                    {
                        using (Transaction tr = db.TransactionManager.StartTransaction())
                        {
                            table = (Table)tr.GetObject(per.ObjectId, OpenMode.ForRead);

                            while (true)
                            {
                                PromptPointOptions ppo = new PromptPointOptions("\nВыберите ячейку: ");
                                PromptPointResult ppr = ed.GetPoint(ppo);
                                if (ppr.Status != PromptStatus.OK) break;
                                Point3d pt = ppr.Value;

                                // Поиск ячейки
                                Cell cell = FindCell(table, pt);
                                if (cell == null)
                                {
                                    ed.WriteMessage("\nНе удалось определить ячейку.");
                                    continue;
                                }
                                if (sum == 0.0)
                                {
                                    try
                                    {
                                        if (!alter) sum += Convert.ToDouble(table.Cells[cell.Row, cell.Column].TextString);
                                        altrsSum += table.Cells[cell.Row, cell.Column].TextString + "+";
                                    }
                                    catch (System.Exception)
                                    {
                                        System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("Значение в ячейке содержит не только число.\nВыполнить считывание контента для ручного редактирования?", "Ошибка считывания", System.Windows.MessageBoxButton.YesNo);
                                        if (result == System.Windows.MessageBoxResult.Yes)
                                        {
                                            altrsSum += table.Cells[cell.Row, cell.Column].TextString + "+";
                                            alter = true;
                                        }
                                        else
                                        {
                                            return null;
                                        }
                                    }
                                    continue;
                                }

                                switch (znac)
                                {
                                    case "+":
                                        try
                                        {
                                            if(!alter)sum += Convert.ToDouble(table.Cells[cell.Row, cell.Column].TextString);
                                            altrsSum += table.Cells[cell.Row, cell.Column].TextString +"+";
                                        }
                                        catch (System.Exception) 
                                        {
                                            System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("Значение в ячейке содержит не только число.\nВыполнить считывание контента для ручного редактирования?", "Ошибка считывания", System.Windows.MessageBoxButton.YesNo);
                                            if (result == System.Windows.MessageBoxResult.Yes)
                                            {
                                                altrsSum += table.Cells[cell.Row, cell.Column].TextString + "+";
                                                alter = true;
                                            }
                                            else
                                            {
                                                return null;
                                            }
                                        }
                                        break;
                                    case "-":
                                        try
                                        {
                                            if (!alter) sum -= Convert.ToDouble(table.Cells[cell.Row, cell.Column].TextString);
                                            altrsSum += table.Cells[cell.Row, cell.Column].TextString+"-";
                                        }
                                        catch (System.Exception)
                                        {
                                            System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("Значение в ячейке содержит не только число.\nВыполнить считывание контента для ручного редактирования?", "Ошибка считывания", System.Windows.MessageBoxButton.YesNo);
                                            if (result == System.Windows.MessageBoxResult.Yes)
                                            {
                                                altrsSum += table.Cells[cell.Row, cell.Column].TextString + "-";
                                                alter = true;
                                            }
                                            else
                                            {
                                                return null;
                                            }
                                        }
                                        break;
                                    case "*":
                                        try
                                        {
                                            if (!alter) sum *= Convert.ToDouble(table.Cells[cell.Row, cell.Column].TextString);
                                            altrsSum += table.Cells[cell.Row, cell.Column].TextString+"*";
                                        }
                                        catch (System.Exception)
                                        {
                                            System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("Значение в ячейке содержит не только число.\nВыполнить считывание контента для ручного редактирования?", "Ошибка считывания", System.Windows.MessageBoxButton.YesNo);
                                            if (result == System.Windows.MessageBoxResult.Yes)
                                            {
                                                altrsSum += table.Cells[cell.Row, cell.Column].TextString + "*";
                                                alter = true;
                                            }
                                            else
                                            {
                                                return null;
                                            }
                                        }
                                        break;
                                    case "/":
                                        try
                                        {
                                            if (!alter) sum /= Convert.ToDouble(table.Cells[cell.Row, cell.Column].TextString);
                                            altrsSum +=  table.Cells[cell.Row, cell.Column].TextString+"/";
                                        }
                                        catch (System.Exception)
                                        {
                                            System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("Значение в ячейке содержит не только число.\nВыполнить считывание контента для ручного редактирования?", "Ошибка считывания", System.Windows.MessageBoxButton.YesNo);
                                            if (result == System.Windows.MessageBoxResult.Yes)
                                            {
                                                altrsSum += table.Cells[cell.Row, cell.Column].TextString + "/";
                                                alter = true;
                                            }
                                            else
                                            {
                                                return null;
                                            }
                                        }
                                        break;
                                }
                            }
                            
                        }
                    }
                }
                return altrsSum == "" ? sum.ToString() : altrsSum;
            }
            else
            {
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
                        

                        try
                        {
                            sum = Convert.ToDouble(table.Cells[cell.Row, cell.Column].TextString);
                        }
                        catch (System.Exception)
                        {
                            System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("Значение в ячейке содержит не только число.\nВыполнить считывание контента для ручного редактирования?", "Ошибка считывания", System.Windows.MessageBoxButton.YesNo);
                            if (result == System.Windows.MessageBoxResult.Yes)
                            {
                                altrsSum = table.Cells[cell.Row, cell.Column].TextString;
                            }
                            else
                            {
                                return null;
                            }
                        }
                        return altrsSum == "" ? sum.ToString() : altrsSum;
                    }
                }
            }

            
        }


        /// <summary>
        /// Метод для получения значения из однострочного/многострочного текста или выноски.
        /// </summary>
        /// <param name="onlyText">Считывание с текста или с выноски</param>
        /// <returns>Если удалось считать текст - возвращает его.</returns>
        public static string GetTextValue(bool onlyText, string znac)
        {
            //Выберите объект: AcDbMLeader выноска
            //Выберите объект: AcDbText текст
            //Выберите объект: AcDbMText М текст
            Document adoc = Application.DocumentManager.MdiActiveDocument;
            Database db = adoc.Database;
            Editor ed = adoc.Editor;

            double sum = 0.0;
            bool alter = false;
            string alterSum = "";

            if (znac != null)
            {
                if (onlyText)
                {
                    while (true)
                    {
                        PromptEntityResult result = ed.GetEntity("\nВыберите элемент: ");
                        if (result.Status == PromptStatus.Cancel) break;

                        ObjectId enId = result.ObjectId;

                        string classObj = enId.ObjectClass.Name;

                        if (!classObj.Equals("AcDbText") && !classObj.Equals("AcDbMText"))
                        {
                            ed.WriteMessage("\nВыбран объект: " + result.ObjectId.ObjectClass.Name);
                            continue;
                        }

                        using (Transaction tr = db.TransactionManager.StartTransaction())
                        {
                            if (classObj == "AcDbText")
                            {
                                DBText obj = tr.GetObject(enId, OpenMode.ForRead, false, true) as DBText;
                                if (sum == 0.0)
                                {
                                    try
                                    {
                                        if (!alter) sum += Convert.ToDouble(obj.TextString);
                                        alterSum += obj.TextString + "+";
                                    }
                                    catch (System.Exception)
                                    {
                                        System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show("Значение в объекте содержит не только число.\nВыполнить считывание контента для ручного редактирования?", "Ошибка считывания", System.Windows.MessageBoxButton.YesNo);
                                        if (res == System.Windows.MessageBoxResult.Yes)
                                        {
                                            alterSum += obj.TextString + "+";
                                            alter = true;
                                        }
                                        else
                                        {
                                            return null;
                                        }
                                    }
                                    continue;
                                }
                                switch (znac)
                                {
                                    case "+":
                                        try
                                        {
                                            if(!alter) sum += Convert.ToDouble(obj.TextString);
                                            alterSum += obj.TextString + "+";
                                        }
                                        catch (System.Exception) 
                                        {
                                            System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show("Значение в объекте содержит не только число.\nВыполнить считывание контента для ручного редактирования?", "Ошибка считывания", System.Windows.MessageBoxButton.YesNo);
                                            if (res == System.Windows.MessageBoxResult.Yes)
                                            {
                                                alterSum += obj.TextString + "+";
                                                alter = true;
                                            }
                                            else
                                            {
                                                return null;
                                            }
                                        }
                                        break;
                                    case "-":
                                        try
                                        {
                                            if (!alter) sum -= Convert.ToDouble(obj.TextString);
                                            alterSum += obj.TextString + "-";
                                        }
                                        catch (System.Exception)
                                        {
                                            System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show("Значение в объекте содержит не только число.\nВыполнить считывание контента для ручного редактирования?", "Ошибка считывания", System.Windows.MessageBoxButton.YesNo);
                                            if (res == System.Windows.MessageBoxResult.Yes)
                                            {
                                                alterSum += obj.TextString + "-";
                                                alter = true;
                                            }
                                            else
                                            {
                                                return null;
                                            }
                                        }
                                        break;
                                    case "*":
                                        try
                                        {
                                            if (!alter) sum *= Convert.ToDouble(obj.TextString);
                                            alterSum += obj.TextString + "*";
                                        }
                                        catch (System.Exception)
                                        {
                                            System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show("Значение в объекте содержит не только число.\nВыполнить считывание контента для ручного редактирования?", "Ошибка считывания", System.Windows.MessageBoxButton.YesNo);
                                            if (res == System.Windows.MessageBoxResult.Yes)
                                            {
                                                alterSum += obj.TextString + "*";
                                                alter = true;
                                            }
                                            else
                                            {
                                                return null;
                                            }
                                        }
                                        break;
                                    case "/":
                                        try
                                        {
                                            if (!alter) sum /= Convert.ToDouble(obj.TextString);
                                            alterSum += obj.TextString + "/";
                                        }
                                        catch (System.Exception)
                                        {
                                            System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show("Значение в объекте содержит не только число.\nВыполнить считывание контента для ручного редактирования?", "Ошибка считывания", System.Windows.MessageBoxButton.YesNo);
                                            if (res == System.Windows.MessageBoxResult.Yes)
                                            {
                                                alterSum += obj.TextString + "/";
                                                alter = true;
                                            }
                                            else
                                            {
                                                return null;
                                            }
                                        }
                                        break;
                                }
                                
                            }
                            if (classObj == "AcDbMText")
                            {
                                MText obj = tr.GetObject(enId, OpenMode.ForRead, false, true) as MText;
                                if (sum == 0.0)
                                {
                                    try
                                    {
                                        if (!alter) sum += Convert.ToDouble(obj.Text);
                                        alterSum += obj.Text + "+";
                                    }
                                    catch (System.Exception)
                                    {
                                        System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show("Значение в объекте содержит не только число.\nВыполнить считывание контента для ручного редактирования?", "Ошибка считывания", System.Windows.MessageBoxButton.YesNo);
                                        if (res == System.Windows.MessageBoxResult.Yes)
                                        {
                                            alterSum += obj.Text + "+";
                                            alter = true;
                                        }
                                        else
                                        {
                                            return null;
                                        }
                                    }
                                    continue;
                                }
                                switch (znac)
                                {
                                    case "+":
                                        try
                                        {
                                            if (!alter) sum += Convert.ToDouble(obj.Text);
                                            alterSum += obj.Text + "+";
                                        }
                                        catch (System.Exception)
                                        {
                                            System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show("Значение в объекте содержит не только число.\nВыполнить считывание контента для ручного редактирования?", "Ошибка считывания", System.Windows.MessageBoxButton.YesNo);
                                            if (res == System.Windows.MessageBoxResult.Yes)
                                            {
                                                alterSum += obj.Text + "+";
                                                alter = true;
                                            }
                                            else
                                            {
                                                return null;
                                            }
                                        }
                                        break;
                                    case "-":
                                        try
                                        {
                                            if (!alter) sum -= Convert.ToDouble(obj.Text);
                                            alterSum += obj.Text + "-";
                                        }
                                        catch (System.Exception)
                                        {
                                            System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show("Значение в объекте содержит не только число.\nВыполнить считывание контента для ручного редактирования?", "Ошибка считывания", System.Windows.MessageBoxButton.YesNo);
                                            if (res == System.Windows.MessageBoxResult.Yes)
                                            {
                                                alterSum += obj.Text + "-";
                                                alter = true;
                                            }
                                            else
                                            {
                                                return null;
                                            }
                                        }
                                        break;
                                    case "*":
                                        try
                                        {
                                            if (!alter) sum *= Convert.ToDouble(obj.Text);
                                            alterSum += obj.Text + "*";
                                        }
                                        catch (System.Exception)
                                        {
                                            System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show("Значение в объекте содержит не только число.\nВыполнить считывание контента для ручного редактирования?", "Ошибка считывания", System.Windows.MessageBoxButton.YesNo);
                                            if (res == System.Windows.MessageBoxResult.Yes)
                                            {
                                                alterSum += obj.Text + "*";
                                                alter = true;
                                            }
                                            else
                                            {
                                                return null;
                                            }
                                        }
                                        break;
                                    case "/":
                                        try
                                        {
                                            if (!alter) sum /= Convert.ToDouble(obj.Text);
                                            alterSum += obj.Text + "/";
                                        }
                                        catch (System.Exception)
                                        {
                                            System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show("Значение в объекте содержит не только число.\nВыполнить считывание контента для ручного редактирования?", "Ошибка считывания", System.Windows.MessageBoxButton.YesNo);
                                            if (res == System.Windows.MessageBoxResult.Yes)
                                            {
                                                alterSum += obj.Text + "/";
                                                alter = true;
                                            }
                                            else
                                            {
                                                return null;
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    return alterSum == "" ? sum.ToString() : alterSum;
                }
                else
                {
                    while (true)
                    {
                        PromptEntityResult result = ed.GetEntity("\nВыберите элемент: ");
                        if (result.Status == PromptStatus.Cancel) break;

                        ObjectId enId = result.ObjectId;

                        string classObj = enId.ObjectClass.Name;

                        if (!classObj.Equals("AcDbMLeader"))
                        {
                            ed.WriteMessage("\nВыбран объект: " + result.ObjectId.ObjectClass.Name);
                            continue;
                        }

                        using (Transaction tr = db.TransactionManager.StartTransaction())
                        {
                            if (classObj == "AcDbMLeader")
                            {
                                MLeader obj = tr.GetObject(enId, OpenMode.ForRead, false, true) as MLeader;
                                if (sum == 0.0)
                                {
                                    try
                                    {
                                        if (!alter) sum += Convert.ToDouble(obj.MText.Text);
                                        alterSum += obj.MText.Text + "+";
                                    }
                                    catch (System.Exception)
                                    {
                                        System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show("Значение в объекте содержит не только число.\nВыполнить считывание контента для ручного редактирования?", "Ошибка считывания", System.Windows.MessageBoxButton.YesNo);
                                        if (res == System.Windows.MessageBoxResult.Yes)
                                        {
                                            alterSum += obj.MText.Text + "+";
                                            alter = true;
                                        }
                                        else
                                        {
                                            return null;
                                        }
                                    }
                                    continue;
                                }
                                switch (znac)
                                {
                                    case "+":
                                        try
                                        {
                                            if (!alter) sum += Convert.ToDouble(obj.MText.Text);
                                            alterSum += obj.MText.Text + "+";
                                        }
                                        catch (System.Exception)
                                        {
                                            System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show("Значение в объекте содержит не только число.\nВыполнить считывание контента для ручного редактирования?", "Ошибка считывания", System.Windows.MessageBoxButton.YesNo);
                                            if (res == System.Windows.MessageBoxResult.Yes)
                                            {
                                                alterSum += obj.MText.Text + "+";
                                                alter = true;
                                            }
                                            else
                                            {
                                                return null;
                                            }
                                        }
                                        break;
                                    case "-":
                                        try
                                        {
                                            if (!alter) sum -= Convert.ToDouble(obj.MText.Text);
                                            alterSum += obj.MText.Text + "-";
                                        }
                                        catch (System.Exception)
                                        {
                                            System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show("Значение в объекте содержит не только число.\nВыполнить считывание контента для ручного редактирования?", "Ошибка считывания", System.Windows.MessageBoxButton.YesNo);
                                            if (res == System.Windows.MessageBoxResult.Yes)
                                            {
                                                alterSum += obj.MText.Text + "-";
                                                alter = true;
                                            }
                                            else
                                            {
                                                return null;
                                            }
                                        }
                                        break;
                                    case "*":
                                        try
                                        {
                                            if (!alter) sum *= Convert.ToDouble(obj.MText.Text);
                                            alterSum += obj.MText.Text + "*";
                                        }
                                        catch (System.Exception)
                                        {
                                            System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show("Значение в объекте содержит не только число.\nВыполнить считывание контента для ручного редактирования?", "Ошибка считывания", System.Windows.MessageBoxButton.YesNo);
                                            if (res == System.Windows.MessageBoxResult.Yes)
                                            {
                                                alterSum += obj.MText.Text + "*";
                                                alter = true;
                                            }
                                            else
                                            {
                                                return null;
                                            }
                                        }
                                        break;
                                    case "/":
                                        try
                                        {
                                            if (!alter) sum /= Convert.ToDouble(obj.MText.Text);
                                            alterSum += obj.MText.Text + "/";
                                        }
                                        catch (System.Exception)
                                        {
                                            System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show("Значение в объекте содержит не только число.\nВыполнить считывание контента для ручного редактирования?", "Ошибка считывания", System.Windows.MessageBoxButton.YesNo);
                                            if (res == System.Windows.MessageBoxResult.Yes)
                                            {
                                                alterSum += obj.MText.Text + "/";
                                                alter = true;
                                            }
                                            else
                                            {
                                                return null;
                                            }
                                        }
                                        break;
                                }
                                
                            }
                        }
                    }
                    return sum.ToString();
                }
            }
            else
            {
                if (onlyText)
                {
                    while (true)
                    {
                        PromptEntityResult result = ed.GetEntity("\nВыберите элемент: ");
                        if (result.Status == PromptStatus.Cancel) break;

                        ObjectId enId = result.ObjectId;

                        string classObj = enId.ObjectClass.Name;

                        if (!classObj.Equals("AcDbText") && !classObj.Equals("AcDbMText"))
                        {
                            ed.WriteMessage("\nВыбран объект: " + result.ObjectId.ObjectClass.Name);
                            continue;
                        }

                        using (Transaction tr = db.TransactionManager.StartTransaction())
                        {
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

                            break;

                        }
                    }
                    return null;
                }
                else
                {
                    while (true)
                    {
                        PromptEntityResult result = ed.GetEntity("\nВыберите элемент: ");
                        if (result.Status == PromptStatus.Cancel) break;

                        ObjectId enId = result.ObjectId;

                        string classObj = enId.ObjectClass.Name;

                        if (!classObj.Equals("AcDbMLeader"))
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

                            break;

                        }
                    }
                    return null;
                }
            }

           
        }

        /// <summary>
        /// Метод для получения площади фигуры
        /// </summary>
        /// <param name="znac"></param>
        /// <returns></returns>
        public static string GetPlosh(string znac)
        {
            Document adoc = Application.DocumentManager.MdiActiveDocument;
            Database db = adoc.Database;
            Editor ed = adoc.Editor;


            double sum = 0.0;
            if (znac != null)
            {
                while (true)
                {
                    PromptEntityResult result = ed.GetEntity("\nВыберите элемент: ");
                    if (result.Status == PromptStatus.Cancel) break;

                    ObjectId enId = result.ObjectId;

                    string classObj = enId.ObjectClass.Name;

                    if (!classObj.Equals("AcDbCircle") && !classObj.Equals("AcDbEllipse") && !classObj.Equals("AcDbPolyline"))
                    {
                        ed.WriteMessage("\nВыбран объект: " + result.ObjectId.ObjectClass.Name);
                        continue;
                    }
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        var obj = tr.GetObject(enId, OpenMode.ForRead, false, true);

                        switch (result.ObjectId.ObjectClass.Name)
                        {
                            case "AcDbCircle":
                                Circle circl = obj as Circle;
                                switch (znac)
                                {
                                    case "+":
                                        sum += circl.Radius * circl.Radius * Math.PI;
                                        break;
                                    case "-":
                                        sum -= circl.Radius * circl.Radius * Math.PI;
                                        break;
                                    case "*":
                                        sum *= circl.Radius * circl.Radius * Math.PI;
                                        break;
                                    case "/":
                                        sum /= circl.Radius * circl.Radius * Math.PI;
                                        break;
                                }
                                break;

                            case "AcDbEllipse":
                                Ellipse ellips = obj as Ellipse;
                                switch (znac)
                                {
                                    case "+":
                                        sum += ellips.Area;
                                        break;
                                    case "-":
                                        sum -= ellips.Area;
                                        break;
                                    case "*":
                                        sum *= ellips.Area;
                                        break;
                                    case "/":
                                        sum /= ellips.Area;
                                        break;
                                }
                                break;
                            case "AcDbPolyline":
                                Polyline polyLine = obj as Polyline;
                                switch (znac)
                                {
                                    case "+":
                                        sum += polyLine.Area;
                                        break;
                                    case "-":
                                        sum -= polyLine.Area;
                                        break;
                                    case "*":
                                        sum *= polyLine.Area;
                                        break;
                                    case "/":
                                        sum /= polyLine.Area;
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
            else
            {
                while (true)
                {
                    PromptEntityResult result = ed.GetEntity("\nВыберите элемент: ");
                    if (result.Status == PromptStatus.Cancel) break;

                    ObjectId enId = result.ObjectId;

                    string classObj = enId.ObjectClass.Name;

                    if (!classObj.Equals("AcDbCircle") && !classObj.Equals("AcDbEllipse") && !classObj.Equals("AcDbPolyline"))
                    {
                        ed.WriteMessage("\nВыбран объект: " + result.ObjectId.ObjectClass.Name);
                        continue;
                    }
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        var obj = tr.GetObject(enId, OpenMode.ForRead, false, true);

                        switch (result.ObjectId.ObjectClass.Name)
                        {
                            case "AcDbCircle":
                                Circle circl = obj as Circle;
                                return (circl.Radius * circl.Radius * Math.PI).ToString();
                                break;
                            case "AcDbEllipse":
                                Ellipse ellips = obj as Ellipse;
                                return ellips.Area.ToString();
                                break;
                            case "AcDbPolyline":
                                Polyline polyLine = obj as Polyline;
                                return polyLine.Area.ToString();
                                break;
                        }
                    }
                }

            }


            return sum.ToString();
        }

        #endregion ////////////////


        #region Insert ///////////////

        /// <summary>
        /// Метод для вставки значения в ячейку таблицы.
        /// </summary>
        /// <param name="insertText"> Текст для вставки в таблицу </param>
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
        /// Метод для вставки однострочного текста.
        /// </summary>
        /// <param name="value">Вставляемый текст</param>
        public static void InsertOneText(string value)
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            // Запрос на выбор точки вставки
            PromptPointOptions insPointPrompt = new PromptPointOptions("\nВыберите точку вставки: ");
            PromptPointResult insPointResult = acDoc.Editor.GetPoint(insPointPrompt);
            if (insPointResult.Status != PromptStatus.OK) return;

            // Создание объекта однострочного текста
            DBText dbText = new DBText();
            dbText.TextString = value;
            dbText.Height = 6.0;

            // Задание точки вставки и добавление текста в пространство модели
            dbText.Position = insPointResult.Value;
            BlockTableRecord acBlkTblRec;
            using (DocumentLock docLock = acDoc.LockDocument())
            {
                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    acBlkTblRec = acTrans.GetObject(acCurDb.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;
                    acBlkTblRec.AppendEntity(dbText);
                    acTrans.AddNewlyCreatedDBObject(dbText, true);
                    acTrans.Commit();
                }
            }


            // Обновляем экран
            acDoc.Editor.Regen();
            acDoc.Editor.WriteMessage("\nТекст успешно вставлен");
        }


        /// <summary>
        /// Метод для замены текста в выбранном однострочном тексте.
        /// </summary>
        /// <param name="value">Текст для замены</param>
        public static void ReplaceOneText(string value)
        {
            Document adoc = Application.DocumentManager.MdiActiveDocument;
            Database db = adoc.Database;
            Editor ed = adoc.Editor;
            bool enter = false;
            while (!enter)
            {
                PromptEntityResult result = ed.GetEntity("\nВыберите текст: ");
                if (result.Status == PromptStatus.Cancel) ed.WriteMessage("\nФункция была отменена!\n ");

                ObjectId enId = result.ObjectId;

                string classObj = enId.ObjectClass.Name;

                if (!classObj.Equals("AcDbText"))
                {
                    ed.WriteMessage("\nВыбран объект: " + result.ObjectId.ObjectClass.Name);
                    continue;
                }
                using (DocumentLock docLock = adoc.LockDocument())
                {
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        DBText obj = tr.GetObject(enId, OpenMode.ForRead, false, true) as DBText;
                        obj.UpgradeOpen();
                        obj.TextString = value;


                        enter = true;
                        tr.Commit();
                    }
                }
            }
            ed.WriteMessage("\nТекст был заменен! \n ");
        }

        /// <summary>
        /// Метод для добавления строки к тексту
        /// </summary>
        /// <param name="value"></param>
        public static void AddToText(string value)
        {
            Document adoc = Application.DocumentManager.MdiActiveDocument;
            Database db = adoc.Database;
            Editor ed = adoc.Editor;
            bool enter = false;
            while (!enter)
            {
                PromptEntityResult result = ed.GetEntity("\nВыберите текст: ");
                if (result.Status == PromptStatus.Cancel) ed.WriteMessage("\nФункция была отменена!\n ");

                ObjectId enId = result.ObjectId;

                string classObj = enId.ObjectClass.Name;

                if (!classObj.Equals("AcDbText"))
                {
                    ed.WriteMessage("\nВыбран объект: " + result.ObjectId.ObjectClass.Name);
                    continue;
                }
                using (DocumentLock docLock = adoc.LockDocument())
                {
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        DBText obj = tr.GetObject(enId, OpenMode.ForRead, false, true) as DBText;
                        obj.UpgradeOpen();
                        obj.TextString += " "+ value;


                        enter = true;
                        tr.Commit();
                    }
                }
            }
            ed.WriteMessage("\nТекст был заменен! \n ");
        }


        /// <summary>
        /// Метод для добавления к многострочному тексту.
        /// </summary>
        /// <param name="value">Вставляемый текст</param>
        public static void InsertPolyText(string value)
        {
            Document adoc = Application.DocumentManager.MdiActiveDocument;
            Editor acEditor = adoc.Editor;
            Database db = adoc.Database;

            // Запрос на выбор многострочного текста на чертеже
            PromptEntityOptions promptEntity = new PromptEntityOptions("\nВыберите многострочный текст на чертеже: ");
            promptEntity.SetRejectMessage("Выбранный элемент не является многострочным текстом.");
            promptEntity.AddAllowedClass(typeof(MText), true);
            PromptEntityResult entityResult = acEditor.GetEntity(promptEntity);
            if (entityResult.Status != PromptStatus.OK) return;

            // Получение объекта многострочного текста и его содержимого
            using (DocumentLock docLock = adoc.LockDocument())
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    MText obj = tr.GetObject(entityResult.ObjectId, OpenMode.ForRead, false, true) as MText;
                    string text = obj.Contents;

                    // Дополнение текста
                    obj.UpgradeOpen();
                    obj.Contents = text + "\n" + value;

                    // Обновление экрана
                    acEditor.Regen();

                    acEditor.WriteMessage("\nТекст успешно дополнен");

                    tr.Commit();
                }
            }
        }

        #endregion //////////////


        /// <summary>
        /// Метод для всатвки выноски с текстом
        /// </summary>
        public static void LeaderInsert(string value)
        {
            
            

            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acDb = acDoc.Database;
            Editor acEd = acDoc.Editor;

            // Запрос выбора первой точки
            PromptPointOptions opt1 = new PromptPointOptions("Укажите первую точку выноски: ");
            PromptPointResult res1 = acEd.GetPoint(opt1);
            if (res1.Status != PromptStatus.OK)
                return;

            // Запрос выбора второй точки
            PromptPointOptions opt2 = new PromptPointOptions("Укажите вторую точку выноски: ");
            opt2.BasePoint = res1.Value;
            opt2.UseBasePoint = true;
            PromptPointResult res2 = acEd.GetCorner(opt2);
            if (res2.Status != PromptStatus.OK)
                return;

            // Создание объекта MLeader
            MLeader mLeader = new MLeader();
            MText mText = new MText();
            mText.Contents = value;

            mLeader.ContentType = ContentType.MTextContent;
            mLeader.MText = mText;

            // Добавление линии лидера

            mLeader.AddLeaderLine(res1.Value);

            // Добавление точек выноски

            mLeader.SetFirstVertex(0, res1.Value);
            mLeader.SetLastVertex(0, res2.Value);



            // Добавление MLeader в чертеж
            using (DocumentLock docLock = acDoc.LockDocument())
            {
                Transaction tr = acDoc.TransactionManager.StartTransaction();
                using (tr)
                {
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(acDb.CurrentSpaceId, OpenMode.ForWrite);
                    ObjectId id = btr.AppendEntity(mLeader);
                    tr.AddNewlyCreatedDBObject(mLeader, true);
                    tr.Commit();
                }
            }
        }



        /// <summary>
        /// Класс для создания кнопки вызова модального окна в панели управления.
        /// </summary>
        public class AddPanel : IExtensionApplication
        {

            // создание новой панели
            RibbonPanelSource ribbonPanelSource = new RibbonPanelSource();
            RibbonPanel ribbonPanel = new RibbonPanel();

            // создание новой кнопки
            RibbonButton ribbonButton = new RibbonButton();

            /// <summary>
            /// Метод создающий панель при инициализации (запуске) библиотеки.
            /// </summary>
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


            /// <summary>
            /// Метод для отрисовки модального окна.
            /// </summary>
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


        /// <summary>
        /// Вспомогательный класс для обработки нажатия на кнопку в панели.
        /// </summary>
        public class ButtonClickCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;


            /// <summary>
            /// Методя для задания действия при нажатии на кнопку
            /// </summary>
            /// <param name="parameter">Комманда которую необходимо выполнить при нажатии</param>
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
