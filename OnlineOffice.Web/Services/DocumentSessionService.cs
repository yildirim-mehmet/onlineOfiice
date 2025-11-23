using OfficeIMO.Word;
using OfficeIMO.Excel;
using OfficeIMO.PowerPoint;
using System.Collections.Concurrent;

namespace OfficeIMO.Collaborative.Web.Services;

public abstract class DocumentSession
{
    public string SessionId { get; set; } = Guid.NewGuid().ToString();
    public string FilePath { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public List<string> ConnectedUsers { get; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastActivity { get; set; } = DateTime.UtcNow;
    public ReaderWriterLockSlim SyncLock { get; } = new();
}

public class ExcelSession : DocumentSession
{
    public ExcelDocument Document { get; set; } = null!;
}

public class WordSession : DocumentSession
{
    public WordDocument Document { get; set; } = null!;
}

public class PowerPointSession : DocumentSession
{
    public PowerPointPresentation Presentation { get; set; } = null!;
}


public class DocumentSessionService
{
    private readonly ConcurrentDictionary<string, DocumentSession> _sessions = new();
    private readonly string _uploadPath;

    public DocumentSessionService(IWebHostEnvironment env)
    {
        _uploadPath = Path.Combine(env.ContentRootPath, "App_Data", "Uploads");
        Directory.CreateDirectory(_uploadPath);
    }

    public string UploadPath => _uploadPath;

    public string CreateExcelSession(string filePath, string originalName)
    {
        var doc = ExcelDocument.Load(filePath);

        var session = new ExcelSession
        {
            SessionId = Guid.NewGuid().ToString(),
            FilePath = filePath,
            OriginalFileName = originalName,
            Document = doc
        };

        _sessions[session.SessionId] = session;
        return session.SessionId;
    }

    public ExcelSession? GetExcelSession(string sessionId)
    {
        return _sessions.TryGetValue(sessionId, out var session) ? session as ExcelSession : null;
    }

    public void UpdateActivity(string sessionId)
    {
        if (_sessions.TryGetValue(sessionId, out var session))
        {
            session.LastActivity = DateTime.UtcNow;
        }
    }

    public IEnumerable<DocumentSession> GetAllSessions() => _sessions.Values;
}
