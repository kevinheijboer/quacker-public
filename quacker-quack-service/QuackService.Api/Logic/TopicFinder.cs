using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuackService.Api.Logic
{
    public class TopicFinder : ITopicFinder
    {
        public string[] GetTopics(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"[@#][\w_-]+");

            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        select TrimSuffix(m.Value);

            return words.ToArray();
        }

        public string TrimSuffix(string word)
        {
            int apostropheLocation = word.IndexOf('\'');
            if (apostropheLocation != -1)
            {
                word = word.Substring(0, apostropheLocation);
            }

            return word;
        }
    }
}
