﻿namespace ChopChop.Extensions.Data.Model.Response;

public abstract class BaseLocalization
{
    public Guid Id { get; set; }
    public string Culture { get; set; }
}
