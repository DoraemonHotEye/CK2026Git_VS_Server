using PacketDesignTool.Managers.Parents;
using PacketDesignTool.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketDesignTool.Managers.Supports
{
    public enum ObjectType
    {  
        BASIC   = 0, //
        CLASS   = 1,
        STRUCT  = 2
    }

    public class ToolStaticLibrary
    {
        //Convert Array Functions
        public static T[] ConvertArrayHeap<T>(params T[] param) where T : class
        {
            return param;
        }
        public static T[] ConvertArrayStack<T>(params T[] param) where T : struct
        {
            return param;
        }
        public static string[] ConvertArrayStack<T>(params string[] param)
        {
            return param;
        }

        //Convert List Functions
        public static List<T> ConverToListHeap<T>(params T[] param) where T : class
        {
            List<T> list = new List<T>();
            for (int i = 0; i < param.Length; ++i)
                list.Add(param[i]);

            return list;
        }
        public static List<T> ConverToListStack<T>(params T[] param) where T : struct
        {
            List<T> list = new List<T>();
            for (int i = 0; i < param.Length; ++i)
                list.Add(param[i]);

            return list;
        }
        public static List<string> ConverToListString(params string[] param)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < param.Length; ++i)
                list.Add(param[i]);

            return list;
        }

        // GetList Functions
        public static List<T> GetListToHeap<T>(T[] array) where T : class
        {
            List<T> list = new List<T>();
            for (int i = 0; i < array.Length; ++i)
                list.Add(array[i]);

            return list;
        }
        public static List<T> GetListToStack<T>(T[] array) where T : struct
        {
            List<T> list = new List<T>();
            for (int i = 0; i < array.Length; ++i)
                list.Add(array[i]);

            return list;
        }
        public static List<string> GetListToString(string[] array)
        {
            List<string> list = new List<string>();

            for (int i = 0; i < array.Length; ++i)
                list.Add(array[i]);

            return list;
        }
    
        public static string GetHashSCCode(string Origin, ProjectSettingProfiler profiler)
        {
            string SC_CODE = "[None]";

            if (profiler != null)
            {
                SC_CODE = profiler.Name.ToUpper().ToString();
            }

            return SC_CODE;
        }

        public static ObjectType GetTypeObject(Packer packer)
        {
            ObjectType type = ObjectType.BASIC;

            if (ClassPackerManager.Inst.PackerList.ContainsValue(packer))
                type = ObjectType.CLASS;
            else if (StructPackerManager.Inst.PackerList.ContainsValue(packer))
                type = ObjectType.STRUCT;

            return type;
        }

        public static Packer GetSliceOfClassPacker(string memberType)
        {
            string[] splitstring = memberType.Split('=');
            string[] memberslice = splitstring[0].Split(':');

            if (ClassPackerManager.Inst.PackerList.TryGetValue(memberslice[1], out Packer packer))
                return packer;

            return null;
        }

        public static Packer GetSliceOfStruct(string memberType)
        {
            string[] splitstring = memberType.Split('=');
            string[] memberslice = splitstring[0].Split(':');

            if (StructPackerManager.Inst.PackerList.TryGetValue(memberslice[1], out Packer packer))
                return packer;

            return null;
        }
    }
}
