namespace Smusdi.Testing.Http;

public sealed record ClaimDesc(string Type, string Value, bool Append = true, bool Replace = false);
