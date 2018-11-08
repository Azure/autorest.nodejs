// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
//

using AutoRest.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoRest.NodeJS.DSL
{
    /// <summary>
    /// A StringBuilder that has helper methods for building TypeScript code.
    /// </summary>
    public class JSBuilder
    {
        private const string newLine = "\n";
        private const string singleIndent = "  ";
        private const int defaultCommentWordWrapWidth = 100;

        private readonly int commentWordWrapWidth;
        private readonly StringBuilder contents = new StringBuilder();
        private readonly StringBuilder linePrefix = new StringBuilder();
        private readonly List<JSPosition> positions = new List<JSPosition>();
        
        public JSBuilder(int commentWordWrapWidth = defaultCommentWordWrapWidth)
        {
            this.commentWordWrapWidth = commentWordWrapWidth;
        }

        internal bool WriteNewLineBeforeNextText { get; private set; }

        /// <summary>
        /// The word wrap width. A null wordWrapWidth indicates that no word wrapping should take place.
        /// </summary>
        public int? WordWrapWidth { get; set; }

        /// <summary>
        /// Create a position object that will track a certain position within the JSBuilder's content.
        /// </summary>
        /// <returns></returns>
        public JSPosition CreatePosition()
        {
            JSPosition position;
            int contentLength = contents.Length;
            if (!positions.Any())
            {
                position = new JSPosition(null, contentLength);
                positions.Add(position);
            }
            else
            {
                position = positions.Last();
                int lastPositionIndexInBuilder = position.GetIndexInBuilder();
                if (lastPositionIndexInBuilder != contentLength)
                {
                    position = new JSPosition(position, contentLength - lastPositionIndexInBuilder);
                    positions.Add(position);
                }
            }
            return position;
        }

        /// <summary>
        /// Get whether or not a new line character has been added to this JSBuilder since the provided character index.
        /// </summary>
        /// <param name="index">The character index to begin the search at.</param>
        /// <returns></returns>
        public bool HasChangedLinesSince(int index)
        {
            bool result = false;
            for (int i = index; i < contents.Length; ++i)
            {
                if (contents[i] == '\n')
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void Insert(int index, string text)
        {
            if (positions.Any())
            {
                int positionIndex = 0;
                foreach (JSPosition position in positions)
                {
                    if (index <= positionIndex + position.CharactersAfterPreviousPosition)
                    {
                        position.CharactersAfterPreviousPosition += text.Length;
                        break;
                    }
                    else
                    {
                        positionIndex += position.CharactersAfterPreviousPosition;
                    }
                }
            }
            contents.Insert(index, text);
        }

        public void InsertNewLine(int index)
        {
            Insert(index, newLine + linePrefix);
        }

        public void AddIndentToLinesAfter(int index)
        {
            for (int i = index; i < contents.Length; ++i)
            {
                if (contents[i] == '\n' && i + 1 < contents.Length && contents[i + 1] != '\n')
                {
                    contents.Insert(i + 1, singleIndent);
                }
            }
        }

        /// <summary>
        /// Get the text that has been added to this JSBuilder.
        /// </summary>
        /// <returns>The text that has been added to this JSBuilder.</returns>
        public override string ToString()
        {
            return contents.ToString();
        }

        /// <summary>
        /// Add the provided value to end of the line prefix.
        /// </summary>
        /// <param name="toAdd">The value to add to the line prefix.</param>
        public void AddToPrefix(string toAdd)
        {
            linePrefix.Append(toAdd);
        }

        /// <summary>
        /// Remove the provided value from the end of the line prefix.
        /// </summary>
        /// <param name="toRemove">The value to remove from the end of the line prefix.</param>
        public void RemoveFromPrefix(string toRemove)
        {
            int toRemoveLength = toRemove.Length;
            if (linePrefix.Length <= toRemoveLength)
            {
                linePrefix.Clear();
            }
            else
            {
                linePrefix.Remove(linePrefix.Length - toRemoveLength, toRemoveLength);
            }
        }

        /// <summary>
        /// Invoke the provided action with the provided additional prefix.
        /// </summary>
        /// <param name="toAdd">The additional text to add to the line prefix for the duration of the provided action.</param>
        /// <param name="action">The action to invoke with the provided additional line prefix text.</param>
        public void WithAddedPrefix(string toAdd, Action action)
        {
            AddToPrefix(toAdd);
            try
            {
                action.Invoke();
            }
            finally
            {
                RemoveFromPrefix(toAdd);
            }
        }

        /// <summary>
        /// Add a single indentation for the context of the provided action.
        /// </summary>
        /// <param name="action">The action to invoke with an extra level of indentation.</param>
        public void Indent(Action action)
        {
            IncreaseIndent();
            try
            {
                action.Invoke();
            }
            finally
            {
                DecreaseIndent();
            }
        }

        /// <summary>
        /// Add a new level of indentation to the line prefix.
        /// </summary>
        public void IncreaseIndent()
            => AddToPrefix(singleIndent);

        /// <summary>
        /// Remove a level of indentation from the line prefix.
        /// </summary>
        public void DecreaseIndent()
            => RemoveFromPrefix(singleIndent);

        /// <summary>
        /// Wrap the provided line using the existing wordWrapWidth.
        /// </summary>
        /// <param name="line">The line to wrap.</param>
        /// <param name="addPrefix">Whether or not to add the line prefix to the wrapped lines.</param>
        /// <returns>The wrapped lines.</returns>
        internal IEnumerable<string> WordWrap(string line, bool addPrefix)
        {
            List<string> lines = new List<string>();

            if (!string.IsNullOrEmpty(line))
            {
                if (WordWrapWidth == null)
                {
                    lines.Add(line);
                }
                else
                {
                    // Subtract an extra column from the word wrap width because columns generally are
                    // 1 -based instead of 0-based.
                    int wordWrapIndexMinusLinePrefixLength = WordWrapWidth.Value - (addPrefix ? linePrefix.Length : 0) - 1;

                    IEnumerable<string> wrappedLines = line.WordWrap(wordWrapIndexMinusLinePrefixLength);
                    foreach (string wrappedLine in wrappedLines.SkipLast(1))
                    {
                        lines.Add(wrappedLine + newLine);
                    }

                    string lastWrappedLine = wrappedLines.Last();
                    if (!string.IsNullOrEmpty(lastWrappedLine))
                    {
                        lines.Add(lastWrappedLine);
                    }
                }
            }
            return lines;
        }

        /// <summary>
        /// Add the provided text to this JSBuilder.
        /// </summary>
        /// <param name="text">The text to add.</param>
        public void Text(string text, params object[] formattedArguments)
        {
            if (!string.IsNullOrEmpty(text) && formattedArguments != null && formattedArguments.Length > 0)
            {
                text = string.Format(text, formattedArguments);
            }

            bool addPrefix = WriteNewLineBeforeNextText;

            List<string> lines = new List<string>();

            if (WriteNewLineBeforeNextText)
            {
                WriteNewLineBeforeNextText = false;
                contents.Append(newLine);
            }

            if (string.IsNullOrEmpty(text))
            {
                lines.Add("");
            }
            else
            {
                int lineStartIndex = 0;
                int textLength = text.Length;
                while (lineStartIndex < textLength)
                {
                    int newLineCharacterIndex = text.IndexOf(newLine, lineStartIndex);
                    if (newLineCharacterIndex == -1)
                    {
                        string line = text.Substring(lineStartIndex);
                        IEnumerable<string> wrappedLines = WordWrap(line, addPrefix);
                        lines.AddRange(wrappedLines);
                        lineStartIndex = textLength;
                    }
                    else
                    {
                        int nextLineStartIndex = newLineCharacterIndex + 1;
                        string line = text.Substring(lineStartIndex, nextLineStartIndex - lineStartIndex);
                        IEnumerable<string> wrappedLines = WordWrap(line, addPrefix);
                        lines.AddRange(wrappedLines);
                        lineStartIndex = nextLineStartIndex;
                    }
                }
            }

            string prefix = addPrefix ? linePrefix.ToString() : null;
            foreach (string line in lines)
            {
                if (addPrefix && !string.IsNullOrWhiteSpace(prefix) || (!string.IsNullOrEmpty(prefix) && !string.IsNullOrWhiteSpace(line)))
                {
                    contents.Append(prefix);
                }

                contents.Append(line);
            }
        }

        /// <summary>
        /// Add the provided line of the text to this JSBuilder.
        /// </summary>
        /// <param name="text">The line of text to add to this JSBuilder.</param>
        /// <param name="formattedArguments">Any optional formatted arguments that will be inserted into the text if provided.</param>
        public void Line(string text = "", params object[] formattedArguments)
        {
            Text(text, formattedArguments);
            WriteNewLineBeforeNextText = true;
        }

        public void Class(string className, Action<JSClass> classAction)
        {
            Block($"class {className}", block =>
            {
                classAction?.Invoke(new JSClass(this));
            });
        }

        public static IEnumerable<string> GetCommentLines(IEnumerable<string> lines)
        {
            return lines == null ? null : lines.Where((string line) => !string.IsNullOrWhiteSpace(line));
        }

        /// <summary>
        /// Get whether or not the provided lines has any lines that are not null and not whitespace.
        /// </summary>
        /// <param name="lines">The lines to check.</param>
        /// <returns>Whether or not the provided lines has any lines that are not null and not whitespace.</returns>
        public static bool AnyCommentLines(IEnumerable<string> lines)
        {
            IEnumerable<string> commentLines = GetCommentLines(lines);
            return commentLines != null && lines.Any();
        }



        private void Comment(string commentHeader, IEnumerable<string> lines)
        {
            IEnumerable<string> commentLines = GetCommentLines(lines);
            if (commentLines != null && commentLines.Any())
            {
                if (commentLines.Count() == 1)
                {
                    Line($"{commentHeader} {commentLines.Single()} */");
                }
                else
                {
                    Line(commentHeader);
                    WithAddedPrefix(" * ", () =>
                    {
                        int? previousWordWrapWidth = WordWrapWidth;
                        WordWrapWidth = commentWordWrapWidth;
                        try
                        {
                            // We're specifically using lines here instead of commentLines to preserve empty lines in 
                            foreach (string line in lines)
                            {
                                if (line != null)
                                {
                                    Line(line);
                                }
                            }
                        }
                        finally
                        {
                            WordWrapWidth = previousWordWrapWidth;
                        }
                    });
                    Line(" */");
                }
            }
        }

        /// <summary>
        /// Add a /* */ comment to this JSBuilder.
        /// </summary>
        /// <param name="line">The line to add.
        public void BlockComment(string line)
        {
            if (!string.IsNullOrEmpty(line))
            {
                if (line.Contains('\n'))
                {
                    BlockComment(line.Split('\n'));
                }
                else
                {
                    Line($"/* {line} */");
                }
            }
        }

        /// <summary>
        /// Add a /* */ comment to this JSBuilder. If no non-null and non-empty lines are provided, then nothing will be added.
        /// </summary>
        /// <param name="lines">The lines to add. Null lines will be ignored.</param>
        public void BlockComment(params string[] lines)
        {
            Comment("/*", lines);
        }

        public void DocumentationComment(Action<JSDocumentationComment> commentAction)
        {
            if (commentAction != null)
            {
                using (JSDocumentationComment comment = new JSDocumentationComment(this, commentWordWrapWidth))
                {
                    commentAction.Invoke(comment);
                }
            }
        }

        /// <summary>
        /// Add a // comment to this JSBuilder.
        /// </summary>
        /// <param name="line"></param>
        public void LineComment(string line)
        {
            Line($"// {line}");
        }

        /// <summary>
        /// Add a /** */ comment to this JSBuilder. If no non-null and non-empty lines are provided, then nothing will be added.
        /// </summary>
        /// <param name="lines">The lines to add. Null lines will be ignored.</param>
        public void DocumentationComment(params string[] lines)
        {
            Comment("/**", lines);
        }

        /// <summary>
        /// Add a /** */ comment to this JSBuilder. If no non-null and non-empty lines are provided, then nothing will be added.
        /// </summary>
        /// <param name="lines">The lines to add. Null lines will be ignored.</param>
        public void DocumentationComment(IEnumerable<string> lines)
        {
            Comment("/**", lines);
        }

        /// <summary>
        /// Add a JSON array to this JSBuilder that uses the provided action to add the array's elements.
        /// </summary>
        /// <param name="action">The action that will be invoked to add the array's elements.</param>
        public void Array(Action<JSArray> action = null)
        {
            Text("[");
            Indent(() =>
            {
                using (JSArray tsArray = new JSArray(this))
                {
                    action?.Invoke(tsArray);
                }
            });
            Text("]");
        }

        /// <summary>
        /// Add a JSON object to this JSBuilder that uses the provided action to add the object's properties.
        /// </summary>
        /// <param name="action">The action that will be invoked to add the object's properties.</param>
        public void Object(Action<JSObject> action = null)
        {
            Text($"{{");
            Indent(() =>
            {
                using (JSObject tsObject = new JSObject(this))
                {
                    action?.Invoke(tsObject);
                }
            });
            Text($"}}");
        }

        /// <summary>
        /// Surround the provided text with double-quotes and add it to this JSBuilder.
        /// </summary>
        /// <param name="text">The text to double-quote and add to this JSBuilder.</param>
        public void QuotedString(string text)
        {
            Text($"\"{text}\"");
        }

        /// <summary>
        /// Add the provided boolean value to this JSBuilder.
        /// </summary>
        /// <param name="value"></param>
        public void Boolean(bool value)
        {
            Text(value ? "true" : "false");
        }

        /// <summary>
        /// Add a null value to this JSBuilder.
        /// </summary>
        public void Null()
        {
            Text("null");
        }

        /// <summary>
        /// Add an undefined value to this JSBuilder.
        /// </summary>
        public void Undefined()
        {
            Text("undefined");
        }

        public void Lambda(string paramName, Action<JSBlock> lambdaBodyAction)
        {
            Line($"{paramName} => {{");
            Indent(() =>
            {
                lambdaBodyAction.Invoke(new JSBlock(this));
            });
            Text($"}}");
        }

        /// <summary>
        /// Invoke the provided action in order to produce a value in this JSBuilder.
        /// </summary>
        /// <param name="valueAction">The action to invoke.</param>
        public void Value(Action<JSValue> valueAction)
        {
            valueAction?.Invoke(new JSValue(this));
        }

        /// <summary>
        /// Add a function call with the provided functionName to this JSBuilder. The provided
        /// action will be used to create the arguments for the function call.
        /// </summary>
        /// <param name="functionName">The name of the function to invoke.</param>
        /// <param name="argumentListAction">The action to invoke to populate the arguments of the function.</param>
        public void FunctionCall(string functionName, Action<JSArgumentList> argumentListAction)
        {
            Text($"{functionName}(");
            using (JSArgumentList argumentList = new JSArgumentList(this))
            {
                argumentListAction.Invoke(argumentList);
            }
            Text(")");
        }

        public void Method(string methodName, string parameterList, Action<JSBlock> methodBodyAction)
        {
            Block($"{methodName}({parameterList})", methodBodyAction);
        }

        private void Block(string beforeBlock, Action<JSBlock> blockAction)
        {
            Line($"{beforeBlock} {{");
            Indent(() =>
            {
                using (JSBlock block = new JSBlock(this))
                {
                    blockAction.Invoke(block);
                }
            });
            WriteNewLineBeforeNextText = true;
            Text($"}}");
            WriteNewLineBeforeNextText = true;
        }

        public JSIfBlock If(string condition, Action<JSBlock> bodyAction)
        {
            Block($"if ({condition})", bodyAction);
            return new JSIfBlock(this);
        }

        public JSIfBlock ElseIf(string condition, Action<JSBlock> bodyAction)
        {
            WriteNewLineBeforeNextText = false;
            Block($" else if ({condition})", bodyAction);
            return new JSIfBlock(this);
        }

        public void Else(Action<JSBlock> bodyAction)
        {
            WriteNewLineBeforeNextText = false;
            Block($" else", bodyAction);
        }

        public JSTryBlock Try(Action<JSBlock> tryAction)
        {
            Block($"try", tryAction);
            return new JSTryBlock(this);
        }

        public void Catch(string errorName, Action<JSBlock> catchAction)
        {
            WriteNewLineBeforeNextText = false;
            Block($" catch ({errorName})", catchAction);
        }

        public void Return(string result)
        {
            Return(value => value.Text(result));
        }

        public void Return(Action<JSValue> returnValueAction)
        {
            Text("return ");
            Value(returnValueAction);
            Line(";");
        }

        public void Throw(string valueToThrow)
        {
            Line($"throw {valueToThrow};");
        }

        public void ConstQuotedStringVariable(string variableName, string text)
        {
            ConstVariable(variableName, $"\"{text}\"");
        }

        public void ConstVariable(string variableName, string variableValue)
        {
            Line($"const {variableName} = {variableValue};");
        }

        public void ConstObjectVariable(string variableName, Action<JSObject> valueAction)
        {
            Text($"const {variableName} = ");
            Object(valueAction);
            Line($";");
        }

        public void ConstObjectVariable(string variableName, string value)
        {
            Line($"const {variableName} = {value};");
        }

        public void Property(string name, string accessModifier = "")
        {
            string modifier = string.IsNullOrEmpty(accessModifier) ? "" : $"{accessModifier} ";
            Line($"{modifier}{name};");
        }
    }
}