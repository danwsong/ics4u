using System;
using System.Diagnostics;

namespace Sorting
{
    class MainClass
    {
        // Swaps the two elements at indices i and j
        static void Swap(ref int[] data, int i, int j)
        {
            int temp = data[i];
            data[i] = data[j];
            data[j] = temp;
        }

        // Sorts data using bubble sort algorithm
        static void BubbleSort(ref int[] data)
        {
            // The length of the subarray that still needs to be sorted
            int N = data.Length;
            while (N > 0)
            {
                // The length of the new subarray after this iteration through the loop
                int newN = 0;
                for (int i = 1; i < N; i++)
                {
                    // Compare each consecutive pair of elements, if wrong order, swap them
                    if (data[i] < data[i - 1])
                    {
                        Swap(ref data, i, i - 1);
                        // If swapped, elements before the swapped pair are not in sorted order yet
                        // New unsorted subarray length is changed to reflect that
                        newN = i;
                    }
                }
                // Set new unsorted subarray length
                N = newN;
            }
        }

        // Sorts data using selection sort algorithm
        static void SelectionSort(ref int[] data)
        {
            // i keeps track of the last index of the sorted subarray
            // This is where the minimum of the unsorted subarray will be swapped in
            for (int i = 0; i < data.Length - 1; i++)
            {
                // Set the minimum of the unsorted subarray to the first element
                int currentMin = i;
                // Iterate through and look for elements that are smaller
                for (int j = i + 1; j < data.Length; j++)
                    if (data[j] < data[currentMin])
                        currentMin = j;
                // The minimum of the unsorted subarray is then swapped in as the last element of the sorted subarray
                // Slight optimization to make sure that swapping an element into its own index doesn't occur
                if (currentMin != i)
                    Swap(ref data, currentMin, i);
            }
        }

        // Sorts data using insertion sort algorithm
        static void InsertionSort(ref int[] data)
        {
            // i keeps track of the index of the next element to be inserted into its correct position
            // i = 0 can be skipped as a subarray of 1 element is already sorted
            for (int i = 1; i < data.Length; i++)
            {
                // Save the element to insert
                int elementToInsert = data[i], j;
                // Shift all of the elements larger than the element up one index
                for (j = i - 1; j >= 0 && data[j] > elementToInsert; j--)
                {
                    data[j + 1] = data[j];
                }
                // Insert the saved element into the newly vacated index
                data[j + 1] = elementToInsert;
            }
        }

        // Sorts data using merge sort algorithm
        static void MergeSort(ref int[] data)
        {
            // Merge sort the entire array
            MergeSort(ref data, 0, data.Length - 1);
        }

        // Merge sort a section of an array between the start and end indexes
        static void MergeSort(ref int[] data, int start, int end)
        {
            // If the section of the array only has 1 element, it is already sorted
            // Otherwise sort using merge sort algorithm
            if (start < end)
            {
                // Merge sort the left half of the array
                MergeSort(ref data, start, (start + end) / 2);
                // Merge sort the right half of the array
                MergeSort(ref data, (start + end) / 2 + 1, end);
                // Merge the two sorted halves together to form a full sorted section
                Merge(ref data, start, end);
            }
        }

        // Merge two sorted halves of a section of an array into a full sorted section
        static void Merge(ref int[] data, int start, int end)
        {
            // Create a holder array to merge the elements into, the same same size as the section of the array to be merged
            int[] work = new int[end - start + 1];
            // Start indexes for the left and right halves that will be merged
            int left = start, right = (start + end) / 2 + 1;
            // Loop through the holder array and fill it up with elements from both halves
            for (int i = 0; i < work.Length; i++)
            {
                // Look at the next elements in each half and place the lower element into the holder array at index i
                if (right > end || left <= (start + end) / 2 && data[left] < data[right])
                    work[i] = data[left++];
                else
                    work[i] = data[right++];
            }
            // Copy the contents of the holder array back into the original array
            for (int i = 0; i < work.Length; i++)
                data[start + i] = work[i];
        }

