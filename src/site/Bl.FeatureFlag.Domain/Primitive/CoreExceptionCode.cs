﻿namespace Bl.FeatureFlag.Domain.Primitive;

public enum CoreExceptionCode
{
    Unauthorized = 401,

    Forbbiden = 403,

    NotFound = 404,

    Conflict = 409,

    BadRequest = 400,
    InvalidEmail = 400_1,
    InvalidStringLength = 400_2,
    InvalidPassword = 400_3,
}