namespace ConsoleApp6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());

            ListRandom listRandom = new ListRandom();
            ListNode[] listNode = new ListNode[4];
          
            for (int i = 0; i < listNode.Length; i++)
            {
                listNode[i] = new ListNode();
            }       

            listNode[0].Data = "Вода";
            listNode[0].Previous = null;
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
                using (StreamWriter sWrite = new StreamWriter($"{Directory.GetCurrentDirectory() + (char)92}test.txt"))
                {                   
                    ListRandom listRandom = new ListRandom();
                    listRandom.Head = this.Head;
                    for (int i = 0; i < Count; i++)
                    {
                        sWrite.Write(listRandom.Head.Data + ":" + listRandom.Head.Random.Data + ";");
                        listRandom.Head = listRandom.Head.Next;
                    }
                }

                Console.WriteLine("\n=============Serialized=============");
            }
        
            public void Deserialize(Stream s)
            {
                Console.WriteLine("\n=============Deserialize=============");
                int elemCount; //считать количество
                string data;
                string[] dataSplit;

                this.Head = new ListNode() { };
                ListNode CurrentList = this.Head;

                using (StreamReader fileStream = new StreamReader($"{Directory.GetCurrentDirectory() + (char)92}test.txt"))
                { 
                    data=fileStream.ReadToEnd();
                }
               
                dataSplit = data.Split(";");
                elemCount = dataSplit.Length-1;
                Console.WriteLine("\nСчитанные и приведённые в читабельный вид данные");
                for (int i = 0; i < elemCount; i++)
                {
                    Console.WriteLine(dataSplit[i]);
                }
                Console.WriteLine();

                CurrentList.Data = dataSplit[0].Split(":")[0];
                CurrentList.Previous = null;
                CurrentList.Next = new ListNode();
                this.Count = elemCount;

                for (int i = 1; i < elemCount; i++)
                {
                    CurrentList.Next.Previous = CurrentList;
                    CurrentList = CurrentList.Next;
                    CurrentList.Data = dataSplit[i].Split(":")[0];
                        if (i != elemCount - 1)
                        CurrentList.Next = new ListNode();
                        else 
                        {
                            this.Tail = CurrentList;
                        }                    
                }

                CurrentList = this.Head;
                ListNode CopyList = CurrentList;
                Console.WriteLine("Проинициализированный лист ");
                Console.WriteLine(Head.Data + " " + Head.Next.Data + " " + Head.Next.Next.Data + " " +Head.Next.Next.Next.Data);
                               
                for (int j = 0; j < elemCount; j++)
                {
                    if(dataSplit[j].Split(":")[0] == CurrentList.Data)
                    {
                        for (int findRand = 0; findRand < elemCount; findRand++)
                        {
                            if (dataSplit[j].Split(":")[1] == CopyList.Data)
                            {
                                CurrentList.Random = CopyList;
                            }
                            CopyList = CopyList.Next;
                        }
                        CopyList = this.Head;
                    }
                    CurrentList = CurrentList.Next;
                }

                CurrentList = this.Head;
                Console.WriteLine("\nпроверка что все данные и рандомные элементы списка проинициализированы=-");
                Console.WriteLine(CurrentList.Data + " " + CurrentList.Random.Data);
                Console.WriteLine(CurrentList.Next.Data + " " + CurrentList.Next.Random.Data);
                Console.WriteLine(CurrentList.Next.Next.Data + " " + CurrentList.Next.Next.Random.Data);
                Console.WriteLine(CurrentList.Next.Next.Next.Data + " " + CurrentList.Next.Next.Next.Random.Data);

                Console.WriteLine("\n=============Deserialize=============");

                Console.ReadLine();
            }

        }
    }
}