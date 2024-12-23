﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MongoDB.Entities;

/// <summary>
/// A helper class to build a JSON command from a string with tag replacement
/// </summary>
/// <typeparam name="T">Any type that implements IEntity</typeparam>
public class Template<T> : Template<T, T> where T : IEntity
{
    /// <summary>
    /// Initializes a template with a tagged input string.
    /// </summary>
    /// <param name="template">The template string with tags for targeting replacements such as "&lt;Author.Name&gt;"</param>
    public Template(string template) : base(template) { }
}

/// <summary>
/// A helper class to build a JSON command from a string with tag replacement
/// </summary>
/// <typeparam name="TInput">The input type</typeparam>
/// <typeparam name="TResult">The output type</typeparam>
public class Template<TInput, TResult> : Template where TInput : IEntity
{
    /// <summary>
    /// Initializes a template with a tagged input string.
    /// </summary>
    /// <param name="template">The template string with tags for targeting replacements such as "&lt;Author.Name&gt;"</param>
    public Template(string template) : base(template) { }

    /// <summary>
    /// Gets the collection name of a given entity type and replaces matching tags in the template such as "&lt;EntityName&gt;"
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to get the collection name of</typeparam>
    public new Template<TInput, TResult> Collection<TEntity>() where TEntity : IEntity
        => (Template<TInput, TResult>)base.Collection<TEntity>();

    /// <summary>
    /// Turns the given member expression (of input type) into a property name like "SomeProp" and replaces matching tags in the template such as "&lt;SomeProp
    /// &gt;"
    /// </summary>
    /// <param name="expression">x => x.RootProp.SomeProp</param>
    public Template<TInput, TResult> Property(Expression<Func<TInput, object?>> expression)
        => (Template<TInput, TResult>)base.Property(expression);

    /// <summary>
    /// Turns the given member expression (of output type) into a property name like "SomeProp" and replaces matching tags in the template such as "&lt;SomeProp
    /// &gt;"
    /// </summary>
    /// <param name="expression">x => x.RootProp.SomeProp</param>
    public Template<TInput, TResult> PropertyOfResult(Expression<Func<TResult, object?>> expression)
        => (Template<TInput, TResult>)base.Property(expression);

    /// <summary>
    /// Turns the given member expression (of any type) into a property name like "SomeProp" and replaces matching tags in the template such as "&lt;SomeProp&gt;"
    /// </summary>
    /// <param name="expression">x => x.RootProp.SomeProp</param>
    public new Template<TInput, TResult> Property<TOther>(Expression<Func<TOther, object?>> expression)
        => (Template<TInput, TResult>)base.Property(expression);

    /// <summary>
    /// Turns the property paths in the given `new` expression (of input type) into names like "PropX &amp; PropY" and replaces matching tags in the template.
    /// </summary>
    /// <param name="expression">x => new { x.Prop1.PropX, x.Prop2.PropY }</param>
    public Template<TInput, TResult> Properties(Expression<Func<TInput, object?>> expression)
        => (Template<TInput, TResult>)base.Properties(expression);

    /// <summary>
    /// Turns the property paths in the given `new` expression (of output type) into names like "PropX &amp; PropY" and replaces matching tags in the template.
    /// </summary>
    /// <param name="expression">x => new { x.Prop1.PropX, x.Prop2.PropY }</param>
    public Template<TInput, TResult> PropertiesOfResult(Expression<Func<TResult, object?>> expression)
        => (Template<TInput, TResult>)base.Properties(expression);

    /// <summary>
    /// Turns the property paths in the given `new` expression (of any type) into paths like "PropX &amp; PropY" and replaces matching tags in the template.
    /// </summary>
    /// <param name="expression">x => new { x.Prop1.PropX, x.Prop2.PropY }</param>
    public new Template<TInput, TResult> Properties<TOther>(Expression<Func<TOther, object?>> expression)
        => (Template<TInput, TResult>)base.Properties(expression);

    /// <summary>
    /// Turns the given expression (of input type) to a dotted path like "SomeList.SomeProp" and replaces matching tags in the template such as "&lt;
    /// SomeList.SomeProp&gt;"
    /// </summary>
    /// <param name="expression">x => x.SomeList[0].SomeProp</param>
    public Template<TInput, TResult> Path(Expression<Func<TInput, object?>> expression)
        => (Template<TInput, TResult>)base.Path(expression);

