using System;

namespace ThreeDent.DevelopmentTools.Option
{
    [System.Serializable]
    public class OptionNullValueProvidedException : System.Exception
    {
        public OptionNullValueProvidedException() : base("There is another type of Option provided, what should not happen. Do not inherit Option class.") { }
        public OptionNullValueProvidedException(string message) : base(message) { }
        public OptionNullValueProvidedException(string message, System.Exception inner) : base(message, inner) { }
        protected OptionNullValueProvidedException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [System.Serializable]
    public class OptionThirdVariantException : System.Exception
    {
        public OptionThirdVariantException() { }
        public OptionThirdVariantException(string message) : base(message) { }
        public OptionThirdVariantException(string message, System.Exception inner) : base(message, inner) { }
        protected OptionThirdVariantException(
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

        public static Option<T> FromNullable<T>(T value)
        {
            if (value == null)
                return new None<T>();
            else
                return new Some<T>(value);
        }

        public static Option<TOutput> Map<TInput, TOutput>(this Option<TInput> option, Func<TInput, TOutput> mappingFunction)
        {
            return option switch
            {
                Some<TInput> x => Some(mappingFunction(x.Value)),
                None<TInput> x => None<TOutput>(),
                _ => throw new OptionThirdVariantException(),
            };
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