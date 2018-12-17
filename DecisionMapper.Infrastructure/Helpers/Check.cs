using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionMapper.Infrastructure.Helpers
{
    public static class Check
    {
        public static void NotNull(object item, string paramName)
        {
            if (item == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void NotZero(int value, string paramName)
        {
            if (value == 0)
            {
                throw new ArgumentException("Value must be non-zero", paramName);
            }
        }

        public static void NotEmpty<T>(IEnumerable<T> collection, string paramName)
        {
            NotNull(collection, paramName);

            if (!collection.Any())
            {
                throw new ArgumentException("Collection has no elements", paramName);
            }
        }
    }
}