    /// <summary>
    /// Turns the given expression (of output type) to a dotted path like "SomeList.SomeProp" and replaces matching tags in the template such as "&lt;
    /// SomeList.SomeProp&gt;"
    /// </summary>
    /// <param name="expression">x => x.SomeList[0].SomeProp</param>
    public Template<TInput, TResult> PathOfResult(Expression<Func<TResult, object?>> expression)
        => (Template<TInput, TResult>)base.Path(expression);

    /// <summary>
    /// Turns the given expression (of any type) to a dotted path like "SomeList.SomeProp" and replaces matching tags in the template such as "&lt;
    /// SomeList.SomeProp&gt;"
    /// </summary>
    /// <param name="expression">x => x.SomeList[0].SomeProp</param>
    public new Template<TInput, TResult> Path<TOther>(Expression<Func<TOther, object?>> expression)
        => (Template<TInput, TResult>)base.Path(expression);

    /// <summary>
    /// Turns the property paths in the given `new` expression (of input type) into paths like "Prop1.Child1 &amp; Prop2.Child2" and replaces matching tags in the
    /// template.
    /// </summary>
    /// <param name="expression">x => new { x.Prop1.Child1, x.Prop2.Child2 }</param>
    public Template<TInput, TResult> Paths(Expression<Func<TInput, object?>> expression)
        => (Template<TInput, TResult>)base.Paths(expression);

    /// <summary>
    /// Turns the property paths in the given `new` expression (of output type) into paths like "Prop1.Child1 &amp; Prop2.Child2" and replaces matching tags in
    /// the template.
    /// </summary>
    /// <param name="expression">x => new { x.Prop1.Child1, x.Prop2.Child2 }</param>
    public Template<TInput, TResult> PathsOfResult(Expression<Func<TResult, object?>> expression)
        => (Template<TInput, TResult>)base.Paths(expression);

    /// <summary>
    /// Turns the property paths in the given `new` expression (of any type) into paths like "Prop1.Child1 &amp; Prop2.Child2" and replaces matching tags in the
    /// template.
    /// </summary>
    /// <param name="expression">x => new { x.Prop1.Child1, x.Prop2.Child2 }</param>
    public new Template<TInput, TResult> Paths<TOther>(Expression<Func<TOther, object?>> expression)
        => (Template<TInput, TResult>)base.Paths(expression);

    /// <summary>
    /// Turns the given expression (of input type) to a positional filtered path like "Authors.$[a].Name" and replaces matching tags in the template such as "&lt;
    /// Authors.$[a].Name&gt;"
    /// <para>TIP: Index positions start from [0] which is converted to $[a] and so on.</para>
    /// </summary>
    /// <param name="expression">x => x.SomeList[0].SomeProp</param>
    public Template<TInput, TResult> PosFiltered(Expression<Func<TInput, object?>> expression)
        => (Template<TInput, TResult>)base.PosFiltered(expression);

    /// <summary>
    /// Turns the given expression (of output type) to a positional filtered path like "Authors.$[a].Name" and replaces matching tags in the template such as "
    /// &lt;
    /// Authors.$[a].Name&gt;"
    /// <para>TIP: Index positions start from [0] which is converted to $[a] and so on.</para>
    /// </summary>
    /// <param name="expression">x => x.SomeList[0].SomeProp</param>
    public Template<TInput, TResult> PosFilteredOfResult(Expression<Func<TResult, object?>> expression)
        => (Template<TInput, TResult>)base.PosFiltered(expression);

    /// <summary>
    /// Turns the given expression (of any type) to a positional filtered path like "Authors.$[a].Name" and replaces matching tags in the template such as "&lt;
    /// Authors.$[a].Name
    /// &gt;"
    /// <para>TIP: Index positions start from [0] which is converted to $[a] and so on.</para>
    /// </summary>
    /// <param name="expression">x => x.SomeList[0].SomeProp</param>
    public new Template<TInput, TResult> PosFiltered<TOther>(Expression<Func<TOther, object?>> expression)
        => (Template<TInput, TResult>)base.PosFiltered(expression);

    /// <summary>
    /// Turns the given expression (of input type) to a path with the all positional operator like "Authors.$[].Name" and replaces matching tags in the template
    /// such as "&lt;
    /// Authors.$[].Name&gt;"
    /// </summary>
    /// <param name="expression">x => x.SomeList[0].SomeProp</param>
    public Template<TInput, TResult> PosAll(Expression<Func<TInput, object?>> expression)
        => (Template<TInput, TResult>)base.PosAll(expression);

