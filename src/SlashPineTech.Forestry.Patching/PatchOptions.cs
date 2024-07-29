namespace SlashPineTech.Forestry.Patching;

public class PatchOptions
{
    /// <summary>
    /// The connection string used to read from and write to the applied
    /// patches table.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// The name of the applied patches table.
    /// </summary>
    /// <remarks>
    /// WARNING: Renaming this after the patch system has created the table
    /// will result in it creating a new table with the new name and
    /// re-running _all_ patches.
    /// </remarks>
    public string TableName { get; set; } = "__AppliedPatches";
}
