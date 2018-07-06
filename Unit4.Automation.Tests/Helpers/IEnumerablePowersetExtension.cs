using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

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
                yield return enumerable.ApplyBitmask(bitmask);
            }
        }

        private static IEnumerable<T> ApplyBitmask<T>(this IEnumerable<T> enumerable, int bitmask)
        {
            var length = enumerable.Count();
            for (int item = 0; item < length; item++)
            {
                if ((bitmask >> item & 1) == 1)
                {
                    yield return enumerable.ElementAt(item);
                }
            }
        }

        [Test]
        public static void GivenTheEmptySet_ThenThePowersetShouldBeTheEmptySet()
        {
            var set = new int[] { };

            Assert.That(set.Powerset(), Is.EquivalentTo(new int[][] { new int[] { } }));
        }

        [Test]
        public static void GivenOneElement_ThenThePowersetShouldBeThatElementAndTheEmptySet()
        {
            var set = new[] { 1 };

            Assert.That(set.Powerset(), Is.EquivalentTo(new int[][] { new int[] { }, new[] { 1 } }));
        }

        [Test]
        public static void GivenTwoElements_ThenThePowersetShouldBeAllCombinations()
        {
            var set = new[] { 1, 2 };

            var expected = new int[][] { new int[] { }, new[] { 1 }, new[] { 2 }, new[] { 1, 2 } };

            Assert.That(set.Powerset(), Is.EquivalentTo(expected));
        }

        [Test]
        public static void GivenThreeElements_ThenThePowersetShouldBeAllCombinations()
        {
            var set = new[] { 1, 2, 3 };

            var expected = new int[][]
            {
                new int[] { },
                new[] { 1 },
                new[] { 2 },
                new[] { 3 },
                new[] { 1, 2 },
                new[] { 1, 3 },
                new[] { 2, 3 },
                new[] { 1, 2, 3 }
            };

            Assert.That(set.Powerset(), Is.EquivalentTo(expected));
        }
    }
}