    /// <summary>
    /// Turns the given expression (of output type) to a path with the all positional operator like "Authors.$[].Name" and replaces matching tags in the template
    /// such as "&lt;
    /// Authors.$[].Name&gt;"
    /// </summary>
    /// <param name="expression">x => x.SomeList[0].SomeProp</param>
    public Template<TInput, TResult> PosAllOfResult(Expression<Func<TResult, object?>> expression)
        => (Template<TInput, TResult>)base.PosAll(expression);

    /// <summary>
    /// Turns the given expression (of any type) to a path with the all positional operator like "Authors.$[].Name" and replaces matching tags in the template
    /// such as "&lt;
    /// Authors.$[].Name&gt;"
    /// </summary>
    /// <param name="expression">x => x.SomeList[0].SomeProp</param>
    public new Template<TInput, TResult> PosAll<TOther>(Expression<Func<TOther, object?>> expression)
        => (Template<TInput, TResult>)base.PosAll(expression);

    /// <summary>
    /// Turns the given expression (of input type) to a path with the first positional operator like "Authors.$.Name" and replaces matching tags in the template
    /// such as "&lt;
    /// Authors.$.Name&gt;"
    /// </summary>
    /// <param name="expression">x => x.SomeList[0].SomeProp</param>
    public Template<TInput, TResult> PosFirst(Expression<Func<TInput, object?>> expression)
        => (Template<TInput, TResult>)base.PosFirst(expression);

    /// <summary>
    /// Turns the given expression (of output type) to a path with the first positional operator like "Authors.$.Name" and replaces matching tags in the template
    /// such as "&lt;
    /// Authors.$.Name&gt;"
    /// </summary>
    /// <param name="expression">x => x.SomeList[0].SomeProp</param>
    public Template<TInput, TResult> PosFirstOfResult(Expression<Func<TResult, object?>> expression)
        => (Template<TInput, TResult>)base.PosFirst(expression);

    /// <summary>
    /// Turns the given expression (of any type) to a path with the first positional operator like "Authors.$.Name" and replaces matching tags in the template
    /// such as "&lt;
    /// Authors.$.Name&gt;"
    /// </summary>
    /// <param name="expression">x => x.SomeList[0].SomeProp</param>
    public new Template<TInput, TResult> PosFirst<TOther>(Expression<Func<TOther, object?>> expression)
        => (Template<TInput, TResult>)base.PosFirst(expression);

    /// <summary>
    /// Turns the given expression (of input type) to a path without any filtered positional identifier prepended to it like "Name" and replaces matching tags in
    /// the template
    /// such as "&lt;Name&gt;"
    /// </summary>
    /// <param name="expression">x => x.SomeProp</param>
    public Template<TInput, TResult> Elements(Expression<Func<TInput, object?>> expression)
        => (Template<TInput, TResult>)base.Elements(expression);

    /// <summary>
    /// Turns the given expression (of output type) to a path without any filtered positional identifier prepended to it like "Name" and replaces matching tags in
    /// the template
    /// such as "&lt;Name&gt;"
    /// </summary>
    /// <param name="expression">x => x.SomeProp</param>
    public Template<TInput, TResult> ElementsOfResult(Expression<Func<TResult, object?>> expression)
        => (Template<TInput, TResult>)base.Elements(expression);

    /// <summary>
    /// Turns the given expression (of any type) to a path without any filtered positional identifier prepended to it like "Name" and replaces matching tags in
    /// the template such
    /// as "&lt;Name&gt;"
    /// </summary>
    /// <param name="expression">x => x.SomeProp</param>
    public new Template<TInput, TResult> Elements<TOther>(Expression<Func<TOther, object?>> expression)
        => (Template<TInput, TResult>)base.Elements(expression);

    /// <summary>
    /// Turns the given index and expression (of input type) to a path with the filtered positional identifier prepended to the property path like "a.Name" and
    /// replaces matching
    /// tags in the template such as "&lt;a.Name&gt;"
    /// </summary>
    /// <param name="index">0=a 1=b 2=c 3=d and so on...</param>
    /// <param name="expression">x => x.SomeProp</param>
    public Template<TInput, TResult> Elements(int index, Expression<Func<TInput, object?>> expression)
        => (Template<TInput, TResult>)base.Elements(index, expression);

