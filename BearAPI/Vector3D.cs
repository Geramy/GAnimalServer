using System;
using System.Collections.Generic;
using System.Text;
namespace BearAPI
{
    public class Vector3D
    {
        
        public string Vector = "";
        public long X;
        public long Y;
        public long Z;

        public Vector3D(string vec)
        {
            
            Vector = vec;
            string temp = vec.Substring(vec.IndexOf("<") + 1, vec.IndexOf(">") - 1);
            string[] vectorssplit = temp.Split(',');
            X = (IntPtr.Size == 8 ? Convert.ToInt64(vectorssplit[0].Substring(0, (vectorssplit[0].IndexOf(".") != -1 ? vectorssplit[0].IndexOf(".") - 1 : vectorssplit[0].Length))) : Convert.ToInt32(vectorssplit[0].Substring(0, (vectorssplit[0].IndexOf(".") != -1 ? vectorssplit[0].IndexOf(".") - 1 : vectorssplit[0].Length))));
            Y = (IntPtr.Size == 8 ? Convert.ToInt64(vectorssplit[1].Substring(0, (vectorssplit[1].IndexOf(".") != -1 ? vectorssplit[1].IndexOf(".") - 1 : vectorssplit[1].Length))) : Convert.ToInt32(vectorssplit[1].Substring(0, (vectorssplit[1].IndexOf(".") != -1 ? vectorssplit[1].IndexOf(".") - 1 : vectorssplit[1].Length))));
            Z = (IntPtr.Size == 8 ? Convert.ToInt64(vectorssplit[2].Substring(0, (vectorssplit[2].IndexOf(".") != -1 ? vectorssplit[2].IndexOf(".") - 1 : vectorssplit[2].Length))) : Convert.ToInt32(vectorssplit[2].Substring(0, (vectorssplit[2].IndexOf(".") != -1 ? vectorssplit[2].IndexOf(".") - 1 : vectorssplit[2].Length))));
        }
        public Vector3D(Vector3D vec)
        {
            X = vec.X;
            Y = vec.Y;
            Z = vec.Z;
        }
        public Vector3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
