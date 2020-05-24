using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Architecture;
using System.Collections;
using System.Data;
using System.IO;

namespace KongJianJuFa
{
    [Transaction(TransactionMode.Manual)]
    public class Class1 : IExternalCommand
    {
        string Assembly = typeof(Class1).Assembly.Location;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //throw new NotImplementedException();
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Dictionary<string , List<Element>> roomBoundaryDict =
                new Dictionary<string, List<Element>>();

            List<string> roomNameList = new List<string>();

            //FamilyInstance fsi = null;
            //fsi.Host;
            FilteredElementCollector roomCollector =
                new FilteredElementCollector(doc);
            roomCollector
                .OfCategory(BuiltInCategory.OST_Rooms)
                .WhereElementIsNotElementType();
            string set = null;
                
            foreach (Element ele in roomCollector)
            {
                roomNameList.Add((ele as Room).get_Parameter(BuiltInParameter.ROOM_NAME).AsString());
                set= (ele as Room).get_Parameter(BuiltInParameter.ROOM_NAME).AsString();
                roomBoundaryDict.Add(set, new List<Element>());
            }
            //获取所有的房间
            //TaskDialog.Show("roomDict", roomBoundaryDict.Count.ToString());

            //先进行合理排序
            roomNameList.Sort();

            FilteredElementCollector doorCollector =
                new FilteredElementCollector(doc);
            doorCollector.OfCategory(BuiltInCategory.OST_Doors)
                .WhereElementIsNotElementType();
            //TaskDialog.Show("门数量", doorCollector.Count().ToString());
            //IList<Element> doorList = doorCollector.ToElements();
            //List<Element> doorWithWallList = new List<Element>();
            FamilyInstance fsi = null;
            string s = null;

            //设置不指定的长度
            //int[,] A = new int[roomCollector.Count(), roomCollector.Count()];
            ArrayList arr = new ArrayList();

            string row = null;
            string column = null;
            List<int> list = new List<int>();
            //list.Add(3);
            //list.Add(9);
            //int[] arr = list.ToArray();

            int matrix = roomCollector.Count() * roomCollector.Count();
            for (int i = 0; i == matrix; i++)
            {
                list.Add(0);
            }
            int n = 0;
            n = roomCollector.Count();
            ArrayList AList = new ArrayList();
            for (int i = 0; i < (n*n); i++) //给数组增加10个Int元素
                AList.Add(0);

            s = null;
            foreach (Element ele in doorCollector)
            {
                fsi = ele as FamilyInstance;
                //doorWithWallList.Add(fsi.Host);
                s = s + "门是从";
                s = s + fsi.FromRoom.get_Parameter(BuiltInParameter.ROOM_NAME).AsString() + " ";
                s = s + "门是到";
                s = s + fsi.ToRoom.get_Parameter(BuiltInParameter.ROOM_NAME).AsString() + "\n";
                //roomNameList.FindIndex();
                row = fsi.FromRoom.get_Parameter(BuiltInParameter.ROOM_NAME).AsString();
                column = fsi.ToRoom.get_Parameter(BuiltInParameter.ROOM_NAME).AsString();
                if (row!=null&&column!=null) {
                    //行数
                    int jinruNumber = roomNameList.FindIndex(a => a == row);

                    //列数
                    int shuchuNumber = roomNameList.FindIndex(a => a == column);
                    //A[jinruNumber, shuchuNumber] = 0;
                    //TaskDialog.Show("ROW",jinruNumber.ToString());
                    //TaskDialog.Show("COLUMN",shuchuNumber.ToString());
                    AList[jinruNumber * (n) + shuchuNumber] = 1;
                    //AList[shuchuNumber * (n) + shuchuNumber] = 1;
                }
                
                //arr.Add()


            }

            //object[] values = AList.ToArray(typeof(object));
            //object[n, n] values= AList.ToArray(typeof(object));
            //TaskDialog.Show("demo", AList.ToArray(typeof(object)).Length.ToString());
            //TaskDialog.Show("demo1",AList.ToArray().Rank.ToString());
            //string[,n] arrString = (string[n,n])AList.ToArray(typeof(string));

            

