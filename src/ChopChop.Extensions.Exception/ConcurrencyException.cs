﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChopChop.Extensions.Exception;

public class ConcurrencyException : AppException
{
    public ConcurrencyException()
        : this("Concurrency")
    {
    }

    public ConcurrencyException(string messageId, int statusCode = 409, object[]? @params = null)
        : base(messageId, "Concurrency", statusCode, @params)
    {
    }
}
