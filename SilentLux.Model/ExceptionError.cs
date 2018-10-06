using LanguageExt;
using System;
using System.Diagnostics;
using static LanguageExt.Prelude;

namespace SilentLux.Model
{
    public struct ExceptionError : IError
    {
        public ExceptionError(Exception e)
        {
            Exception = e;
            StackTrace = new StackTrace(e);
            InnerError = e.InnerException != null ? (Option<IError>)new ExceptionError(e.InnerException) : None;
        }

        public Exception Exception { get; }
        public Option<IError> InnerError { get; }
        public Option<string> Message => Exception.Message;
        public Option<StackTrace> StackTrace { get; }

        public override bool Equals(object obj)
        {
            if (!(obj is ExceptionError e)) return false;

            return e.Exception == Exception && e.InnerError == InnerError;
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash += 31 * Exception.GetHashCode();
            hash += 31 * InnerError.GetHashCode();

            return hash;
        }

        public static bool operator ==(ExceptionError left, ExceptionError right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ExceptionError left, ExceptionError right)
        {
            return !(left == right);
        }
    }
}
