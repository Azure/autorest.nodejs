﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// 

namespace AutoRest.NodeJS.DSL
{
    /// <summary>
    /// A position within a JSBuilder.
    /// </summary>
    public class JSPosition
    {
        private readonly JSPosition previousPosition;

        public JSPosition(JSPosition previousPosition, int charactersAfterPreviousPosition)
        {
            this.previousPosition = previousPosition;
            this.CharactersAfterPreviousPosition = charactersAfterPreviousPosition;
        }

        /// <summary>
        /// The number of characters between this position and the position before this one.
        /// </summary>
        public int CharactersAfterPreviousPosition { get; set; }

        /// <summary>
        /// Get the character index within the JSBuilder that this position points to.
        /// </summary>
        /// <returns></returns>
        public int GetIndexInBuilder()
        {
            return (previousPosition == null ? 0 : previousPosition.GetIndexInBuilder()) + CharactersAfterPreviousPosition;
        }
    }
}