            //for(int j=0;j<AList.Count;j++) {
            //    TaskDialog.Show("1",AList[j].ToString());
            //}

            //var dt = new DataTable();
            ////DataColumn dc = new DataColumn("no");
            ////dt.Columns.Add(dc);

            //foreach (String s1 in roomNameList) {
            //    DataColumn dc = new DataColumn(s1);
            //    dt.Columns.Add(dc);
            //}

            //DataRow row;
            //for (int i = 0; i < list.Count; i++)
            //{
            //    //进行跳跃
            //    if (i%roomCollector.Count()==0) {

            //    }
            //    //row = dt.NewRow();
            //    //row["no"] = list[i];
            //    //dt.Rows.Add(row);
            //}


            //TaskDialog.Show("房间关系", s);
            //s = null;

            //for (int i=0;i<list.Count;i++) {
            //    s =s+ list[i].ToString()+" ";
            //}


            //Matrix matrix = new Matrix(roomCollector.Count(), roomCollector.Count());
            //matrix[0][0] = 1;
            //foreach (int item in A) {
            //    s = item + " ";
            //}
            //TaskDialog.Show("关系", s);



            Room room = null;
            //获取所有边界
            foreach (Element ele in roomCollector)
            {
                room = ele as Room;
                IList<IList<BoundarySegment>> roomBoundaryListList = room
                .GetBoundarySegments(new SpatialElementBoundaryOptions());
                foreach (IList<BoundarySegment> roomBoundaryList in roomBoundaryListList)
                {
                    foreach (BoundarySegment boundarySegment in roomBoundaryList)
                    {
                        Element tmp = doc.GetElement(boundarySegment.ElementId);
                        List<Element> tmpList = new List<Element>();
                        //如果包含的话

                        //如果门所有对应的墙包含这个元素，那么就把这个房间标记为有门的房间
                        Wall wall = doc.GetElement(boundarySegment.ElementId) as Wall;
                        ModelLine ml = doc.GetElement(boundarySegment.ElementId) as ModelLine;
                        ////TaskDialog.Show("1", "9");
                        if (wall != null)
                        {

                        }
                        else {
                            if (ml != null)
                            {
                                //原来是这种嘛
                                //TaskDialog.Show("room");
                                roomBoundaryDict[room.get_Parameter(BuiltInParameter.ROOM_NAME).AsString()].Add(ml);
                                //似乎绘制墙体的同时也会绘制模型线

                                //TaskDialog.Show("roomName", room.get_Parameter(BuiltInParameter.ROOM_NAME).AsString());
                                //TaskDialog.Show("dict", roomBoundaryDict.Count.ToString());

                                //TaskDialog.Show("1", roomBoundaryDict[room.get_Parameter(BuiltInParameter.ROOM_NAME).AsString()].Add(ml).Count.Tostring());
                                //roomBoundaryDict[];
                                //    //wallOrSpList.Add(wall.Id);
                            }
                        }
                        

                    }
                }
            }

            //string s = null;

            string name1 = null;
            string name2 = null;
            foreach (KeyValuePair<string, List<Element>> kvp in roomBoundaryDict)
            {
                

                //s = s + kvp.Value.Count.ToString() + "\n";
                //Console.WriteLine("姓名：{0},电影：{1}", kvp.Key, kvp.Value);
                foreach (KeyValuePair<string, List<Element>> kvp1 in roomBoundaryDict)
                {
                    //name1 = kvp.Key.get_Parameter(BuiltInParameter.ROOM_NAME).AsString();
                    //name2 = kvp1.Key.get_Parameter(BuiltInParameter.ROOM_NAME).AsString();
                    name1 = kvp.Key;
                    name2 = kvp1.Key;
                    //s = s + kvp.Value.Count.ToString() + "\n";
                    if (kvp.Value.Union(kvp1.Value).Count()!=0&&(name1!=name2)) {
                        int jinruNumber = roomNameList.FindIndex(a => a == name1);

                        //列数
                        int shuchuNumber = roomNameList.FindIndex(a => a == name2);
                        //TaskDialog.Show("DEMO1",jinruNumber.ToString());
                        //TaskDialog.Show("DEMO2",shuchuNumber.ToString());

                        //A[jinruNumber, shuchuNumber] = 0;
                        //TaskDialog.Show("ROW",jinruNumber.ToString());
                        //TaskDialog.Show("COLUMN",shuchuNumber.ToString());
                        //AList[jinruNumber * (n) + shuchuNumber] = 1;
                    }
                    //Console.WriteLine("姓名：{0},电影：{1}", kvp.Key, kvp.Value);
                }

            }



