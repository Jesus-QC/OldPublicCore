namespace Core.Modules.Subclasses.Features;

public class SubclassesManager
{
   /* private readonly Dictionary<RoleType, SubclassGroup> _sortedClasses = new();
    private readonly Dictionary<ushort, Subclass> _subclasses = new();

    public void Load()
    {
        _sortedClasses.Clear();
        foreach (var role in Enum.GetValues(typeof(RoleType)))
            _sortedClasses.Add((RoleType)role, new SubclassGroup());
            
        _subclasses.Clear();

        foreach (var file in Directory.GetFiles(Paths.MainFolder, "*.json"))
        {
            Log.Info(file);
            var subclass = File.ReadAllText(file).ToSubclass(file);

            if (subclass == null)
            {
                Log.Debug($"The subclass with the path \"{file}\" is null. (Skipping)", CoreSubclasses.PluginConfig.IsDebugEnabled);
                continue;
            }

            Log.Info(subclass);
                
            var id = (ushort) _subclasses.Count;

            _subclasses.Add(id, subclass);

            foreach (var role in subclass.AffectedRoles)
                _sortedClasses[role].AddSubclass(id, subclass);
        }
    }

    public SubclassGroup GetRoleSubclasses(RoleType roleType) => _sortedClasses[roleType];
    public Subclass GetSubclassById(ushort id) => _subclasses[id];*/
}