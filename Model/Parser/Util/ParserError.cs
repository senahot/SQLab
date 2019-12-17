using System;
using System.Collections.Generic;
using System.Text;

namespace SQLab.Model.Parser.Util
{
    public class ParserError
    {
        public ParserError(int elementIndex, ParserErrorType type)
        {
            ElementIndex = elementIndex;
            this.Type = type;
        }

        //Index of the element that has an error
        public int ElementIndex { get; }
        public ParserErrorType Type { get; }
    }
}