        // Sorts data using quicksort algorithm
        static void QuickSort(ref int[] data)
        {
            // Quicksort the entire array
            QuickSort(ref data, 0, data.Length - 1);
        }

        // Quicksort a section of the array between the start and end indexes
        static void QuickSort(ref int[] data, int start, int end)
        {
            // If the section of the array only has 1 element, it is already sorted
            // Otherwise sort using quicksort algorithm
            if (start < end)
            {
                // Use the partition algorithm to find a pivot
                // Elements lower than the pivot are now to the left, greater are to the right of the pivot
                // Pivot is now in the correct position
                int pivot = Partition(ref data, start, end);
                // Quicksort the elements on both left and right side of the pivot
                QuickSort(ref data, start, pivot - 1);
                QuickSort(ref data, pivot + 1, end);
            }
        }

        // Performs the partition algorithm on a section of an array, returning the index of the pivot
        static int Partition(ref int[] data, int start, int end)
        {
            // Choose the last element as the pivot
            int pivot = data[end];
            // leftIndex stores the last index of the left side of the array containing elements smaller than the pivot
            int leftIndex = start;
            // Iterate through the array, swapping elements smaller than the pivot into the left side
            for (int current = start; current < end; current++)
                if (data[current] <= pivot)
                    Swap(ref data, leftIndex++, current);
            // Swap the pivot into the index next to the left side of the array
            Swap(ref data, leftIndex, end);
            // Return the index of the pivot
            return leftIndex;
        }

        public static void Main(string[] args)
        {
            const int FACTOR = 10;
            Random random = new Random();
            Stopwatch sw = new Stopwatch();
            // Test the algorithms with arrays of size 100000, 10000, 1000, 100, and 10
            for (int pow = 5; pow >= 1; pow--)
            {
                Console.WriteLine("Testing algorithms on {0} elements...", (int)Math.Pow(FACTOR, pow));
                int[] test = new int[(int)Math.Pow(FACTOR, pow)], actualTest = new int[test.Length];
                for (int i = 0; i < test.Length; i++)
                    test[i] = random.Next();

                for (int i = 0; i < test.Length; i++)
                    actualTest[i] = test[i];
                sw.Restart();
                SelectionSort(ref actualTest);
                sw.Stop();
                Console.WriteLine("Finished sorting {0} elements with selection sort in {1} ms", (int)Math.Pow(FACTOR, pow), sw.Elapsed.TotalMilliseconds);

                for (int i = 0; i < test.Length; i++)
                    actualTest[i] = test[i];
                sw.Restart();
                BubbleSort(ref actualTest);
                sw.Stop();
                Console.WriteLine("Finished sorting {0} elements with bubble sort in {1} ms", (int)Math.Pow(FACTOR, pow), sw.Elapsed.TotalMilliseconds);

                for (int i = 0; i < test.Length; i++)
                    actualTest[i] = test[i];
                sw.Restart();
                InsertionSort(ref actualTest);
                sw.Stop();
                Console.WriteLine("Finished sorting {0} elements with insertion sort in {1} ms", (int)Math.Pow(FACTOR, pow), sw.Elapsed.TotalMilliseconds);

                for (int i = 0; i < test.Length; i++)
                    actualTest[i] = test[i];
                sw.Restart();
                MergeSort(ref actualTest);
                sw.Stop();
                Console.WriteLine("Finished sorting {0} elements with merge sort in {1} ms", (int)Math.Pow(FACTOR, pow), sw.Elapsed.TotalMilliseconds);

                for (int i = 0; i < test.Length; i++)
                    actualTest[i] = test[i];
                sw.Restart();
                QuickSort(ref actualTest);
                sw.Stop();
                Console.WriteLine("Finished sorting {0} elements with quick sort in {1} ms", (int)Math.Pow(FACTOR, pow), sw.Elapsed.TotalMilliseconds);

                Console.WriteLine();
            }
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
