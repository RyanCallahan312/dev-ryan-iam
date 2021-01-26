using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dev_ryan_iam.DTOs
{
    public record ExceptionDTO
    {
        public Exception Exception { get; init; }
        public string Message { get; init; }
    }
}