    /// <summary>
    /// Turns the given index and expression (of output type) to a path with the filtered positional identifier prepended to the property path like "a.Name" and
    /// replaces
    /// matching tags in the template such as "&lt;a.Name&gt;"
    /// </summary>
    /// <param name="index">0=a 1=b 2=c 3=d and so on...</param>
    /// <param name="expression">x => x.SomeProp</param>
    public Template<TInput, TResult> ElementsOfResult(int index, Expression<Func<TResult, object?>> expression)
        => (Template<TInput, TResult>)base.Elements(index, expression);

    /// <summary>
    /// Turns the given index and expression (of any type) to a path with the filtered positional identifier prepended to the property path like "a.Name" and
    /// replaces matching
    /// tags in the template such as "&lt;a.Name&gt;"
    /// </summary>
    /// <param name="index">0=a 1=b 2=c 3=d and so on...</param>
    /// <param name="expression">x => x.SomeProp</param>
    public new Template<TInput, TResult> Elements<TOther>(int index, Expression<Func<TOther, object?>> expression)
        => (Template<TInput, TResult>)base.Elements(index, expression);

    /// <summary>
    /// Replaces the given tag in the template like "&lt;search_term&gt;" with the supplied value.
    /// </summary>
    /// <param name="tagName">The tag name without the surrounding &lt; and &gt;</param>
    /// <param name="replacementValue">The value to replace with</param>
    public new Template<TInput, TResult> Tag(string tagName, string replacementValue)
        => (Template<TInput, TResult>)base.Tag(tagName, replacementValue);

    /// <summary>
    /// Executes the tag replacement and returns a pipeline definition.
    /// <para>TIP: if all the tags don't match, an exception will be thrown.</para>
    /// </summary>
    public PipelineDefinition<TInput, TResult> ToPipeline()
        => ToPipeline<TInput, TResult>();

    /// <summary>
    /// Executes the tag replacement and returns array filter definitions.
    /// <para>TIP: if all the tags don't match, an exception will be thrown.</para>
    /// </summary>
    public IEnumerable<ArrayFilterDefinition> ToArrayFilters()
        => ToArrayFilters<TInput>();
}

/// <summary>
/// A helper class to build a JSON command from a string with tag replacement
/// </summary>
public class Template
{
    static readonly Regex _regex = new("<.*?>", RegexOptions.Compiled);
    static readonly ConcurrentDictionary<int, string> _cache = new();

    internal readonly StringBuilder Builder;

    bool _cacheHit,
         _hasAppendedStages;

    readonly int _cacheKey;
    readonly HashSet<string> _goalTags = [];
    readonly HashSet<string> _missingTags = [];
    readonly HashSet<string> _replacedTags = [];
    readonly Dictionary<string, string> _valueTags = new();

    /// <summary>
    /// Initialize a command builder with the supplied template string.
    /// </summary>
    /// <param name="template">The template string with tags for targeting replacements such as "&lt;Author.Name&gt;"</param>
    public Template(string template)
    {
        if (string.IsNullOrWhiteSpace(template))
            throw new ArgumentException("Unable to instantiate a template from an empty string!");

        _cacheKey = template.GetHashCode();

        _cache.TryGetValue(_cacheKey, out var cachedTemplate);

        if (cachedTemplate != null)
        {
            _cacheHit = true;
            Builder = new(cachedTemplate);
        }
        else
            Builder = new(template.Trim());

        if (Builder[0] == '[' && Builder[1] == ']')
            return; //not an empty array

        foreach (var match in _regex.Matches(_cacheHit ? cachedTemplate! : template).Cast<Match>())
            _goalTags.Add(match.Value);

        if (!_cacheHit && _goalTags.Count == 0)
            throw new ArgumentException("No replacement tags such as '<tagname>' were found in the supplied template string");
    }

    Template ReplacePath(string path)
    {
        var tag = $"<{path}>";

        if (!_goalTags.Contains(tag))
            _missingTags.Add(tag);
        else
        {
            Builder.Replace(tag, path);
            _replacedTags.Add(tag);
        }

        return this;
    }

