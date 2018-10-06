using LanguageExt;
using System.Diagnostics;

namespace SilentLux.Model
{
    public interface IError
    {
        Option<IError> InnerError { get; }
        Option<string> Message { get; }
        Option<StackTrace> StackTrace { get; }
    }
}
