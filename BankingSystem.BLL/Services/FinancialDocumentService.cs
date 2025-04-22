using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using Microsoft.AspNetCore.Http;

public class FinancialDocumentService(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<string> UploadFinancialDocument(string userId, IFormFile file, string documentType, string description, DateTime? issueDate,int LoanID)
    {
        try
        {
            // Basic validation
            if (string.IsNullOrEmpty(userId))
                return "User ID is required";

            if (file == null || file.Length == 0)
                return "No file uploaded";

            // Quick PDF validation
            if (!file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                return "Only PDF files are allowed";

            // Simple size check
            if (file.Length > 10 * 1024 * 1024)
                return "File too large (max 10MB)";
            // Validate issue date
            if (!issueDate.HasValue)
                return "Issue date is required";

            // Save document
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            var doc = new FinancialDocument
            {
                CustomerId = userId,
                FileData = ms.ToArray(),
                FileName = file.FileName,
                DocumentType = documentType,
                Description = description,
                IssueDate = issueDate.Value,
                LoanId = LoanID

            };

            _unitOfWork.Repository<FinancialDocument>().Add(doc);
            _unitOfWork.Complete();

            return doc.Id.ToString(); // Return ID as string on success
        }
        catch (Exception ex)
        {
            // Log the exception details
            Console.WriteLine($"Upload failed: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
            return $"Upload failed: {ex.Message}";
        }
    }
    public FinancialDocument? GetFinancialDocument(int id) => _unitOfWork.Repository<FinancialDocument>().Get(id) ?? null;

}