    /// <summary>
    /// Appends a pipeline stage json string to the current pipeline.
    /// This method can only be used if the template was initialized with an array of pipeline stages.
    /// If this is going to be the first stage of your pipeline, you must instantiate the template with an empty array string <c>new Template("[]")</c>
    /// <para>WARNING: Appending stages prevents this template from being cached!!!</para>
    /// </summary>
    /// <param name="pipelineStageString">The pipeline stage json string to append</param>
    public void AppendStage(string pipelineStageString)
    {
        _hasAppendedStages = true;

        var pipelineEndPos = 0;
        var lastCharPos = Builder.Length - 1;

        if (Builder[lastCharPos] == ']')
            pipelineEndPos = lastCharPos;

        if (pipelineEndPos == 0)
        {
            throw new InvalidOperationException(
                "Stages can only be appended to a template initialized with an array of stages. " +
                "Initialize the template with an empty array \"[]\" if this is the first stage.");
        }

        if (!pipelineStageString.StartsWith("{") && !pipelineStageString.EndsWith("}"))
            throw new ArgumentException("A pipeline stage string must begin with a { and end with a }");

        foreach (var match in _regex.Matches(pipelineStageString).Cast<Match>())
            _goalTags.Add(match.Value);

        if (Builder[0] == '[' && Builder[1] == ']') //empty array
            Builder.Remove(lastCharPos, 1);
        else
            Builder[lastCharPos] = ',';

        Builder
            .Append(pipelineStageString)
            .Append(']');
    }

    /// <summary>
    /// Gets the collection name of a given entity type and replaces matching tags in the template such as "&lt;EntityName&gt;"
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to get the collection name of</typeparam>
    public Template Collection<TEntity>() where TEntity : IEntity
        => _cacheHit ? this : ReplacePath(Prop.Collection<TEntity>());

    /// <summary>
    /// Turns the given member expression into a property name like "SomeProp" and replaces matching tags in the template such as "&lt;SomeProp&gt;"
    /// </summary>
    /// <param name="expression">x => x.RootProp.SomeProp</param>
    public Template Property<T>(Expression<Func<T, object?>> expression)
        => _cacheHit ? this : ReplacePath(Prop.Property(expression));

    /// <summary>
    /// Turns the property paths in the given `new` expression into property names like "PropX &amp; PropY" and replaces matching tags in the template.
    /// </summary>
    /// <param name="expression">x => new { x.Prop1.PropX, x.Prop2.PropY }</param>
    public Template Properties<T>(Expression<Func<T, object?>> expression)
    {
        if (_cacheHit)
            return this;

        var props =
            (expression.Body as NewExpression)?
            .Arguments
            .Cast<MemberExpression>()
            .Select(e => e.Member.Name);

        if (props?.Any() != true)
            throw new ArgumentException("Unable to parse any property names from the supplied `new` expression!");

        foreach (var p in props)
            ReplacePath(p);

        return this;
    }

    /// <summary>
    /// Turns the given expression into a dotted path like "SomeList.SomeProp" and replaces matching tags in the template such as "&lt;SomeList.SomeProp&gt;"
    /// </summary>
    /// <param name="expression">x => x.SomeList[0].SomeProp</param>
    public Template Path<T>(Expression<Func<T, object?>> expression)
        => _cacheHit ? this : ReplacePath(Prop.Path(expression));

    /// <summary>
    /// Turns the property paths in the given `new` expression into paths like "Prop1.Child1 &amp; Prop2.Child2" and replaces matching tags in the template.
    /// </summary>
    /// <param name="expression">x => new { x.Prop1.Child1, x.Prop2.Child2 }</param>
    public Template Paths<T>(Expression<Func<T, object?>> expression)
    {
        if (_cacheHit)
            return this;

        var paths =
            (expression.Body as NewExpression)?
            .Arguments
            .Select(a => Prop.GetPath(a.ToString()));

        if (paths?.Any() != true)
            throw new ArgumentException("Unable to parse any property paths from the supplied `new` expression!");

        foreach (var p in paths)
            ReplacePath(p);

        return this;
    }

    /// <summary>
    /// Turns the given expression into a positional filtered path like "Authors.$[a].Name" and replaces matching tags in the template such as "&lt;
    /// Authors.$[a].Name&gt;"
    /// <para>TIP: Index positions start from [0] which is converted to $[a] and so on.</para>
    /// </summary>
    /// <param name="expression">x => x.SomeList[0].SomeProp</param>
    public Template PosFiltered<T>(Expression<Func<T, object?>> expression)
        => _cacheHit ? this : ReplacePath(Prop.PosFiltered(expression));

