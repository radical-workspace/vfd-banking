using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.BLL.Services
{
    public class FinancialDocumentService(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<string> UploadFinancialDocument(string userId, IFormFile file, string documentType, string description)
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

                // Save document
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);

                var doc = new FinancialDocument
                {
                    CustomerId = userId,
                    FileData = ms.ToArray(),
                    FileName = file.FileName,
                    DocumentType = documentType,
                    Description = description
                };

                _unitOfWork.Repository<FinancialDocument>().Add(doc);
                _unitOfWork.Complete();

                return doc.Id.ToString(); // Return ID as string on success
            }
            catch (Exception ex)
            {
                // Return simple error message
                return $"Upload failed: {ex.Message}";
            }
        }

        public FinancialDocument? GetFinancialDocument(int id) => _unitOfWork.Repository<FinancialDocument>().Get(id) ?? null;

    }

}

