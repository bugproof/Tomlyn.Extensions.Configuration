using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
            foreach (var keyValuePair in table)
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
            foreach (var tomlObj in array)
            {
                EnterContext((i++).ToString());
                VisitObject(tomlObj);
                ExitContext();
            }
        }

        private void VisitObject(object obj)
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
                // The TomlDateTime case can be deleted once https://github.com/xoofx/Tomlyn/pull/21 is merged and released
                case TomlDateTime tomlDateTime:
                    _data.Add(_paths.Peek(), tomlDateTime.ToString());
                    break;
                default:
                    _data.Add(_paths.Peek(), Convert.ToString(obj, CultureInfo.InvariantCulture));
                    break;
            }
        }

        private void EnterContext(string context) =>
            _paths.Push(_paths.Count > 0 ?
                _paths.Peek() + ConfigurationPath.KeyDelimiter + context :
                context);

        private void ExitContext() => _paths.Pop();
    }
}