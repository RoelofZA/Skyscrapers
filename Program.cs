using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using NUnit.Framework;

namespace Highrises
{
    class Program
    {
        static void Main(string[] args)
        {
            var clues = new int[]{ 2, 2, 1, 3,
                           2, 2, 3, 1,
                           1, 2, 2, 3,
                           3, 2, 1, 3};

            var expected = new[]{ new []{1, 3, 4, 2},
                                new []{4, 2, 1, 3},
                                new []{3, 4, 2, 1},
                                new []{2, 1, 3, 4 }};

            //var actual = Skyscrapers.SolvePuzzle(clues);

            //CollectionAssert.AreEqual(expected, actual);

            clues = new[]{ 0, 0, 1, 2,
                                       0, 2, 0, 0,
                                       0, 3, 0, 0,
                                       0, 1, 0, 0};

            expected = new[]{ new []{2, 1, 4, 3},
                                new []{3, 4, 1, 2},
                                new []{4, 2, 3, 1},
                                new []{1, 3, 2, 4}};

            var actual = Skyscrapers.SolvePuzzle(clues);
            CollectionAssert.AreEqual(expected, actual);

            //Console.WriteLine(actual == expected);
        }
    }
}

public class Skyscrapers
{
    static List<int[][]> alternative = new List<int[][]>() {
        new int[][] { new int[] { 4, 3, 2, 1 }, new int[] { 4, 3, 1, 2 }, new int[] { 4, 2, 3, 1 }, new int[] { 4, 2, 1, 3 }, new int[] { 4, 1, 3, 2 }, new int[] { 4, 1, 2, 3 } },
        new int[][] { new int[] { 3, 4, 2, 1 }, new int[] { 3, 4, 1, 2 }, new int[] { 3, 2, 4, 1 }, new int[] { 3, 1, 4, 2 }, new int[] { 3, 1, 2, 4 }, new int[] { 3, 2, 1, 4 }, new int[] { 2, 4, 1, 3 }, new int[] { 2, 4, 3, 1 }, new int[] { 2, 1, 4, 3 }, new int[] { 1, 4, 2, 3 }, new int[] { 1, 4, 3, 2 } },
        new int[][] { new int[] { 2, 3, 4, 1 }, new int[] { 1, 3, 4, 2 }, new int[] { 2, 1, 3, 4 }, new int[] { 2, 3, 1, 4 }, new int[] { 1, 3, 2, 4 }, new int[] { 1, 2, 4, 3 }  },
        new int[][] { new int[] { 1, 2, 3, 4 } }
    };

    static bool CompareArr(List<int[]> source, int[] b)
    {
        for (int arr = 0; arr < source.Count(); arr++)
        {
            bool result = true;
            for (int i = 0; i < source[arr].Length; i++)
            {
                result = result && source[arr][i] == b[i];
            }
            if (result) return true;
        }
        return false;
    }


    public static int[][] SolvePuzzle(int[] clues)
    {
        ArrayList collections = new ArrayList();

        // Build all collection
        List<int[]> allCollection = new List<int[]>();
        for (int outer = 0; outer < alternative.Count; outer++)
        {
            for (int inner = 0; inner < alternative[outer].Count(); inner++)
            {
                allCollection.Add(alternative[outer][inner]);
            }
        }

        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                int clue = y == 0 ? clues[(y * 4) + x] : clues[15 - x];
                int clueAlt = y == 0 ? clues[11 - x] : clues[4 + x];

                Console.WriteLine($"{clue} {clueAlt} - {x} {y}");

                List<int[]> tempCol = new List<int[]>();

                if (clue > 0)
                {
                    for (int i = 0; i < alternative[clue - 1].Count(); i++)
                    {
                        tempCol.Add(alternative[clue - 1][i]);
                    }
                }
                else if (clueAlt == 0)
                {
                    tempCol.AddRange(allCollection);
                }

                List<int[]> tempColFilter = new List<int[]>();

                if (clueAlt > 0)
                    for (int i = 0; i < alternative[clueAlt - 1].Count(); i++)
                    {
                        int[] invert = alternative[clueAlt - 1][i].Reverse().ToArray();
                        if (clue > 0)
                        {

                            if (CompareArr(tempCol, invert))
                                tempColFilter.Add(invert);
                        }
                        else
                        {
                            tempColFilter.Add(invert);
                        }
                    }
                else
                {
                    tempColFilter = tempCol;
                }

                collections.Add(tempColFilter);
            }
        }

        for (int zxc = 0; zxc < 4; zxc++)
        {
            // Check for possible X
            for (int x = 0; x < 4; x++)
            {
                List<int[]> collection = (List<int[]>)collections[x];
                List<int[]> collectionKeep = new List<int[]>();

                foreach (var item in collection)
                {
                    bool chk = false;
                    for (int y = 0; y < 4; y++)
                    {
                        chk = false;
                        List<int[]> collectionY = (List<int[]>)collections[4 + y];
                        foreach (var itemY in collectionY)
                        {
                            if (itemY[x] == item[y])
                            {
                                chk = true;
                                break;
                            }
                        }
                        if (!chk)
                            break;
                    }
                    if (chk) collectionKeep.Add(item);

                }
                collections[x] = collectionKeep;

            }

            // Check for possible Y
            for (int x = 0; x < 4; x++)
            {
                List<int[]> collection = (List<int[]>)collections[4 + x];
                List<int[]> collectionKeep = new List<int[]>();

                foreach (var item in collection)
                {
                    bool chk = false;
                    for (int y = 0; y < 4; y++)
                    {
                        chk = false;
                        List<int[]> collectionY = (List<int[]>)collections[y];
                        foreach (var itemY in collectionY)
                        {
                            if (itemY[x] == item[y])
                            {

                                chk = true;
                                break;
                            }
                        }
                        if (!chk)
                            break;
                    }
                    if (chk) collectionKeep.Add(item);

                }
                collections[x + 4] = collectionKeep;

            }
        }

        // for (int x = 0; x < 4; x++)
        // {
        //     List<int[]> collection = (List<int[]>)collections[x];
        //     List<int[]> collectionKeep = new List<int[]>();

        //     foreach (var item in collection)
        //     {
        //         bool chk = false;
        //         for (int y = 0; y < 4; y++)
        //         {
        //             chk = false;
        //             List<int[]> collectionY = (List<int[]>)collections[4+y];
        //             foreach (var itemY in collectionY)
        //             {
        //                 if (itemY[x] == item[y]) {
        //                     chk = true;
        //                     break;
        //                 }
        //             }
        //             if (!chk) 
        //                 break;
        //         }
        //         if (chk) collectionKeep.Add(item);

        //     }
        //     collections[x] = collectionKeep;

        // }


        // Populate the Final structure
        var result = new[]{ new []{0, 0, 0, 0},
                                new []{0, 0, 0, 0},
                                new []{0, 0, 0, 0},
                                new []{0, 0, 0, 0}};

        // Apply ones to result
        for (int i = 0; i < collections.Count; i++)
        {
            List<int[]> item = (List<int[]>)collections[i];
            if (item.Count == 1)
            {
                if (i < 4)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        result[y][i] = item[0][y];
                    }
                }
                else
                {
                    result[i - 4] = item[0].ToArray();
                }
            }
        }



        // Start your coding here...
        return result;
    }
}
