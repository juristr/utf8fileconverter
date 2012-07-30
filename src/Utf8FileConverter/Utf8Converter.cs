using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utf8FileConverter
{
    class Utf8Converter
    {
        private string sourceDirectory;
        private bool withoutBom = true;

        public Utf8Converter(string sourceDirectory, bool withoutBom = true)
        {
            this.sourceDirectory = sourceDirectory;
            this.withoutBom = withoutBom;
        }
    }
}
