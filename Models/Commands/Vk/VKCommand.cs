using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VkNet;

namespace MultiplatformBot.Models.Commands.Vk
{
    public abstract class VkCommand: Command
    {
        protected VkCommand(string[]? argument, VkConversation vkConversation) : base(argument)
        {
            this.VkConversation = vkConversation;
        }

        
        protected VkConversation VkConversation { get; }
        
        
        public static VkCommand? ParseCommand(string text, VkConversation vkConversation)
        {
            var array = text.Split(' ', '(');
            var commandString = array[0];
            string? argumentsString = null;
            if (array.Length > 1)
            {
                argumentsString = text.Substring(commandString.Length);
            }
            try
            {
                var command = Enum.Parse<CommandType>(array[0].Substring(1), true);
                switch (command)
                {
                    case CommandType.Test:
                        return new Test(null, vkConversation);
                    case CommandType.GetCountOf:
                        if (string.IsNullOrEmpty(argumentsString)) return null;

                        var arguments = ParseArguments(argumentsString);
                        if (arguments == null) return null;

                        return new GetCountOf(arguments.ToArray(), vkConversation);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (ArgumentException exception)
            {
                return null;
            }


            return null;
        }

        private static IEnumerable<string>? ParseArguments(string argumentsString)
        {
            string? inParenthesis = null;
            var inParenthesisList = argumentsString.Split().Where(x => x.StartsWith("(") && x.EndsWith(")"))
                .Select(x=>x.Replace("(", string.Empty).Replace(")", string.Empty)).ToList();
            if (inParenthesisList.Any())
            {
                inParenthesis = inParenthesisList[0];
            }
            if (string.IsNullOrEmpty(inParenthesis)) return null;
            return inParenthesis.Split(',');
        }

        private enum CommandType
        {
            Test,
            GetCountOf
        };
    }
}