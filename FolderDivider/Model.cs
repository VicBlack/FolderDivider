using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FolderDivider
{
    class Model : INotifyPropertyChanged
    {
        #region 成员
        public string InputPath { get { return _InputPath; } set { if (_InputPath == value) return; _InputPath = value; _TotalNums = GetFilesCount(InputPath); OnPropertyChanged(nameof(TotalNums)); OnPropertyChanged(nameof(InputPath)); } }
        private string _InputPath;

        public string OutPath { get { return _OutPath; } set { if (_OutPath == value) return; _OutPath = value; OnPropertyChanged(nameof(OutPath)); } }
        private string _OutPath;

        public int TotalNums { get { return _TotalNums; } set { if (_TotalNums == value) return; _TotalNums = value; OnPropertyChanged(nameof(TotalNums)); } }
        private int _TotalNums;

        public int Progress { get { return _Progress; } set { if (_Progress == value) return; _Progress = value; OnPropertyChanged(nameof(Progress)); } }
        private int _Progress;

        public float Percent { get { return _Percent; } set { if (_Percent == value) return; _Percent = Math.Max(Math.Min(value, 50), 0); _Nums = (int)(TotalNums * Percent /100); OnPropertyChanged(nameof(Percent)); OnPropertyChanged(nameof(Nums)); } }
        private float _Percent;

        public int Nums { get { return _Nums; } set { if (_Nums == value) return; _Nums = Math.Max(Math.Min(value,TotalNums/2),0); _Percent = (float)Nums * 100 / Math.Max(TotalNums, 1); OnPropertyChanged(nameof(Percent)); OnPropertyChanged(nameof(Nums)); } }
        private int _Nums;

        public List<string> filelist;
        #endregion

        #region 方法
        public Model()
        {
            
        }

        private int GetFilesCount(string path)
        {
            int count = 0;
            count += Directory.GetFiles(path).Length;
            foreach (var folder in Directory.GetDirectories(path))
            {
                count += GetFilesCount(folder);
            }
            return count;
        }

        public void GetFilesName(string path)
        {
            string[] templist;
            templist = Directory.GetFiles(path);
            foreach (string filename in templist)
            {
                filelist.Add(filename);
            }
            foreach (var folder in Directory.GetDirectories(path))
            {
                GetFilesName(folder);
            }
        }

        public  List<T> GetRandomList<T>(List<T> inputList)
        {
            //Copy to a array
            T[] copyArray = new T[inputList.Count];
            inputList.CopyTo(copyArray);

            //Add range
            List<T> copyList = new List<T>();
            copyList.AddRange(copyArray);

            //Set outputList and random
            List<T> outputList = new List<T>();
            Random rd = new Random(DateTime.Now.Millisecond);

            while (copyList.Count > 0)
            {
                //Select an index and item
                int rdIndex = rd.Next(0, copyList.Count - 1);
                T remove = copyList[rdIndex];

                //remove it from copyList and add it to output
                copyList.Remove(remove);
                outputList.Add(remove);
            }
            return outputList;
        }
        #endregion

        #region INotifyPropertyChanged接口便利实现
        private void OnPropertyChanged(string aPropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
