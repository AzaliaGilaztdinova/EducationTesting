using System;

namespace EducationTesting.Client
{
    public class MomentProvider : IMomentProvider
    {
        public DateTime Now => DateTime.Now;
    }
}