using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketDesignTool.DefaultClass
{
    public class ConstList<T>
    {
        List<T> values = new List<T>();

        public ConstList(params T[] objParam) 
        {
            for (int i = 0; i < objParam.Length; i++)
            {
                values.Add(objParam[i]);
            }
        }

        public int Length { get { return values.Count; } }

        public T this[int index]
        {
            get => values[index];       // 읽기
            set => values[index] = value; // 쓰기 (필요 없으면 제거 가능)
        }
    }
}
