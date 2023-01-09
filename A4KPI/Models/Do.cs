using A4KPI.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A4KPI.Models
{
    [Table("Do")]
    public class Do : IDateTracking
    {
        public Do(string content, string reusltContent, string achievement, int actionId)
        {
            Content = content;
            ReusltContent = reusltContent;
            Achievement = achievement;
            ActionId = actionId;
        }

        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public string ReusltContent { get; set; }
        public string Achievement { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public int ActionId { get; set; }
        [ForeignKey(nameof(ActionId))]
        public virtual Action Action { get; set; }
    }
}
