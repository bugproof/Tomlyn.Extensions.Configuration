using Tomlyn.Model;
using Tomlyn.Serialization;

namespace Tomlyn.Extensions.Configuration
{
    [TomlSerializable(typeof(TomlTable))]
    internal partial class TomlModelSerializerContext : TomlSerializerContext
    {
    }
}
