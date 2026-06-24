using HelpDeskHero.Shared.Contracts.Tickets;

namespace HelpDeskHero.UI.Services.Realtime;

public interface ITicketsRealtimeClient : IAsyncDisposable
{
    event Func<TicketLiveUpdateDto, Task>? OnTicketChanged;

    Task StartAsync(
        string accessToken,
        CancellationToken ct = default);
}