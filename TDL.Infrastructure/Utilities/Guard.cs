using System;

namespace TDL.Infrastructure.Utilities
{
    public static class Guard
    {
        /// <summary>
        /// Thorow an exception with a message if the object is null.
        /// </summary>
        /// <typeparam name="TExceptionType"></typeparam>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void ThorwIfNull<TExceptionType>(object obj, string message) where TExceptionType : Exception
        {
            if(obj != null)
            {
                return;
            }

            if(!(Activator.CreateInstance(typeof(TExceptionType), message) is Exception exceptionObj))
            {
                throw new ArgumentException(message);
            }

            throw exceptionObj;
        }

        /// <summary>
        /// Throw an exception with a message if condition is true.
        /// </summary>
        /// <typeparam name="TExceptionType"></typeparam>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowByCondition<TExceptionType>(bool condition, string message) where TExceptionType : Exception
        {
            if(!condition) 
            {
                return;
            }

            if(!(Activator.CreateInstance(typeof(TExceptionType), message) is Exception exceptionObj))
            {
                throw new ArgumentException(message);
            }

            throw exceptionObj;
        }

        /// <summary>
        /// Excute an action if the object is null.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="action"></param>
        public static void DoIfNull(object obj, Action action)
        {
            if(obj != null)
            {
                return;
            }

            action();
        }

        /// <summary>
        /// Excute an action if the condition is true.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="action"></param>
        public static void DoByCondition(bool condition, Action action)
        {
            if(!condition)
            {
                return;
            }

            action();
        }
    }
}
