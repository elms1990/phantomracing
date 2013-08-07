using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhantomRacing
{
    public static class LinkedListExtensions
    {
        /// <summary>
        /// Appends list2 on list1.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="ll1">List to be copied to.</param>
        /// <param name="ll2">List to be copied from.</param>
        public static void AppendLinkedList<T>(this LinkedList<T> ll1, IEnumerable<T> ll2)
        {
            foreach (T t in ll2)
            {
                ll1.AddLast(t);
            }
        }
    }
}
