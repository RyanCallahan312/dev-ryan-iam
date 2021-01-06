using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace auto_highlighter_iam.DTOs
{
    public record LoginDTO
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
