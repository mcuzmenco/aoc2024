var input = File.ReadAllText("real.txt").ToArray();
var fileSystem = GetFileSystem(input).ToList();
Part1(fileSystem);
fileSystem = GetFileSystem(input).ToList();
Part2(fileSystem);
void Part1(List<int> list)
{
   var j = list.Count - 1;
   for (int i = 0; i < list.Count; i++)
   {
      if (list[i] != -1)
      {
         continue;
      }

      while (list[j] == -1)
      {
         j--;
      }
   
      if (i > j)
      {
         break;
      }

      list[i] = list[j];
      list[j] = -1;
      j--;
   }
   
   var checksum = Checksum(fileSystem);
   Console.WriteLine(checksum);
}

void Part2(List<int> list)
{
   var j = list.Count - 1;
   while (j > 0)
   {
      if (list[j] == -1)
      {
         j--;
         continue;
      }
      
      var capacityRequired = 0;
      while (j - capacityRequired > 0 && list[j] == list[j - capacityRequired])
      {
         capacityRequired++;
      }

      if (TryGetFirstAvailableNumberToFill(capacityRequired, j, list, out var freeSpaceStartIndex))
      {
         for (int k = 0; k < capacityRequired; k++)
         {
            list[freeSpaceStartIndex + k] = list[j - k];
            list[j - k] = -1;
         }
      }
      
      j-= capacityRequired;
   }
   
   var checksum = Checksum(list);
   Console.WriteLine(checksum);
}

bool TryGetFirstAvailableNumberToFill(int capacityRequired, int tryUntil, List<int> list, out int startingIndex)
{
   startingIndex = -1;
   var spacesAvailable = 0;
   var i = 0;
   while (i < tryUntil && spacesAvailable < capacityRequired)
   {
      if (list[i] == -1)
      {
         if (spacesAvailable == 0)
         {
            startingIndex = i;
         }
         
         spacesAvailable++;
      }
      else
      {
         spacesAvailable = 0;
         startingIndex = -1;
      }

      i++;
   }
   
   return spacesAvailable >= capacityRequired;
}


IEnumerable<int> GetFileSystem(char[] chars)
{
   var id = 0;
   for (var i = 0; i < chars.Length; i++)
   {
      var c = i % 2 == 0 ? id++ : -1;
      var number = int.Parse(chars[i].ToString());  
      for (var j = 0; j < number; j++)
      {
         yield return c;
      }
   }
}

long Checksum(List<int> ints)
{
   long l = 0;
   for (int i = 0; i < ints.Count; i++)
   {
      if (ints[i] != -1)
      {
         l += ints[i] * i;
      }
   }

   return l;
}

