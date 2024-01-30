using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonsterQuest
{
    public static class StringHelper
    {
        public static string JoinWithAnd(IEnumerable<string> items, bool useSerialComma = true)
        {
            int count = items.Count();

            if (count == 0)
            {
                return "";
            }
            else if (count == 1)
            {
                return items.ElementAt(0);
            }
            else if (count == 2)
            {
                return items.ElementAt(0) + " and " + items.ElementAt(1);
            }
            else
            {
                List<string> itemsCopy = new List<string>(items);

                if (useSerialComma)
                {
                    itemsCopy[itemsCopy.Count - 1] = "and " + itemsCopy[itemsCopy.Count - 1];

                    return string.Join(", ", itemsCopy);
                }
                else
                {
                    itemsCopy[itemsCopy.Count - 2] = itemsCopy[itemsCopy.Count - 2] + " and " + itemsCopy[itemsCopy.Count - 1];
                    itemsCopy.RemoveAt(itemsCopy.Count - 1);

                    return string.Join(", ", itemsCopy);
                }
            }
        }
    }
}
