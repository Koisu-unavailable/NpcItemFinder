using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NpcItemFinder
{
    public class Util
    {
        public static int[] ConvertCopperToCoins(int copper)
        {
            // TODO: Implment
            throw new NotImplementedException();
        }
        public static List<string> FuzzySearch(
            string searchItem,
            string[] itemsToBeSearched,
            int thresholdCharacters)
        {
            List<string> matches = [];
            foreach (string item in itemsToBeSearched)
            {
                int difference = GetDifference(searchItem.ToLower(), item.ToLower());
                if (item.Contains(searchItem, StringComparison.CurrentCultureIgnoreCase))
                {
                    matches.Add(item);
                    continue;
                }
                if (difference <= searchItem.Length)
                {
                    continue;
                }
                if (difference <= thresholdCharacters)
                {
                    matches.Add(item);
                    continue;
                }

            }
            return matches;
        }
        /// <summary>
        /// Get the difference between 2 strings using Levenshtein algorithim
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="Threshhold">If this is set to true, the function will return the percent difference rather than the amount of characters different</param>
        /// <returns>The difference between the 2 strings or the amount of characters, depending on what you specified.</returns>
        public static int GetDifference(string str1, string str2)
        {
            if (str1 == str2)
            {
                return 0;
            }

            int amountDifferent = 0;
            string largerStr = str1.Length > str2.Length ? str1 : str2;
            string shorterStr = str1.Length < str2.Length ? str1 : str2;
            if (str1.Length == str2.Length)
            {
                largerStr = str1;
                shorterStr = str2;
            }


            foreach (char c in shorterStr)
            {
                if (c != largerStr.ToCharArray()[shorterStr.IndexOf(c)])
                {
                    amountDifferent++;
                }
            }
            amountDifferent += Math.Abs(str1.Length - str2.Length);
            return amountDifferent;

        }
    }
}