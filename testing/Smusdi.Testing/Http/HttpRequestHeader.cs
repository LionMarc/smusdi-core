using System.Net.Mime;
using System.Text;
using Smusdi.Core.Json;

namespace Smusdi.Testing.Http;

public sealed record HttpRequestHeader(string Name, string? Value);
