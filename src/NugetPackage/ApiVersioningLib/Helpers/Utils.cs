using System.Text.RegularExpressions;

namespace ApiVersioningLib.Helpers;

public static class Utils
    {
    /// <summary>
    ///     Validates the version string using semantic versioning pattern.
    /// </summary>
    /// <param name="version">The version to validate.</param>
    /// <returns>True if valid, false otherwise.</returns>
    public static bool IsValidVersion(string version)
    {
        const string versionPattern = @"^\d+\.\d+\.\d+$"; // Regex for semantic versioning (e.g., 1.2.1)
        return Regex.IsMatch(version, versionPattern);
    }
    }