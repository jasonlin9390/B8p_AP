#define TEST

using System.Collections;

namespace PP791
{


    // interface IEnumerable, ICollection and  IList


    //using
    // 用來匯入在其他命名空間中定義的型別 

    //using System.ComponentModel;

    //namespace 關鍵字的用途在於宣告範圍。這個命名空間範圍讓您組織程式碼並且提供建立全域唯一型別的方法。


    public class messages : CollectionBase
    {
        public void Add(message input)
        {
            List.Add(input);
        }

        public void Remove(int index)
        {
            List.RemoveAt(index);
            //List.Count --; 
        }

        public int Total()
        {
            return List.Count;
        }

        public int FindIndex(message input)
        {
            return List.IndexOf(input);
        }

        public messages()
        {
        }

        // indexer common array usage as follow  

        public message this[int messageIndex]
        {
            get
            {
                return (message)List[messageIndex];
            }
            set
            {
                List[messageIndex] = value;
            }
        }
    }
}



