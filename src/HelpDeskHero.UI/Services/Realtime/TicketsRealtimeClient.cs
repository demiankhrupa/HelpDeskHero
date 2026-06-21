using HelpDeskHero.Shared.Contracts.Tickets;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components;

namespace HelpDeskHero.UI.Services.Realtime;

public sealed class TicketsRealtimeClient : IAsyncDisposable
{
    private readonly NavigationManager _navigationManager;
    private HubConnection? _connection;

    public event Func<TicketLiveUpdateDto, Task>? OnTicketChanged;

    public TicketsRealtimeClient(
        NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    public async Task StartAsync(
        string accessToken,
        CancellationToken ct = default)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(
                _navigationManager.ToAbsoluteUri(
                    "https://localhost:5001/hubs/tickets"),
                options =>
                {
                    options.AccessTokenProvider =
                        () => Task.FromResult(accessToken)!;
                })
            .WithAutomaticReconnect()
            .Build();

        _connection.On<TicketLiveUpdateDto>(
            "TicketChanged",
            async dto =>
            {
                if (OnTicketChanged is not null)
                {
                    await OnTicketChanged(dto);
                }
            });
        await _connection.StartAsync(ct);
        Console.WriteLine(
    $"SignalR state: {_connection.State}");
        await _connection.SendAsync("JoinDashboard", ct);
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null)
        {
            await _connection.DisposeAsync();
        }
    }
}