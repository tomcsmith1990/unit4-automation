using System;
using NUnit.Framework;
using System.IO;
using Unit4.Automation.Model;
using System.Linq;
using Criteria = Unit4.Automation.Tests.Helpers.A.Criteria;
using Unit4.Automation.Tests.Helpers;
using System.Collections.Generic;
using System.Text;

namespace Unit4.Automation.Tests.Parser
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

        [Test]
        public void GivenTheBcrCommandInAnyCase_ThenTheCommandShouldBeBcr(
            [Values("bcr", "BCR", "BcR", "Bcr")] string command)
        {
            Assert.That(_parser.GetOptions(command), Is.TypeOf(typeof(BcrOptions)));
        }

        [TestCaseSource("CaseDifferences")]
        public void GivenTheBcrCommand_ThenTheTierOptionShouldBeRecognised(Criteria criteria, string optionName)
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

        [Test]
        public void GivenTheBcrCommand_ThenTheTierOptionShouldTakeCommaSeparatedValues([Values]Criteria criteria)
        {
            var commandSeparatedTiers = new [] { "firstTier", "secondTier" };
            
            var options = _parser.GetOptions("bcr", string.Format("--{0}={1}", criteria.Name(), string.Join(",", commandSeparatedTiers)));
            var bcrOptions = options as BcrOptions;

            Assert.That(bcrOptions.ValueOf(criteria), Is.EquivalentTo(commandSeparatedTiers));
        }

        [Test]
        public void GivenTheBcrCommand_ThenExtraCommasShouldBeIgnored(
            [Values] Criteria criteria, 
            [Values("myTier,,,", ",myTier,,", ",,myTier,", ",,,myTier")] string option)
        {
            var options = _parser.GetOptions("bcr", string.Format("--{0}={1}", criteria.Name(), option));

            var bcrOptions = options as BcrOptions;

            Assert.That(bcrOptions.ValueOf(criteria), Is.EquivalentTo(new [] { "myTier" }));
        }

        [Test]
        public void GivenTheBcrCommand_ThenTheOutputOptionShouldBeRecognised()
        {
            var path = @"C:\my\path\to\output";

            var options = _parser.GetOptions("bcr", string.Format("--output={0}", path));
            var bcrOptions = options as BcrOptions;

            Assert.That(bcrOptions.OutputDirectory, Is.EqualTo(path));
        }
    }
}