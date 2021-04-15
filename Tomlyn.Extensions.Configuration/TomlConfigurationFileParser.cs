using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Tomlyn.Model;

namespace Tomlyn.Extensions.Configuration
{
    internal sealed class TomlConfigurationFileParser
    {
        private TomlConfigurationFileParser() { }

        private readonly IDictionary<string, string> _data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Stack<string> _paths = new();

        public static IDictionary<string, string> Parse(Stream input)
            => new TomlConfigurationFileParser().ParseStream(input);

        private IDictionary<string, string> ParseStream(Stream input)
        {
            using var reader = new StreamReader(input);
            var doc = Toml.Parse(reader.ReadToEnd());
            VisitObject(doc.ToModel());
            return _data;
        }

        private void VisitTable(TomlTable table)
        {
            foreach (var keyValuePair in table.GetTomlEnumerator())
            {
                EnterContext(keyValuePair.Key);
                VisitObject(keyValuePair.Value);
                ExitContext();
            }
        }

        private void VisitTableArray(TomlTableArray tableArray)
        {
            for (var i = 0; i < tableArray.Count; i++) {
                EnterContext(i.ToString());
                VisitTable(tableArray[i]);
                ExitContext();
            }
        }

        private void VisitArray(TomlArray array)
        {
            var i = 0;
            foreach (var tomlObj in array.GetTomlEnumerator())
            {
                EnterContext((i++).ToString());
                VisitObject(tomlObj);
                ExitContext();
            }
        }

        private void VisitObject(TomlObject obj)
        {
            switch (obj)
            {
                case TomlTable tomlTable:
                    VisitTable(tomlTable);
                    break;
                case TomlTableArray tomlTableArray:
                    VisitTableArray(tomlTableArray);
                    break;
                case TomlArray tomlArray:
                    VisitArray(tomlArray);
                    break;
                case TomlValue tomlValue:
                    string key = _paths.Peek();
                    _data[key] = Convert.ToString(tomlValue.ValueAsObject, CultureInfo.InvariantCulture);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(obj));
            }
        }

        private void EnterContext(string context) =>
            _paths.Push(_paths.Count > 0 ?
                _paths.Peek() + ConfigurationPath.KeyDelimiter + context :
                context);

        private void ExitContext() => _paths.Pop();
    }
}