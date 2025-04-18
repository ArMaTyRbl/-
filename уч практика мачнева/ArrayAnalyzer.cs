using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace уч_практика_мачнева
{
    public class ArrayAnalyzer
    {
        public int[] Data { get; private set; }
        public List<int> DuplicateIndexes { get; private set; }

        public ArrayAnalyzer(int[] data)
        {
            Data = data;
            DuplicateIndexes = new List<int>();

            for (int i = 1; i < data.Length; i++)
            {
                if (data[i] == data[i - 1])
                {
                    DuplicateIndexes.Add(i); // сохраняем индекс повторяющегося элемента
                }
            }
        }

        public int CountDuplicates()
        {
            return DuplicateIndexes.Count;
        }
    }

}
