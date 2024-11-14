using System.ComponentModel.DataAnnotations;
using static PMS.Common.EntityValidationConstants.JobOrderConstants;

namespace PMSWeb.ViewModels.JobOrderVM
{
    public class CompleteTheJobViewModel
    {
        public string JobId { get; set; }
        
        public string JobName { get; set; }

        [Required]
        [MaxLength(JobOrderDescriptionMaxLength)]
        public string Details { get; set; }
        
        public string DueDate { get; set; }
        
        public string ResponsiblePosition { get; set; }
        
        public string Equipment { get; set; }

        public string EquipmentId { get; set; }

    }
}
