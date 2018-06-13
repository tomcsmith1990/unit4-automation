using System;
using Unit4.Automation;
using NUnit.Framework;
using System.IO;
using Unit4.Automation.Model;
using System.Linq;
using Criteria = Unit4.Automation.Tests.Helpers.A.Criteria;
using Unit4.Automation.Tests.Helpers;
using System.Collections.Generic;
using System.Text;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class CommandParserTests
    {
        private CommandParser<BcrOptions> _parser;

        [SetUp]
        public void Setup()
        {
            _parser = new CommandParser<BcrOptions>(TextWriter.Null);
        }

        [Test]
        public void GivenNoArguments_ThenTheCommandShouldBeHelp()
        {
            Assert.That(_parser.GetOptions(), Is.TypeOf(typeof(NullOptions)));
        }

        [Test]
        public void GivenAnUnknownCommand_ThenTheCommandShouldBeHelp()
        {
            Assert.That(_parser.GetOptions("unknown"), Is.TypeOf(typeof(NullOptions)));
        }

        [TestCase("bcr")]
        [TestCase("BCR")]
        [TestCase("BcR")]
        [TestCase("Bcr")]
        public void GivenTheBcrCommandInAnyCase_ThenTheCommandShouldBeBcr(string command)
        {
            Assert.That(_parser.GetOptions(command), Is.TypeOf(typeof(BcrOptions)));
        }

        [TestCaseSource("CaseDifferences")]
        public void GivenTheBcrCommand_ThenTheTier2OptionShouldBeRecognised(Criteria criteria, string optionName)
        {
            var options = _parser.GetOptions("bcr", string.Format("--{0}=myTier", optionName));
            var bcrOptions = options as BcrOptions;
            
            Assert.That(bcrOptions.ValueOf(criteria).Single(), Is.EqualTo("myTier"));
        }

        private static IEnumerable<TestCaseData> CaseDifferences
        {
            get
            {
                var criterias = (Criteria[])Enum.GetValues(typeof(Criteria));
                foreach (var criteria in criterias)
                {
                    var lowerCase = criteria.Name().ToLowerInvariant();
                    var upperCase = criteria.Name().ToUpperInvariant();

                    var stringBuilder = new StringBuilder(upperCase.Length);
                    stringBuilder.Append(upperCase.First());
                    var firstLetterCapitalised = lowerCase.Skip(1).Aggregate(stringBuilder, (builder, c) => builder.Append(c)).ToString();

                    yield return new TestCaseData(criteria, lowerCase);
                    yield return new TestCaseData(criteria, upperCase);
                    yield return new TestCaseData(criteria, firstLetterCapitalised);
                }
            }
        }

        [TestCase(Criteria.Tier2)]
        [TestCase(Criteria.Tier3)]
        public void GivenTheBcrCommand_ThenTheTierOptionShouldTakeCommaSeparatedValues(Criteria criteria)
        {
            var commandSeparatedTiers = new string[] { "firstTier2", "secondTier2" };
            
            var options = _parser.GetOptions("bcr", string.Format("--{0}={1}", criteria.Name(), string.Join(",", commandSeparatedTiers)));
            var bcrOptions = options as BcrOptions;

            Assert.That(bcrOptions.ValueOf(criteria), Is.EquivalentTo(commandSeparatedTiers));
        }

        [TestCaseSource("ExtraCommas")]
        public void GivenTheBcrCommand_ThenExtraCommasShouldBeIgnored(Criteria criteria, string option)
        {
            var options = _parser.GetOptions("bcr", string.Format("--{0}={1}", criteria.Name(), option));
            var bcrOptions = options as BcrOptions;

            Assert.That(bcrOptions.ValueOf(criteria), Is.EquivalentTo(new string[] { "myTier" }));
        }

        private static IEnumerable<TestCaseData> ExtraCommas
        {
            get
            {
                var criterias = (Criteria[])Enum.GetValues(typeof(Criteria));
                foreach (var criteria in criterias)
                {
                    yield return new TestCaseData(criteria, "myTier,,,");
                    yield return new TestCaseData(criteria, ",myTier,,");
                    yield return new TestCaseData(criteria, ",,myTier,");
                    yield return new TestCaseData(criteria, ",,,myTier");
                }
            }
        }
    }
}