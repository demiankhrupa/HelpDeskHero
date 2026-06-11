using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using HelpDeskHero.Shared.Contracts.Tickets;

namespace HelpDeskHero.Api.IntegrationTests;

public sealed class TicketsEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TicketsEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTickets_ShouldReturnSuccess()
    {
        var response = await _client.GetAsync("/api/tickets");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateTicket_ShouldReturnCreated()
    {
        var dto = new CreateTicketDto
        {
            Title = "Integration test ticket",
            Description = "Created by integration test",
            Priority = "High"
        };

        var response = await _client.PostAsJsonAsync("/api/tickets", dto);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await response.Content.ReadFromJsonAsync<TicketDto>();
        created.Should().NotBeNull();
        created!.Title.Should().Be(dto.Title);
    }
}