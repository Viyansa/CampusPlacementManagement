namespace CampusPlacementManagement.DTOs.Students;

public class BulkImportResultDto
{
    public int TotalRows { get; set; }
    public int SuccessfulRows { get; set; }
    public int FailedRows { get; set; }
    public List<string> Errors { get; set; } = new();
}
