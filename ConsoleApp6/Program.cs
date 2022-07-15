namespace ConsoleApp6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());

            ListRandom listRandom = new ListRandom();
            ListNode[] listNode = new ListNode[4];
            for (int i = 0; i < 4; i++)
            {
                listNode[i] = new ListNode();
            }

            //
            
            listNode[0].Data = "Воздух";
            listNode[0].Previous = null;
            listNode[0].Next = listNode[1];
            listNode[0].Random = listNode[1];

            listNode[1].Data = "ГРИФОН";
            listNode[1].Previous = listNode[0];
            listNode[1].Next = listNode[2];
            listNode[1].Random = listNode[3];

            listNode[2].Data = "компьютер";
            listNode[2].Previous = listNode[1];
            listNode[2].Next = listNode[3];
            listNode[2].Random = listNode[0];

            listNode[3].Data = "машина";
            listNode[3].Previous = listNode[2];
            listNode[3].Next = null;
            listNode[3].Random = listNode[2];

            listRandom.Count =listNode.Length;
            listRandom.Head = listNode[0];
            listRandom.Tail = listNode[3];

           
            Console.WriteLine();
            foreach (var item in listNode)
            {
                if (item.Next != null)
                    Console.WriteLine(item.Data + " " + item.Next.Data);
                else
                {
                    Console.WriteLine(item.Data + " " + "Следующего нету");
                }
            }            

            Stream stream = null;
            listRandom.Serialize(stream);
            Console.WriteLine("=============Deserialize=============");
            listRandom.Deserialize(stream);

            Console.ReadLine();
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
                List<ListNode> list = new List<ListNode>();
                for (ListNode thisNode=this.Head; thisNode != null; thisNode=thisNode.Next)
                { 
                    list.Add(thisNode);
                }

                using (StreamWriter sWrite = new StreamWriter($"{Directory.GetCurrentDirectory() + (char)92}test.txt"))
                {
                    foreach (ListNode item in list)
                    {
                        sWrite.Write(item.Data + ";" + item.Random.Data + ":");
                    }
                }

                Console.WriteLine("List serialized");
            }
        
            public void Deserialize(Stream s)
            {
                int elemCount = 4; //считать количество
                string data;
                string[] dataSplit;

                this.Head = new ListNode() { };
                ListNode current = this.Head;

                using (StreamReader fileStream = new StreamReader($"{Directory.GetCurrentDirectory() + (char)92}test.txt"))
                { 
                    data=fileStream.ReadToEnd();
                }
                

                dataSplit = data.Split(":");
                Console.WriteLine("\nСчитанные и приведённые в читабельный вид данныеданные");
                for (int i = 0; i < elemCount; i++)
                {
                    Console.WriteLine(dataSplit[i]);
                }
                Console.WriteLine();

                current.Data = dataSplit[0].Split(";")[0];
                current.Previous = null;
                current.Next = new ListNode();
                this.Count = elemCount;

                for (int i = 1; i < elemCount; i++)
                {
                        current.Next.Previous = current;
                        current = current.Next;
                        current.Data = dataSplit[i].Split(";")[0];
                        if (i != elemCount - 1)
                            current.Next = new ListNode();
                        else 
                        {
                            this.Tail = current;
                        }                    
                }

                current=this.Head;
                ListNode CopyList = current;
                Console.WriteLine("Проинициализированный лист ");
                Console.WriteLine(Head.Data + " " + Head.Next.Data + " " + Head.Next.Next.Data + " " +Head.Next.Next.Next.Data);
                               
                for (int j = 0; j < elemCount; j++)
                {
                    if(dataSplit[j].Split(";")[0] == current.Data)
                    {
                        for (int findRand = 0; findRand <= elemCount-1; findRand++)
                        {
                            if (dataSplit[j].Split(";")[1] == CopyList.Data)
                            {
                                current.Random = CopyList;
                            }
                            CopyList = CopyList.Next;
                        }
                        CopyList = this.Head;
                    }

                    current = current.Next;
                }
                
                current = this.Head;
                Console.WriteLine("\nпроверка что все данные и рандомные элементы списка проинициализированы=-");
                Console.WriteLine(current.Data + " " + current.Random.Data);
                Console.WriteLine(current.Next.Data + " " + current.Next.Random.Data);
                Console.WriteLine(current.Next.Next.Data + " " + current.Next.Next.Random.Data);
                Console.WriteLine(current.Next.Next.Next.Data + " " + current.Next.Next.Next.Random.Data);
                Console.ReadLine();
            }

        }
    }
}