using FluentAssertions;
using QuackService.Api.Logic;
using System;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace QuackService.Tests.UnitTests
{
    public class TopicFinderUnitTests
    {
        private readonly ITestOutputHelper _output;

        public TopicFinderUnitTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData("this quack has one #topic", new string[] { "#topic" })]
        [InlineData("this quack has #two #topics", new string[] { "#two", "#topics" })]
        public void GetTopic_ReturnsTopic(string quack, string[] expected)
        {
            // Arrange
            ITopicFinder topicFinder = new TopicFinder();

            // Act
            var topics = topicFinder.GetTopics(quack);
            _output.WriteLine(JsonSerializer.Serialize(topics).ToString());

            // Assert
            topics.Should().Contain(expected);
        }

        [Theory]
        [InlineData("#topic's", "topic")]
        [InlineData("nosuffix", "nosuffix")]
        public void TrimSuffix_ReturnsTopic(string word, string expected)
        {
            // Arrange
            ITopicFinder topicFinder = new TopicFinder();

            // Act
            var trimmedWord = topicFinder.TrimSuffix(word);

            // Assert
            trimmedWord.Should().Equals(expected);
        }
    }
}
