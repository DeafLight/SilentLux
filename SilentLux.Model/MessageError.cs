using LanguageExt;
using System.Diagnostics;
using static LanguageExt.Prelude;

namespace SilentLux.Model
{
    public class MessageError : IError
    {
        public MessageError(string message)
        {
            Message = message;
        }

        public Option<IError> InnerError => None;

        public Option<string> Message { get; }

        public Option<StackTrace> StackTrace => None;
    }
}
