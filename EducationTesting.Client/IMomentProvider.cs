using System;

namespace EducationTesting.Client
{
    public interface IMomentProvider
    {
        DateTime Now { get; }
    }
}