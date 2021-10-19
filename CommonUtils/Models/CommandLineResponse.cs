using System;

namespace CommonUtils.Models
{
    public class CommandLineResponse
    {
        public string StandartOutput { get; set; }

        public Exception Exception { get; set; }

        public CommandLineResponseStatus ResponseStatus { get; set; }
    }
}