            //TaskDialog.Show("房间与数量", s);

            string url = Assembly.Replace("KongJianJuFa.dll", "空间句法.txt");
            Txt_Writer(AList, url, n);

            return Result.Succeeded;
        }

        //arrayList写入txt
        public static void Txt_Writer(ArrayList cList, string savePath,int n)
        {
            FileStream fs = new FileStream(savePath, FileMode.Create);
            int i = 1;
            foreach (var item in cList)
            {
                //如果是整除，那么就写个\n
                if (i % n == 0)
                {
                    byte[] data = new UTF8Encoding().GetBytes(item + "\r\n");
                    fs.Write(data, 0, data.Length);
                }
                else {
                    byte[] data = new UTF8Encoding().GetBytes(item + "\r");
                    fs.Write(data, 0, data.Length);
                }
                
                i = i + 1;
            }
            fs.Flush(); fs.Close();
        }

        //矩阵打包成类，矩阵为m * n,直接调用
        public class Matrix
        {
            double[,] A;
            int m, n;
            string name;
            public Matrix(int am, int an)
            {
                m = am;
                n = an;
                A = new double[m, n];
                name = "Result";
            }
            public Matrix(int am, int an, string aName)
            {
                m = am;
                n = an;
                A = new double[m, n];
                name = aName;
            }

            public int getM
            {
                get { return m; }
            }
            public int getN
            {
                get { return n; }
            }
            public double[,] Detail
            {
                get { return A; }
                set { A = value; }
            }
            public string Name
            {
                get { return name; }
                set { name = value; }
            }
        }

        /***********矩阵通用操作打包*************/

        class MatrixOperator
        {
            //矩阵加法
            public static Matrix MatrixAdd(Matrix Ma, Matrix Mb)
            {
                int m = Ma.getM;
                int n = Ma.getN;
                int m2 = Mb.getM;
                int n2 = Mb.getN;

                if ((m != m2) || (n != n2))
                {
                    Exception myException = new Exception("数组维数不匹配");
                    throw myException;
                }

                Matrix Mc = new Matrix(m, n);
                double[,] c = Mc.Detail;
                double[,] a = Ma.Detail;
                double[,] b = Mb.Detail;

                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n; j++)
                        c[i, j] = a[i, j] + b[i, j];
                return Mc;
            }

