using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Criteria = Unit4.Automation.Tests.Helpers.A.Criteria;
using System.Text;
using NUnit.Framework;
using Unit4.Automation.Commands;
using Unit4.Automation.Model;
using Unit4.Automation.Tests.Helpers;

namespace Unit4.Automation.Tests.Parser
{
    [TestFixture]
    internal class BcrCommandParserTests
    {
        private CommandParser<BcrOptions, ConfigOptions> _parser;

        [SetUp]
        public void Setup()
        {
            _parser = new CommandParser<BcrOptions, ConfigOptions>(TextWriter.Null);
        }

        [Test]
        public void GivenTheBcrCommandInAnyCase_ThenTheCommandShouldBeBcr(
            [Values("bcr", "BCR", "BcR", "Bcr")] string command)
        {
            Assert.That(_parser.GetOptions(command), Is.TypeOf(typeof(BcrOptions)));
        }

        [TestCaseSource(nameof(CaseDifferences))]
        public void GivenTheBcrCommand_ThenTheTierOptionShouldBeRecognised(A.Criteria criteria, string optionName)
        {
            var options = _parser.GetOptions("bcr", $"--{optionName}=myTier");
            var bcrOptions = options as BcrOptions;

            Assert.That(bcrOptions.ValueOf(criteria).Single(), Is.EqualTo("myTier"));
        }

        private static IEnumerable<TestCaseData> CaseDifferences
        {
            get
            {
                var criterias = (Criteria[]) Enum.GetValues(typeof(Criteria));
                foreach (var criteria in criterias)
                {
                    var lowerCase = criteria.Name().ToLowerInvariant();
                    var upperCase = criteria.Name().ToUpperInvariant();

                    var stringBuilder = new StringBuilder(upperCase.Length);
                    stringBuilder.Append(upperCase.First());
                    var firstLetterCapitalised = lowerCase.Skip(1)
                        .Aggregate(stringBuilder, (builder, c) => builder.Append(c)).ToString();

                    yield return new TestCaseData(criteria, lowerCase);
                    yield return new TestCaseData(criteria, upperCase);
                    yield return new TestCaseData(criteria, firstLetterCapitalised);
                }
            }
        }

        [Test]
        public void GivenTheBcrCommand_ThenTheTierOptionShouldTakeCommaSeparatedValues([Values] Criteria criteria)
        {
            var commandSeparatedTiers = new[] { "firstTier", "secondTier" };

            var options = _parser.GetOptions("bcr", $"--{criteria.Name()}={string.Join(",", commandSeparatedTiers)}");
            var bcrOptions = options as BcrOptions;

            Assert.That(bcrOptions.ValueOf(criteria), Is.EquivalentTo(commandSeparatedTiers));
        }

        [Test]
        public void GivenTheBcrCommand_ThenExtraCommasShouldBeIgnored(
            [Values] Criteria criteria,
            [Values("myTier,,,", ",myTier,,", ",,myTier,", ",,,myTier")]
            string option)
        {
            var options = _parser.GetOptions("bcr", $"--{criteria.Name()}={option}");

            var bcrOptions = options as BcrOptions;

            Assert.That(bcrOptions.ValueOf(criteria), Is.EquivalentTo(new[] { "myTier" }));
        }

        [Test]
        public void GivenTheBcrCommand_ThenTheOutputOptionShouldBeRecognised()
        {
            var path = @"C:\my\path\to\output";

            var options = _parser.GetOptions("bcr", $"--output={path}");
            var bcrOptions = options as BcrOptions;

            Assert.That(bcrOptions.OutputDirectory, Is.EqualTo(path));
        }

        [Test]
        public void GivenTheBcrCommand_ThenTheUpdateCacheOptionShouldBeRecognised()
        {
            var options = _parser.GetOptions("bcr", "--updatecache");
            var bcrOptions = options as BcrOptions;

            Assert.That(bcrOptions.UpdateCache, Is.True);
        }
    }
}