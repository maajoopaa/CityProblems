using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityProblems.DataAccess.Entities
{
    public class IssueEntity
    {
        public Guid Id { get; set; } 

        public Guid CategoryId { get; set; }

        public virtual CategoryEntity Category { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Latitude { get; set; } = null!;

        public string Longitude { get; set; } = null!;

        public byte[] Image { get; set; } = null!;

        public DateTime CreatedAt { get;set; }

        public ExecutionState ExecutionState { get; set; }

        public Guid CreatedById { get; set; }

        public virtual UserEntity CreatedBy { get; set; } = null!;

    }
}
