using Bunit;
using FluentAssertions;
using HelpDeskHero.Shared.Contracts.Common;
using HelpDeskHero.Shared.Contracts.Tickets;
using HelpDeskHero.UI.Pages.Tickets;
using HelpDeskHero.UI.Services.Api;
using Microsoft.Extensions.DependencyInjection;

namespace HelpDeskHero.UI.Tests;

public sealed class TicketListPageTests : BunitContext
{
    [Fact]
    public void TicketListPage_ShouldRenderTicketTitle()
    {
        Services.AddSingleton<ITicketApiClient>(new FakeTicketApiClient());

        var cut = Render<TicketListPage>();

        cut.WaitForAssertion(() =>
        {
            cut.Markup.Should().Contain("Test Ticket from bUnit");
        });
    }

    private sealed class FakeTicketApiClient : ITicketApiClient
    {
        public Task<PagedResultDto<TicketDto>?> GetPageAsync(
            TicketQueryDto query,
            CancellationToken ct = default)
        {
            return Task.FromResult<PagedResultDto<TicketDto>?>(new PagedResultDto<TicketDto>
            {
                TotalCount = 1,
                Items =
                [
                    new TicketDto
                    {
                        Id = 1,
                        Number = "HDH-0001",
                        Title = "Test Ticket from bUnit",
                        Description = "desc",
                        Status = "New",
                        Priority = "High",
                        CreatedAtUtc = DateTime.UtcNow
                    }
                ]
            });
        }

        public Task<TicketDto?> GetByIdAsync(int id, CancellationToken ct = default)
            => Task.FromResult<TicketDto?>(null);

        public Task<HttpResponseMessage> CreateAsync(CreateTicketDto dto, CancellationToken ct = default)
            => Task.FromResult(new HttpResponseMessage());

        public Task<HttpResponseMessage> UpdateAsync(int id, UpdateTicketDto dto, CancellationToken ct = default)
            => Task.FromResult(new HttpResponseMessage());

        public Task<HttpResponseMessage> DeleteAsync(int id, CancellationToken ct = default)
            => Task.FromResult(new HttpResponseMessage());

        public Task<List<TicketDto>?> GetDeletedAsync(CancellationToken ct = default)
            => Task.FromResult<List<TicketDto>?>([]);

        public Task<HttpResponseMessage> RestoreAsync(int id, CancellationToken ct = default)
            => Task.FromResult(new HttpResponseMessage());
    }
}