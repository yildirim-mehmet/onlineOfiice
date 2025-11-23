using Microsoft.AspNetCore.SignalR;

namespace OnlineOffice.Web.Hubs;

public class TableHub : Hub
{
    // Basit placeholder: sadece client'lar arasında mesaj iletir
    public async Task BroadcastCellChange(string docId, int row, int col, string? value)
    {
        await Clients.OthersInGroup(docId).SendAsync("CellChanged", docId, row, col, value);
    }

    public async Task JoinDocument(string docId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, docId);
    }
}