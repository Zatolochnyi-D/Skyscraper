using System;

namespace ThreeDent.DevelopmentTools.Option
{
    [Serializable]
    public class OptionNullValueProvidedException : Exception
    {
        public OptionNullValueProvidedException() { }
        public OptionNullValueProvidedException(string message) : base(message) { }
        public OptionNullValueProvidedException(string message, Exception inner) : base(message, inner) { }
        protected OptionNullValueProvidedException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public static class Option
    {
        public static Option<T> Some<T>(T value)
        {
            if (value != null)
                throw new OptionNullValueProvidedException();
            return new Some<T>(value);
        }

        public static Option<T> None<T>()
        {
            return new None<T>();
        }

        public static Option<T> FromPossibleNull<T>(T value)
        {
            if (value == null)
                return new None<T>();
            else
                return new Some<T>(value);
        }

        public static T DefaultWith<T>(this Option<T> option, T defaultValue)
        {
            if (option is Some<T> x)
                return x.Value;
            else
                return defaultValue;
        }

        public static Option<TOutput> Map<TInput, TOutput>(this Option<TInput> option, Func<TInput, TOutput> mappingFunction)
        {
            if (option is Some<TInput> x)
                return Some(mappingFunction(x.Value));
            else
                return None<TOutput>();
        }
    }

    public class Option<T> { }

    public sealed class Some<T> : Option<T>
    {
        private readonly T value;

        public T Value => value;

        internal Some(T value)
        {
            this.value = value;
        }
    }

    public sealed class None<T> : Option<T> { }
}