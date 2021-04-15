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

        private readonly IDictionary<string, string> _data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Stack<string> _context = new();
        private string _currentPath;

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
            foreach (var (key, val) in table.GetTomlEnumerator())
            {
                EnterContext(key);
                VisitObject(val);
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
            var key = _currentPath;

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
                    _data[key] = Convert.ToString(tomlValue.ValueAsObject, CultureInfo.InvariantCulture);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(obj));
            }
        }

        private void EnterContext(string context)
        {
            _context.Push(context);
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }

        private void ExitContext()
        {
            _context.Pop();
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }
    }
}