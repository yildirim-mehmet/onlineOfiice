using Microsoft.AspNetCore.SignalR;
using OfficeIMO.Collaborative.Web.Services;

namespace OfficeIMO.Collaborative.Web.Hubs;

public class CollaborativeHub : Hub
{
    private readonly DocumentSessionService _service;

    public CollaborativeHub(DocumentSessionService service)
    {
        _service = service;
    }

    public async Task JoinExcel(string sessionId, string userName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
        _service.UpdateActivity(sessionId);

        var session = _service.GetExcelSession(sessionId);
        if (session != null && !session.ConnectedUsers.Contains(userName))
        {
            session.ConnectedUsers.Add(userName);
        }

        await Clients.Group(sessionId)
            .SendAsync("UserJoined", userName, session?.ConnectedUsers.Count ?? 0);
    }

    public async Task UpdateExcelCell(string sessionId, string sheetName, string cellAddress, object value) {
        var session = _service.GetExcelSession(sessionId);
        if (session == null) return;

        session.SyncLock.EnterWriteLock();
        try {
            var sheet = session.Document.Sheets
                .FirstOrDefault(s => s.Name == sheetName);

            if (sheet != null) {
                var cell = sheet.GetCell(cellAddress);
                cell.Value = value?.ToString();
                session.Document.Save();
            }
        } finally {
            session.SyncLock.ExitWriteLock();
        }

        await Clients.OthersInGroup(sessionId)
            .SendAsync("CellUpdated", sheetName, cellAddress, value);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Basit: şimdilik sadece bırakıyoruz (geliştirilebilir)
        await base.OnDisconnectedAsync(exception);
    }
}
