using System;
using System.Collections.Generic;
using System.Text;

namespace AttributeSniffer.analyzer.model.exception
{
    public class IgnoreElementIdentifierException : Exception
    {
        public IgnoreElementIdentifierException(string message) : base(message)
        {
        }
    }
}
