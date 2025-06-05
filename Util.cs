using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NpcItemFinder
{
    public class Util
    {
        public static List<string> FuzzySearch(
            string searchItem,
            string[] itemsToBeSearched,
            float thresholdPercent,
            int thresholdCharacters,
            int maxiumLengthForCharacterDifference)
        {
            List<string> matches = [];
            foreach (string item in itemsToBeSearched)
            {
                if (item.Length <= maxiumLengthForCharacterDifference)
                {
                    if (GetDifference(searchItem, item, false) <= thresholdCharacters)
                    {
                        matches.Add(item);
                    }
                }
                else
                {
                    if (GetDifference(searchItem, item, true) <= thresholdPercent)
                    {
                        matches.Add(item);
                    }
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
        private static float GetDifference(string str1, string str2, bool Threshhold)
        {
            if (str1 == str2)
            {
                return 0f;
            }

            int amountDifferent = 0;
            float percent = 0f;
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
            
            if (Threshhold == true)
            {
                return amountDifferent / (float)largerStr.Length;
            }
            else
            {
                return amountDifferent;
            }


        }
    }
}