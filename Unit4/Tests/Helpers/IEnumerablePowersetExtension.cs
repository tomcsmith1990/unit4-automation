using System;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;

namespace Unit4.Automation.Tests.Helpers
{
    [TestFixture]
    internal static class IEnumerablePowersetExtension
    {
        public static IEnumerable<IEnumerable<T>> Powerset<T>(this IEnumerable<T> enumerable)
        {
            var items = 1 << enumerable.Count();
            for (int bitmask = 0; bitmask < items; bitmask++)
            {
                if ((bitmask >> 0 & 1) == 1)
                {
                    yield return new T[] { enumerable.ElementAt(0) };
                }
            }
            yield return Enumerable.Empty<T>();
        }

        [Test]
        public static void GivenTheEmptySet_ThenThePowersetShouldBeTheEmptySet()
        {
            var set = new int[] {};

            Assert.That(set.Powerset(), Is.EquivalentTo(new int[][] { new int[] { } }));
        }

        [Test]
        public static void GivenOneElement_ThenThePowersetShouldBeThatElementAndTheEmptySet()
        {
            var set = new int[] { 1 };

            Assert.That(set.Powerset(), Is.EquivalentTo(new int[][] { new int[] { }, new int[] { 1 } }));
        }

        [Test]
        public static void GivenTwoElements_ThenThePowersetShouldBeAllCombinations()
        {
            var set = new int[] { 1, 2 };

            var expected = new int[][] { new int[] { }, new int[] { 1 }, new int[] { 2 }, new int[] { 1, 2 } };

            Assert.That(set.Powerset(), Is.EquivalentTo(expected));
        }
    }
}