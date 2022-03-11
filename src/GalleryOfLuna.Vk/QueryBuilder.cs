// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.Text;
using System.Text.Encodings.Web;

// The IEnumerable interface is required for the collection initialization syntax: new QueryBuilder() { { "key", "value" } };
/// <summary>
///     Allows constructing a query string.
/// </summary>
internal class QueryBuilder : IEnumerable<KeyValuePair<string, string>>
{
    private readonly IList<KeyValuePair<string, string>> _params;

    /// <summary>
    ///     Initializes a new instance of <see cref="QueryBuilder" />.
    /// </summary>
    public QueryBuilder()
    {
        _params = new List<KeyValuePair<string, string>>();
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="QueryBuilder" />.
    /// </summary>
    /// <param name="parameters">The parameters to initialize the instance with.</param>
    public QueryBuilder(IEnumerable<KeyValuePair<string, string>> parameters)
    {
        _params = new List<KeyValuePair<string, string>>(parameters);
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="QueryBuilder" />.
    /// </summary>
    /// <param name="parameters">The parameters to initialize the instance with.</param>
    public QueryBuilder(IEnumerable<KeyValuePair<string, IEnumerable<string>>> parameters)
        : this(parameters.SelectMany(kvp => kvp.Value, (kvp, v) => KeyValuePair.Create(kvp.Key, v)))
    {
    }

    public QueryBuilder Add<T>(string key, T value)
        where T : IConvertible
    {
        return Add(key, value.ToString());
    }

    public QueryBuilder Add<T>(string key, IEnumerable<T> values)
        where T : IConvertible
    {
        foreach (var value in values)
            Add(key, value.ToString());

        return this;
    }

    /// <summary>
    ///     Adds a query string token to the instance.
    /// </summary>
    /// <param name="key">The query key.</param>
    /// <param name="values">The sequence of query values.</param>
    public QueryBuilder Add(string key, IEnumerable<string> values)
    {
        foreach (var value in values)
            Add(key, value);

        return this;
    }

    /// <summary>
    ///     Adds a query string token to the instance.
    /// </summary>
    /// <param name="key">The query key.</param>
    /// <param name="value">The query value.</param>
    public QueryBuilder Add(string key, string? value)
    {
        if (value is not null)
            _params.Add(new KeyValuePair<string, string>(key, value));

        return this;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var builder = new StringBuilder();
        var first = true;
        for (var i = 0; i < _params.Count; i++)
        {
            var pair = _params[i];
            builder.Append(first ? '?' : '&');
            first = false;
            builder.Append(UrlEncoder.Default.Encode(pair.Key));
            builder.Append('=');
            builder.Append(UrlEncoder.Default.Encode(pair.Value));
        }

        return builder.ToString();
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return ToString().Equals(obj);
    }

    /// <inheritdoc />
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return _params.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _params.GetEnumerator();
    }
}