            //矩阵减法
            public static Matrix MatrixSub(Matrix Ma, Matrix Mb)
            {
                int m = Ma.getM;
                int n = Ma.getN;
                int m2 = Mb.getM;
                int n2 = Mb.getN;
                if ((m != m2) || (n != n2))
                {
                    Exception myException = new Exception("数组维数不匹配");
                    throw myException;
                }
                Matrix Mc = new Matrix(m, n);
                double[,] c = Mc.Detail;
                double[,] a = Ma.Detail;
                double[,] b = Mb.Detail;

                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n; j++)
                        c[i, j] = a[i, j] - b[i, j];
                return Mc;
            }

            //矩阵乘法
            public static Matrix MatrixMulti(Matrix Ma, Matrix Mb)
            {
                int m = Ma.getM;
                int n = Ma.getN;
                int m2 = Mb.getM;
                int n2 = Mb.getN;

                if (n != m2)
                {
                    Exception myException = new Exception("数组维数不匹配");
                    throw myException;
                }

                Matrix Mc = new Matrix(m, n2);
                double[,] c = Mc.Detail;
                double[,] a = Ma.Detail;
                double[,] b = Mb.Detail;

                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n2; j++)
                    {
                        c[i, j] = 0;
                        for (int k = 0; k < n; k++)
                            c[i, j] += a[i, k] * b[k, j];
                    }
                return Mc;

            }

            //矩阵数乘
            public static Matrix MatrixSimpleMulti(double k, Matrix Ma)
            {
                int m = Ma.getM;
                int n = Ma.getN;
                Matrix Mc = new Matrix(m, n);
                double[,] c = Mc.Detail;
                double[,] a = Ma.Detail;

                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n; j++)
                        c[i, j] = a[i, j] * k;
                return Mc;
            }

            //矩阵转置
            public static Matrix MatrixTrans(Matrix MatrixOrigin)
            {
                int m = MatrixOrigin.getM;
                int n = MatrixOrigin.getN;
                Matrix MatrixNew = new Matrix(n, m);
                double[,] c = MatrixNew.Detail;
                double[,] a = MatrixOrigin.Detail;
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < m; j++)
                        c[i, j] = a[j, i];
                return MatrixNew;
            }

            //矩阵求逆（伴随矩阵法）
            public static Matrix MatrixInvByCom(Matrix Ma)
            {
                double d = MatrixOperator.MatrixDet(Ma);
                if (d == 0)
                {
                    Exception myException = new Exception("没有逆矩阵");
                    throw myException;
                }
                Matrix Ax = MatrixOperator.MatrixCom(Ma);
                Matrix An = MatrixOperator.MatrixSimpleMulti((1.0 / d), Ax);
                return An;
            }
            //对应行列式的代数余子式矩阵
            public static Matrix MatrixSpa(Matrix Ma, int ai, int aj)
            {
                int m = Ma.getM;
                int n = Ma.getN;
                if (m != n)
                {
                    Exception myException = new Exception("矩阵不是方阵");
                    throw myException;
                }
                int n2 = n - 1;
                Matrix Mc = new Matrix(n2, n2);
                double[,] a = Ma.Detail;
                double[,] b = Mc.Detail;

                //左上
                for (int i = 0; i < ai; i++)
                    for (int j = 0; j < aj; j++)
                    {
                        b[i, j] = a[i, j];
                    }
                //右下
                for (int i = ai; i < n2; i++)
                    for (int j = aj; j < n2; j++)
                    {
                        b[i, j] = a[i + 1, j + 1];
                    }
                //右上
                for (int i = 0; i < ai; i++)
                    for (int j = aj; j < n2; j++)
                    {
                        b[i, j] = a[i, j + 1];
                    }
                //左下
                for (int i = ai; i < n2; i++)
                    for (int j = 0; j < aj; j++)
                    {
                        b[i, j] = a[i + 1, j];
                    }
                //符号位
                if ((ai + aj) % 2 != 0)
                {
                    for (int i = 0; i < n2; i++)
                        b[i, 0] = -b[i, 0];

                }
                return Mc;

            }

            //矩阵的行列式,矩阵必须是方阵
            public static double MatrixDet(Matrix Ma)
            {
                int m = Ma.getM;
                int n = Ma.getN;
                if (m != n)
                {
                    Exception myException = new Exception("数组维数不匹配");
                    throw myException;
                }
                double[,] a = Ma.Detail;
                if (n == 1) return a[0, 0];

                double D = 0;
                for (int i = 0; i < n; i++)
                {
                    D += a[1, i] * MatrixDet(MatrixSpa(Ma, 1, i));
                }
                return D;
            }

            //矩阵的伴随矩阵
            public static Matrix MatrixCom(Matrix Ma)
            {
                int m = Ma.getM;
                int n = Ma.getN;
                Matrix Mc = new Matrix(m, n);
                double[,] c = Mc.Detail;
                double[,] a = Ma.Detail;

                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n; j++)
                        c[i, j] = MatrixDet(MatrixSpa(Ma, j, i));

                return Mc;
            }

        }
    }
}
