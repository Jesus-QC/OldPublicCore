using System;
using Core.Modules.Subclasses.Features.Structs.Subclasses;
using Exiled.API.Features;
using YamlDotNet.Core;

namespace Core.Modules.Subclasses.Features.Extensions
{
    public static class YamlExtensions
    {
        public static string ToYaml(this Subclass subclass) => Exiled.Loader.Loader.Serializer.Serialize(subclass);

        public static Subclass ToSubclass(this string yaml, string path = "")
        {
            try
            {
                return Exiled.Loader.Loader.Deserializer.Deserialize<Subclass>(yaml);
            }
            catch (YamlException yamlException)
            {
                Log.Error($"Class with path: {path} could not be loaded, Skipping. {yamlException}");
            }
            catch (FormatException e)
            {
                Log.Error($"Class with path: {path} could not be loaded due to a format exception. {e}\nBegin stack trace:\n{e.StackTrace}");
            }
            catch (Exception e)
            {
                Log.Error($"Class with path: {path} could not be loaded. {e}\nBegin stack trace:\n{e.StackTrace}");
            }

            return null;
        }
    }
}