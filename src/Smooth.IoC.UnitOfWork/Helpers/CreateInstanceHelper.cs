﻿using System;

namespace Smooth.IoC.UnitOfWork.Helpers;

public static class CreateInstanceHelper
{
    public static T Resolve<T>(params object[] parameters) where T : class
    {
        return (T)Activator.CreateInstance(typeof(T), parameters);
    }
}