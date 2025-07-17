namespace CovidTracker.Domain.Mapping;

public static class StateCodeMapper
{
    private static readonly IReadOnlyDictionary<string, string> _stateNameToCode =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
        { "Alabama", "AL" },
        { "Alaska", "AK" },
        { "Arizona", "AZ" },
        { "Arkansas", "AR" },
        { "California", "CA" },
        { "Colorado", "CO" },
        { "Connecticut", "CT" },
        { "Delaware", "DE" },
        { "Florida", "FL" },
        { "Georgia", "GA" },
        { "Hawaii", "HI" },
        { "Idaho", "ID" },
        { "Illinois", "IL" },
        { "Indiana", "IN" },
        { "Iowa", "IA" },
        { "Kansas", "KS" },
        { "Kentucky", "KY" },
        { "Louisiana", "LA" },
        { "Maine", "ME" },
        { "Maryland", "MD" },
        { "Massachusetts", "MA" },
        { "Michigan", "MI" },
        { "Minnesota", "MN" },
        { "Mississippi", "MS" },
        { "Missouri", "MO" },
        { "Montana", "MT" },
        { "Nebraska", "NE" },
        { "Nevada", "NV" },
        { "New Hampshire", "NH" },
        { "New Jersey", "NJ" },
        { "New Mexico", "NM" },
        { "New York", "NY" },
        { "North Carolina", "NC" },
        { "North Dakota", "ND" },
        { "Ohio", "OH" },
        { "Oklahoma", "OK" },
        { "Oregon", "OR" },
        { "Pennsylvania", "PA" },
        { "Rhode Island", "RI" },
        { "South Carolina", "SC" },
        { "South Dakota", "SD" },
        { "Tennessee", "TN" },
        { "Texas", "TX" },
        { "Utah", "UT" },
        { "Vermont", "VT" },
        { "Virginia", "VA" },
        { "Washington", "WA" },
        { "West Virginia", "WV" },
        { "Wisconsin", "WI" },
        { "Wyoming", "WY" },
        // DC is often needed
        { "District of Columbia", "DC" }
        };

    private static readonly Dictionary<string, string> _stateCodeToName =
        _stateNameToCode.ToDictionary(kvp => kvp.Value, kvp => kvp.Key, StringComparer.OrdinalIgnoreCase);

    public static bool TryGetCode(string stateName, out string? stateCode)
    {
        return _stateNameToCode.TryGetValue(stateName, out stateCode);
    }

    public static bool TryGetName(string stateCode, out string? stateName)
    {
        return _stateCodeToName.TryGetValue(stateCode, out stateName);
    }
}
