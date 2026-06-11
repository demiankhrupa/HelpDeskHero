using System.Net.Http.Json;
using HelpDeskHero.Shared.Contracts.Audit;

namespace HelpDeskHero.UI.Services.Api;

public sealed class AuditApiClient
{
    private readonly HttpClient _http;

    public AuditApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<AuditLogListItemDto>?> GetAsync(
        string? action = null,
        string? entityName = null,
        string? performedBy = null,
        DateTime? fromUtc = null,
        DateTime? toUtc = null,
        CancellationToken ct = default)
    {
        var url =
            $"api/audit?action={Uri.EscapeDataString(action ?? "")}" +
            $"&entityName={Uri.EscapeDataString(entityName ?? "")}" +
            $"&performedBy={Uri.EscapeDataString(performedBy ?? "")}";

        if (fromUtc.HasValue)
            url += $"&fromUtc={fromUtc.Value:O}";

        if (toUtc.HasValue)
            url += $"&toUtc={toUtc.Value:O}";

        return await _http.GetFromJsonAsync<List<AuditLogListItemDto>>(url, ct);
    }
}