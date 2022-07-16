namespace ConsoleApp6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ListNode[] listNode = InitList();
            ListRandom listRandom = InitListRandom(listNode);

            foreach (var item in listNode)
                Console.WriteLine(item.Data + " " + ((item.Next != null) ? item.Next.Data : "Следующего нет"));

            using (Stream stream = new FileStream($"{Directory.GetCurrentDirectory()}\\test.txt", FileMode.Create))
            {
                listRandom.Serialize(stream);
            }
            using (Stream stream = new FileStream($"{Directory.GetCurrentDirectory()}\\test.txt", FileMode.Open))
            {
                listRandom.Deserialize(stream);
            }
            Console.ReadLine();
        }

        public static ListNode[] InitList()
        {
            ListNode[] listNode = new ListNode[4];

            for (int i = 0; i < listNode.Length; i++)
                listNode[i] = new ListNode();

            listNode[0].Data = "Вода";
            listNode[0].Next = listNode[1];
            listNode[0].Random = listNode[1];

            listNode[1].Data = "Земля";
            listNode[1].Previous = listNode[0];
            listNode[1].Next = listNode[2];
            listNode[1].Random = listNode[3];

            listNode[2].Data = "Огонь";
            listNode[2].Previous = listNode[1];
            listNode[2].Next = listNode[3];
            listNode[2].Random = listNode[0];

            listNode[3].Data = "Воздух";
            listNode[3].Previous = listNode[2];
            listNode[3].Random = listNode[2];

            return listNode;
        }

        public static ListRandom InitListRandom(ListNode[] listNode)
        {
            return new ListRandom
            {
                Count = listNode.Length,
                Head = listNode[0],
                Tail = listNode[^1]
            };
        }
    }


    class ListNode
        {
            public ListNode Previous;
            public ListNode Next;
            public ListNode Random;
            public string Data;
        }

    class ListRandom
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(Stream s)
        {
            ListRandom listRandom = new ListRandom();
            listRandom.Head = this.Head;
            string str = string.Empty;
            for (int i = 0; i < Count; i++)
            {
                str += listRandom.Head.Data + ":" + listRandom.Head.Random.Data + ";";
                listRandom.Head = listRandom.Head.Next;
            }

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            s.Write(buffer, 0, buffer.Length);

            Console.WriteLine("\n=============Serialized=============");
        }

        public void Deserialize(Stream s)
        {
            Console.WriteLine("\n=============Deserialize=============");
            Head = new ListNode() { };
            ListNode CurrentList = Head;
            string[] data;

            using (StreamReader sr = new StreamReader(s))
            {
                data = sr.ReadToEnd().Split(";");
            }
            Count = data.Length - 1;

            Console.WriteLine("\nСчитанные и приведённые в читабельный вид данные");
            foreach (var item in data)
                Console.WriteLine(item);

            CurrentList.Data = data[0].Split(":")[0];
            CurrentList.Next = new ListNode();

            for (int i = 1; i < Count; i++)
            {
                CurrentList.Next = new ListNode { Previous = CurrentList };
                CurrentList = CurrentList.Next;
                CurrentList.Data = data[i].Split(":")[0];
                if (i == Count - 1)
                {
                    this.Tail = CurrentList;
                    continue;
                }
            }

            CurrentList = Head;
            ListNode CopyList = CurrentList;
            Console.WriteLine("Проинициализированный лист");
            while (CurrentList.Next != null)
            {
                Console.WriteLine(CurrentList.Data + " ");
                CurrentList = CurrentList.Next;
                if (CurrentList.Data != null && CurrentList.Next == null)
                    Console.WriteLine(CurrentList.Data);
            }

            CurrentList = Head;

            for (int j = 0; j < Count; j++)
            {
                if (data[j].Split(":")[0] == CurrentList.Data)
                {
                    for (int findRand = 0; findRand < Count; findRand++)
                    {
                        if (data[j].Split(":")[1] == CopyList.Data)
                        {
                            CurrentList.Random = CopyList;
                            break;
                        }
                        CopyList = CopyList.Next;
                    }
                    CopyList = this.Head;
                }
                CurrentList = CurrentList.Next;
            }

            CurrentList = Head;
            Console.WriteLine("\nПроверка что все данные и рандомные элементы списка проинициализированы");
            while (CurrentList.Next != null)
            {
                Console.WriteLine(CurrentList.Data + " " + CurrentList.Random.Data);
                CurrentList = CurrentList.Next;
            }
            Console.WriteLine("\n=============Deserialized=============");
        }
    }
}