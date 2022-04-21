using System;
using System.Collections.Generic;

namespace ToggleComment.Utils
{
    /// <summary>
    /// This class defines extension methods for <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Add key/value pairs to <see cref="IDictionary{TKey, TValue}"/> if the key does not yet exist.
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="dictionary">The <see cref="IDictionary{TKey, TValue}"/> instance of the calling object</param>
        /// <param name="key">Key of the element to be added</param>
        /// <param name="addValueFactory">Function used to generate the value of the key</param>
        /// <returns>The value of the key. The existing value of the key if the key already exists in <paramref name="dictionary"/>, or the new value of the key returned from <paramref name="addValueFactory"/> if the key did not exist. </returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> or <paramref name="addValueFactory"/> is <see langword="null"/>. </exception>. </exception>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> addValueFactory)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }
            if (addValueFactory == null)
            {
                throw new ArgumentNullException(nameof(addValueFactory));
            }

            TValue value;
            if (dictionary.TryGetValue(key, out value) == false)
            {
                value = addValueFactory(key);
                dictionary.Add(key, value);
            }

            return value;
        }
    }
}
