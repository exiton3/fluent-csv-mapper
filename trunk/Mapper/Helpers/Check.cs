using System;

namespace Mapper.Helpers
{
    public static class Check
    {
        public static void NotNull<TArg>(TArg arg, string message) where TArg:class
        {
            if (arg == null)
            {
                throw new ArgumentNullException(message);
            }
        }
    }
}