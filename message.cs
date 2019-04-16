
namespace PP791
{

    // abstract 修飾詞可用於類別、方法、屬性、索引子 (Indexer) 和事件。
    // 在類別宣告裡使用 abstract 修飾詞，表示該類別只是當做其他類別的基底類別而已。
    // 成員如果標記為抽象，或是包含在抽象類別 (Abstract Class) 內，則必須由衍生自此抽象類別的類別實作這個成員

    /*
     *  Abtract classe are intended for use as the base class for families of objects   
     *  that share certain centra characteristes sucg as a common purpose ansd structure.
     * 
     *  
     * 
     * 
     */
    public abstract class message
    {
        protected string msg;
        // 只有當存取是透過衍生類別型別進行時，才可以存取衍生類別中基底類別 (Base Class) 的 protected 成員  

        public string Msg
        {
            get    // define properity 
            {
                return msg;
            }
            set
            {
                msg = value;
            }
        }

        public message()
        {
            msg = "No Msg";
        }

        public message(string input)
        {
            msg = input;
        }

        public string output()
        {
            return msg;
        }
    }

    public class messageIn : message
    {
        public messageIn(string input)
            : base(input)                // base: input to constructor
        {
        }
    }




}

