using System;
using System.Collections.Generic;
using System.Linq;

namespace Next.PCL.Extensions
{
	/// <summary>
	/// At the outset strived to calculate Median.
	/// </summary>
	/// <remarks>
	/// Original Source: <a href="https://stackoverflow.com/a/22702269/3499361"/>
	/// </remarks>
	public static class MathExts
    {
		/// <summary>
		/// Partitions the given list around a pivot element such that all elements on left of pivot are <= pivot
		/// and the ones at thr right are > pivot. This method can be used for sorting, N-order statistics such as
		/// as median finding algorithms.
		/// Pivot is selected ranodmly if random number generator is supplied else its selected as last element in the list.
		/// Reference: Introduction to Algorithms 3rd Edition, Corman et al, pp 171
		/// </summary>
		private static int Partition<T>(this IList<T> list, int start, int end, Random rnd = null) 
			where T : IComparable<T>
		{
			if (rnd != null)
				list.Swap(end, rnd.Next(start, end + 1));

			var pivot = list[end];
			var lastLow = start - 1;
			for (var i = start; i < end; i++)
			{
				if (list[i].CompareTo(pivot) <= 0)
					list.Swap(i, ++lastLow);
			}
			list.Swap(end, ++lastLow);
			return lastLow;
		}
		/// <summary>
		/// Returns Nth smallest element from the list. Here n starts from 0 so that n=0 returns minimum, n=1 returns 2nd smallest element etc.
		/// Note: specified list would be mutated in the process.
		/// Reference: Introduction to Algorithms 3rd Edition, Corman et al, pp 216
		/// </summary>
		public static T NthOrderStatistic<T>(this IList<T> list, int n, Random rnd = null) 
			where T : IComparable<T>
		{
			return NthOrderStatistic(list, n, 0, list.Count - 1, rnd);
		}
		private static T NthOrderStatistic<T>(this IList<T> list, int n, int start, int end, Random rnd) 
			where T : IComparable<T>
		{
			while (true)
			{
				var pivotIndex = list.Partition(start, end, rnd);
				if (pivotIndex == n)
					return list[pivotIndex];

				if (n < pivotIndex)
					end = pivotIndex - 1;
				else
					start = pivotIndex + 1;
			}
		}

		public static void Swap<T>(this IList<T> list, int i, int j)
		{
			if (i == j)   //This check is not required but Partition function may make many calls so its for perf reason
				return;
			var temp = list[i];
			list[i] = list[j];
			list[j] = temp;
		}
		/// <summary>
		/// Note: specified list would be mutated in the process.
		/// </summary>
		public static T Median<T>(this IList<T> list) 
			where T : IComparable<T>
		{
			if (list.Count <= 0)
				return default(T);
			return list.NthOrderStatistic((list.Count - 1) / 2);
		}

		public static double Median<T>(this IEnumerable<T> sequence, Func<T, double> getValue)
		{
			var list = sequence.Select(getValue).ToList();
			var mid = (list.Count - 1) / 2;
			return list.NthOrderStatistic(mid);
		}

		public static int Median(params int[] arr)
		{
			int min = arr[0];
			int max = arr[arr.Length-1];

			Console.WriteLine("Min: {0}, Max: {1}", min, max);
			int i = 0;
			while (i < arr.Length)
            {
				int v = arr[i];
				if (v < min)
					min = v;
				if (v > max)
					max = v;
				Console.WriteLine("{0} | Min: {1}, Max: {2}", i, min, max);
				i++;
            }
			return (max + min) / 2;
		}
	}
}