using System.ComponentModel.DataAnnotations;

namespace BankingSystem.PL.ViewModels.Manager
{
    public class BranchWorkingTimeViewModel
    {
        public int BranchId { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan Opens { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan Closes { get; set; }
    }
}