    /// <summary>
    /// Turns the given expression into a path with the all positional operator like "Authors.$[].Name" and replaces matching tags in the template such as "&lt;
    /// Authors.$[].Name
    /// &gt;"
    /// </summary>
    /// <param name="expression">x => x.SomeList[0].SomeProp</param>
    public Template PosAll<T>(Expression<Func<T, object?>> expression)
        => _cacheHit ? this : ReplacePath(Prop.PosAll(expression));

    /// <summary>
    /// Turns the given expression into a path with the first positional operator like "Authors.$.Name" and replaces matching tags in the template such as "&lt;
    /// Authors.$.Name
    /// &gt;"
    /// </summary>
    /// <param name="expression">x => x.SomeList[0].SomeProp</param>
    public Template PosFirst<T>(Expression<Func<T, object?>> expression)
        => _cacheHit ? this : ReplacePath(Prop.PosFirst(expression));

    /// <summary>
    /// Turns the given expression into a path without any filtered positional identifier prepended to it like "Name" and replaces matching tags in the template
    /// such as "&lt;
    /// Name&gt;"
    /// </summary>
    /// <param name="expression">x => x.SomeProp</param>
    public Template Elements<T>(Expression<Func<T, object?>> expression)
        => _cacheHit ? this : ReplacePath(Prop.Elements(expression));

    /// <summary>
    /// Turns the given index and expression into a path with the filtered positional identifier prepended to the property path like "a.Name" and replaces
    /// matching tags in the
    /// template such as "&lt;a.Name&gt;"
    /// </summary>
    /// <param name="index">0=a 1=b 2=c 3=d and so on...</param>
    /// <param name="expression">x => x.SomeProp</param>
    public Template Elements<T>(int index, Expression<Func<T, object?>> expression)
        => _cacheHit ? this : ReplacePath(Prop.Elements(index, expression));

    /// <summary>
    /// Replaces the given tag in the template like "&lt;search_term&gt;" with the supplied value.
    /// </summary>
    /// <param name="tagName">The tag name without the surrounding &lt; and &gt;</param>
    /// <param name="replacementValue">The value to replace with</param>
    public Template Tag(string tagName, string replacementValue)
    {
        var tag = $"<{tagName}>";

        if (!_goalTags.Contains(tag))
            _missingTags.Add(tag);
        else
            _valueTags[tag] = replacementValue;

        return this;
    }

    /// <summary>
    /// Executes the tag replacement and returns a string.
    /// <para>TIP: if all the tags don't match, an exception will be thrown.</para>
    /// </summary>
    public string RenderToString()
    {
        if (!_cacheHit && !_hasAppendedStages)
        {
            _cache[_cacheKey] = Builder.ToString();
            _cacheHit = true; //in case this method is called multiple times
        }

        foreach (var t in _valueTags.ToArray())
        {
            Builder.Replace(t.Key, t.Value);
            _replacedTags.Add(t.Key);
        }

        if (_missingTags.Count > 0)
            throw new InvalidOperationException($"The following tags were missing from the template: [{string.Join(",", _missingTags)}]");

        var unReplacedTags = _goalTags.Except(_replacedTags);

        return unReplacedTags.Any()
                   ? throw new InvalidOperationException($"Replacements for the following tags are required: [{string.Join(",", unReplacedTags)}]")
                   : Builder.ToString();
    }

    /// <summary>
    /// Executes the tag replacement and returns the pipeline stages as an array of BsonDocuments.
    /// <para>TIP: if all the tags don't match, an exception will be thrown.</para>
    /// </summary>
    public IEnumerable<BsonDocument> ToStages()
    {
        return BsonSerializer
               .Deserialize<BsonArray>(RenderToString())
               .Select(v => v.AsBsonDocument);
    }

    /// <summary>
    /// Executes the tag replacement and returns a pipeline definition.
    /// <para>TIP: if all the tags don't match, an exception will be thrown.</para>
    /// </summary>
    /// <typeparam name="TInput">The input type</typeparam>
    /// <typeparam name="TOutput">The output type</typeparam>
    public PipelineDefinition<TInput, TOutput> ToPipeline<TInput, TOutput>()
        => ToStages().ToArray();

    /// <summary>
    /// Executes the tag replacement and returns array filter definitions.
    /// <para>TIP: if all the tags don't match, an exception will be thrown.</para>
    /// </summary>
    public IEnumerable<ArrayFilterDefinition> ToArrayFilters<T>()
    {
        return BsonSerializer
               .Deserialize<BsonArray>(RenderToString())
               .Select(v => (ArrayFilterDefinition<T>)v.AsBsonDocument);
    }
}