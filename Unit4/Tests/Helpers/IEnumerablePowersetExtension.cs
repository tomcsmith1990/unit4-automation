using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace Unit4.Automation.Tests.Helpers
{
    [TestFixture]
    internal static class IEnumerablePowersetExtension
    {
        public static IEnumerable<IEnumerable<T>> Powerset<T>(this IEnumerable<T> enumerable)
        {
            throw new NotSupportedException();
        }

        [Test]
        public static void GivenTheEmptySet_ThenThePowersetShouldBeTheEmptySet()
        {
            var set = new int[] {};

            Assert.That(set.Powerset(), Is.EquivalentTo(new int[][] { new int[] { } }));
        }
    }
}