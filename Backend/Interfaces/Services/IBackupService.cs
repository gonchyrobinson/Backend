namespace Backend.Interfaces.Services
{
    public interface IBackupService
    {
        Task<byte[]> GenerateBackupAsync();
    }
}